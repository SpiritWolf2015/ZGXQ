using System.Collections.Generic;
using System;
using com.gzc.zgxq.view;

namespace com.gzc.zgxq.game {

    /// <summary>
    /// 走法引擎主类。
    /// 该类通过最大最小搜索，alpha-beta搜索，迭代加深搜索，
    /// alpha截断，beta截断等算法来共同搜索出最佳下棋步骤。
    /// </summary>
    public static class GameLogic {

        /// <summary>
        /// 电脑走的一步棋
        /// </summary>
        public static int mvResult;

        /// <summary>
        /// 轮到谁走，0 = 红方，1 = 黑方
        /// </summary>
        public static int sdPlayer = 0;
        /// <summary>
        /// 棋盘上的棋子，搜索数组
        /// </summary>
        public static int[ ] ucpcSquares = new int[256];
        /// <summary>
        /// 红、黑双方的子力价值
        /// </summary>
        public static int vlWhite, vlBlack;
        /// <summary>
        /// 距离根节点的步数
        /// </summary>
        public static int nDistance;

        /// <summary>
        /// 历史表
        /// </summary>
        public static int[ ] nHistoryTable = new int[65536];
        
        /// <summary>
        /// 初始化历史表
        /// </summary>
        public static void InitHistorytable ( ) {
            for ( int i = 0; i < nHistoryTable.Length; i++ ) {
                nHistoryTable[i] = 0;
            }
        }


        /// <summary>
        /// 初始化棋盘
        /// </summary>
        public static void Startup ( ) {
            //sq->位置下标, pc->哪颗棋子代号
            int sq, pc;
            sdPlayer = 0;   //0为红方先走，即玩家先走。测试改为1，则是让AI先走。
            vlWhite = vlBlack = nDistance = 0;

            // 初始化为零
            for ( int i = 0; i < 256; i++ ) {
                ucpcSquares[i] = 0;
            }
            for ( sq = 0; sq < 256; sq++ ) {
                // 根据初始棋子位置数组，在棋盘上放棋子，for循环结束，棋盘上所有棋子也就按初始位置摆好了
                pc = Constant.cucpcStartup[sq];
                if ( pc != 0 ) {
                    // 如初始游戏时，sq=52(黑方左边的车), cucpcStartup[sq]为20, pc=cucpcStartup[sq]此时pc=20
                    // 黑方加分，cucvlPiecePos[pc - 16][SQUARE_FLIP(sq)]为cucvlPiecePos[4][202]=206，为黑方子力价值加206分
                    // 在棋盘上放一枚棋子
                    AddPiece(sq, pc);
                }
            }
        }


        /// <summary>
        /// 电脑走棋
        /// 迭代加深搜索过程，该方法从0到最大搜索深度进行搜索。
        /// </summary>
        public static void SearchMain ( ) {
            Debuger.Log("Loadutil->SearchMain( ) 开始");

            int i, vl;
            i = vl = 0;

            // 初始化
            InitHistorytable( );
            nDistance = 0; // 初始步数

            // 开始搜索计时
            TimeSpan tsBegin = new TimeSpan(DateTime.Now.Ticks);
            Debuger.Log("Loadutil->SearchMain( ) 开始搜索计时: " + tsBegin.ToString());

            // 迭代加深过程
            for ( i = 1; i <= Constant.LIMIT_DEPTH; i++ ) {
                // Alpha-Beta搜索
                vl = SearchFull(-Constant.MATE_VALUE, Constant.MATE_VALUE, i);
                // 搜索到杀棋，就终止搜索
                if ( vl > Constant.WIN_VALUE || vl < -Constant.WIN_VALUE ) {
                    break;
                }
                // 搜索计时
                TimeSpan tsEnd = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = tsEnd.Subtract(tsBegin).Duration( );  // 获取TimeSpan的绝对值
                Debuger.Log("Loadutil->SearchMain( ) 搜索计时 : " + ts.Minutes);
                // TODO: 纯C#的计时器怎么写，现在子线程停止不鸟。
                // 超过时间（15分钟），就终止搜索
                if ( ts.Seconds > ViewConstant.thinkDeeplyTime ) {
                    Debuger.Log(string.Format("GameLogic->SearchMain( ) 搜索时间到，停止搜索. ts.Seconds={0}", ts.Seconds));
                    break;
                }
            }

            Debuger.Log("GameLogic->SearchMain( ) 结束");
        }

        private static List<int> s_sortTempList = new List<int>( );

        /// <summary>
        /// 超出边界(Fail-Soft)的Alpha-Beta搜索过程，最主要的方法，
        /// 通过递归调用，遍历所有走法，
        /// 然后得出每个走法下的局面价值，
        /// 根据其价值的大小搜索出最佳下棋步骤。
        /// </summary>
        /// <param name="vlAlpha"></param>
        /// <param name="vlBeta"></param>
        /// <param name="nDepth"></param>
        /// <returns></returns>
        private static int SearchFull ( int vlAlpha, int vlBeta, int nDepth ) {
            Debuger.LogWarning("Loadutil->SearchFull( ) 开始");

            int i = 0, nGenMoves = 0, pcCaptured = 0;
            int vl, vlBest, mvBest;
            vl = vlBest = mvBest = 0;
            // 每一层生成的所有走法
            int[ ] moves = new int[Constant.MAX_GEN_MOVES];

            // 一个Alpha-Beta完全搜索分为以下6个阶段:

            // 1. 到达水平线，则返回局面评价值
            if ( nDepth == 0 ) {
                int value = Evaluate( );
                Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 局面评价={0}", value));
                // 局面评价
                return value;
            }

            // 2. 初始化最佳值和最佳走法
            vlBest = -Constant.MATE_VALUE; // 这样可以知道，是否一个走法都没走过(杀棋)
            mvBest = 0; // 这样可以知道，是否搜索到了Beta走法或PV走法，以便保存到历史表

            // 3. 生成全部走法，并根据历史表排序
            nGenMoves = GenerateMoves(moves);
            #region 根据历史表排序
            
            s_sortTempList.Clear( );
            // 只对产生的走法排序，其他不变
            for ( int a = 0; a < nGenMoves; a++ ) {
                s_sortTempList.Add(moves[a]);
            }            
            // 在进行alpha-beta搜索过程时，对其局面下的所有走法进行排序，排序依据为历史表，历史表数组的下标代表走法，
            // 其值为在该步骤下的深度值，所以，每次搜索时就可以先搜索最好的走法。减少对后面的搜索次数。
            s_sortTempList.Sort(( index1, index2 ) => {
                int value = GameLogic.nHistoryTable[index2] - GameLogic.nHistoryTable[index1];
                Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 历史表排序={0}", value));
                return value;
            });           
            //s_list.CopyTo(mvs); 
            for ( int a = 0; a < nGenMoves; a++ ) {
                // 用List排序好后，赋值回数组。只对产生的走法排序，数组中其他值不变
                moves[a] = s_sortTempList[a];
            }           
            for ( int i2 = 0; i2 < moves.Length; i2++ ) {
                Debuger.LogError(string.Format("moves[{0}] = {1}", i2, moves[i2]));
            }

            #endregion 根据历史表排序
            
            Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 生成走法个数={0}", nGenMoves));
            // 4. 逐一走这些走法，并进行递归
            for ( i = 0; i < nGenMoves; i++ ) {
                pcCaptured = ucpcSquares[Chess_LoadUtil.DST(moves[i])];
                Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 第 {0} 个走法, moves[{1}] = {2}, pcCaptured={3}", (i + 1), i, moves[i], pcCaptured));

                if ( MakeMove(moves[i], pcCaptured) ) {
                    // 递归
                    vl = -SearchFull(-vlBeta, -vlAlpha, nDepth - 1);
                    Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 递归vl={0}", vl));
                    UndoMakeMove(moves[i], pcCaptured);

                    // 5. 进行Alpha-Beta大小判断和截断
                    if ( vl > vlBest ) { // 找到最佳值(但不能确定是Alpha、PV还是Beta走法)
                        vlBest = vl; // "vlBest"就是目前要返回的最佳值，可能超出Alpha-Beta边界
                        if ( vl >= vlBeta ) { // 找到一个Beta走法
                            mvBest = moves[i]; // Beta走法要保存到历史表
                            break; // Beta截断
                        }
                        if ( vl > vlAlpha ) { // 找到一个PV走法
                            mvBest = moves[i]; // PV走法要保存到历史表
                            vlAlpha = vl; // 缩小Alpha-Beta边界
                        }
                    }
                }
            }
            Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 遍历所有走法完成!!"));

            // 6. 所有走法都搜索完了，把最佳走法(不能是Alpha走法)保存到历史表，返回最佳值
            if ( vlBest == -Constant.MATE_VALUE ) {
                int value = nDistance - Constant.MATE_VALUE;
                Debuger.LogWarning(string.Format("Loadutil->SearchFull( ) 如果是杀棋，就根据杀棋步数给出评价={0}" + value));
                // 如果是杀棋，就根据杀棋步数给出评价
                return value;
            }
            if ( mvBest != 0 ) {
                // 如果不是Alpha走法，就将最佳走法保存到历史表
                nHistoryTable[mvBest] += nDepth * nDepth;
                if ( nDistance == 0 ) {
                    // 搜索根节点时，总是有一个最佳走法(因为全窗口搜索不会超出边界)，将这个走法保存下来
                    mvResult = mvBest;
                }
            }

            Debuger.LogWarning("Loadutil->SearchFull( ) over, vlBest = " + vlBest);
            return vlBest;
        }

        /// <summary>
        /// 撤消走一步棋
        /// </summary>
        /// <param name="mv"></param>
        /// <param name="pcCaptured"></param>
        public static void UndoMakeMove ( int mv, int pcCaptured ) {
            nDistance--;
            ChangeSide( );// 交换走子       
            UndoMovePiece(mv, pcCaptured);// 撤销走子
        }

        /// <summary>
        /// 撤消搬一步棋的棋子，该方法要传入走棋的步骤，以及该走棋步骤下的目标位置上的棋子。
        /// 先将该棋子搬到初始位置，如果该步骤下的目标位置上的棋子不为空，即： 是吃子走法时，
        /// 把被吃掉的棋子重新搬到该位置，以此来实现撤销搬一步棋的功能。
        /// </summary>
        /// <param name="mv"></param>
        /// <param name="pcCaptured"></param>
        public static void UndoMovePiece ( int mv, int pcCaptured ) {
            int sqSrc, sqDst, pc;
            sqSrc = Chess_LoadUtil.SRC(mv);	// 得到起始位置的数组下标
            sqDst = Chess_LoadUtil.DST(mv);	// 得到目标位置的数组下标
            pc = ucpcSquares[sqDst];		// 得到目的格子的棋子
            if ( 0 != pc ) {
                DelPiece(sqDst, pc);	// 删除目标格子的棋子
            }            
            AddPiece(sqSrc, pc);	// 在起始格子上放棋子
            if ( pcCaptured != 0 ) {	// 如果目标格子上起初有棋子
                AddPiece(sqDst, pcCaptured);		// 将该棋子放在目标格子上
            }
            Debuger.LogWarning(string.Format("Loadutil->UndoMovePiece( )"));
        }

        /// <summary>
        /// 走一步棋，该方法先走一步棋，如果走棋后被将军了，则撤销该走棋步骤，并返回false，表示将军，此步不能走，撤销走法
        /// </summary>
        /// <param name="mv">走棋步骤</param>
        /// <param name="pcCaptured">原来目标格子上的棋子</param>
        /// <returns></returns>
        public static bool MakeMove ( int mv, int pcCaptured ) {
            pcCaptured = MovePiece(mv);
            Debuger.LogWarning(string.Format("GameLogic->MakeMove( )开始"));

            if ( Checked( ) ) {
                UndoMovePiece(mv, pcCaptured);
                Debuger.LogWarning(string.Format("GameLogic->MakeMove( ) false"));
                return false;
            }
            ChangeSide( );
            nDistance++;
            Debuger.LogWarning(string.Format("GameLogic->MakeMove( ) true"));
            return true;
        }


        /// <summary>
        /// 搬一步棋的棋子
        /// </summary>
        /// <param name="mv">走棋步骤</param>
        /// <returns>返回原来目标格子上的棋子</returns>
        public static int MovePiece ( int mv ) {
            Debuger.LogWarning(string.Format("Loadutil->MovePiece( ) 开始, mv = {0}", mv));

            int sqSrc, sqDst, pc, pcCaptured;

            sqSrc = Chess_LoadUtil.SRC(mv);	// 得到起始位置的数组下标
            sqDst = Chess_LoadUtil.DST(mv);	// 得到目标位置的数组下标

            pcCaptured = ucpcSquares[sqDst];		// 得到目的格子的棋子
            Debuger.LogWarning(string.Format("Loadutil->MovePiece( ) 得到目的格子的棋子pcCaptured ={0}", pcCaptured));

            if ( pcCaptured != 0 ) {// 目的地不为空
                DelPiece(sqDst, pcCaptured);// 删掉目标格子棋子
            }
            pc = ucpcSquares[sqSrc];// 得到初始格子上的棋子
            Debuger.LogWarning(string.Format("Loadutil->MovePiece( ) 得到初始格子上的棋子pc ={0}", pc));

            if ( 0 != pc ) {
                DelPiece(sqSrc, pc);// 删掉初始格子上的棋子
                Debuger.LogWarning(string.Format("Loadutil->MovePiece( ) 删掉初始格子上的棋子sqSrc={0}, pc ={1}", sqSrc, pc));
                AddPiece(sqDst, pc);// 在目标格子上放上棋子
            }   
            
            Debuger.LogWarning(string.Format("Loadutil->MovePiece( ) pcCaptured={0}", pcCaptured));
            return pcCaptured;// 返回原来目标格子上的棋子
        }


        /// <summary>
        /// 在棋盘上放一枚棋子
        /// </summary>
        /// <param name="sq">位置下标</param>
        /// <param name="pc">哪颗棋子代号</param>
        public static void AddPiece ( int sq, int pc ) {
            Debuger.LogWarning(string.Format("Loadutil->AddPiece( ) 开始"));
            //try {
                ucpcSquares[sq] = pc;
                Debuger.LogWarning(string.Format("Loadutil->AddPiece( ) sq = {0}, pc = {1}", sq, pc));

                // 红方加分，黑方(注意"cucvlPiecePos"取值要颠倒)减分
                // 小于16是红方棋子，大于16为黑方棋子，0表示没棋子
                if ( pc < 16 ) {
                    vlWhite += Constant.cucvlPiecePos[pc - 8][sq];
                    Debuger.LogWarning(string.Format("Loadutil->AddPiece( ) vlWhite = {0}", vlWhite));
                } else {
                    vlBlack += Constant.cucvlPiecePos[pc - 16][Chess_LoadUtil.SQUARE_FLIP(sq)];
                    Debuger.LogWarning(string.Format("Loadutil->AddPiece( ) vlBlack = {0}", vlBlack));
                }
            //} catch ( IndexOutOfRangeException e ) {
            //    Debuger.LogError(string.Format("LoadUtil->AddPiece()异常：Message={0}, Source={1}, StackTrace={2}, TargetSite={3}", e.Message, e.Source, e.StackTrace, e.TargetSite));
            //}
        }

        /// <summary>
        /// 从棋盘上拿走一枚棋子，当拿走一枚棋子时，要将拿走的该棋子对应该位子的子力价值从该方在该局下的局面价值上减去
        /// </summary>
        /// <param name="sq">位置下标</param>
        /// <param name="pc">哪颗棋子</param>
        public static void DelPiece ( int sq, int pc ) {
            Debuger.LogWarning(string.Format("GameLogic->DelPiece() sq = {0}, pc = {1}", sq, pc));

            ucpcSquares[sq] = 0;
            //try {
                // 红方减分，黑方(注意"cucvlPiecePos"取值要颠倒)加分
                if ( pc < 16 ) {
                    vlWhite -= Constant.cucvlPiecePos[pc - 8][sq];
                } else {
                    vlBlack -= Constant.cucvlPiecePos[pc - 16][Chess_LoadUtil.SQUARE_FLIP(sq)];
                }

                Debuger.LogWarning(string.Format("GameLogic->DelPiece()完成 vlWhite = {0}, vlBlack = {1}", vlWhite, vlBlack));
            //} catch ( IndexOutOfRangeException e ) {
            //    Debuger.LogError(string.Format("LoadUtil->DelPiece()异常：Message={0}, Source={1}, StackTrace={2}, TargetSite={3}", e.Message, e.Source, e.StackTrace, e.TargetSite));
            //}          
        }

        /// <summary>
        /// 生成所有走法，该方法根据该局面来生成所有该方的走棋步骤
        /// </summary>
        /// <param name="mvs"></param>
        /// <returns></returns>
        public static int GenerateMoves ( int[ ] mvs ) {
            int i, j, nGenMoves, nDelta, sqSrc, sqDst;
            int pcSelfSide, pcOppSide, pcSrc, pcDst;

            // 生成所有走法，需要经过以下2个步骤：
            nGenMoves = 0;
            pcSelfSide = Chess_LoadUtil.SIDE_TAG(sdPlayer);
            pcOppSide = Chess_LoadUtil.OPP_SIDE_TAG(sdPlayer);

            for ( sqSrc = 0; sqSrc < 256; sqSrc++ ) {
                // 1. 找到一个本方棋子，再做以下判断：
                pcSrc = ucpcSquares[sqSrc];
                if ( (pcSrc & pcSelfSide) == 0 ) {
                    continue;
                }

                // 2. 根据棋子确定走法
                switch ( pcSrc - pcSelfSide ) {
                    // 帅(将)
                    case Constant.PIECE_KING:
                        for ( i = 0; i < 4; i++ ) {
                            sqDst = sqSrc + Constant.ccKingDelta[i];
                            if ( !Chess_LoadUtil.IN_FORT(sqDst) ) {
                                continue;
                            }
                            pcDst = ucpcSquares[sqDst];
                            if ( (pcDst & pcSelfSide) == 0 ) {
                                mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);// 根据起点和终点获得走法
                                Debuger.LogWarning(string.Format("GameLogic->GenerateMoves(),帅(将)moves[{0}] = {1}", nGenMoves, mvs[nGenMoves]));
                                nGenMoves++;
                            }
                        }
                        break;
                    // 士
                    case Constant.PIECE_ADVISOR:
                        for ( i = 0; i < 4; i++ ) {
                            sqDst = sqSrc + Constant.ccAdvisorDelta[i];
                            if ( !Chess_LoadUtil.IN_FORT(sqDst) ) {
                                continue;
                            }
                            pcDst = ucpcSquares[sqDst];
                            if ( (pcDst & pcSelfSide) == 0 ) {
                                mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                nGenMoves++;
                            }
                        }
                        break;
                    //小兵
                    case Constant.PIECE_BISHOP:
                        for ( i = 0; i < 4; i++ ) {
                            sqDst = sqSrc + Constant.ccAdvisorDelta[i];
                            if ( !(Chess_LoadUtil.IN_BOARD(sqDst) && Chess_LoadUtil.HOME_HALF(sqDst, sdPlayer) && ucpcSquares[sqDst] == 0) ) {
                                continue;
                            }
                            sqDst += Constant.ccAdvisorDelta[i];
                            pcDst = ucpcSquares[sqDst];
                            if ( (pcDst & pcSelfSide) == 0 ) {
                                mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                nGenMoves++;
                            }
                        }
                        break;
                    case Constant.PIECE_KNIGHT:
                        for ( i = 0; i < 4; i++ ) {
                            sqDst = sqSrc + Constant.ccKingDelta[i];
                            if ( ucpcSquares[sqDst] != 0 ) {
                                continue;
                            }
                            for ( j = 0; j < 2; j++ ) {
                                sqDst = sqSrc + Constant.ccKnightDelta[i][j];
                                if ( !Chess_LoadUtil.IN_BOARD(sqDst) ) {
                                    continue;
                                }
                                pcDst = ucpcSquares[sqDst];
                                if ( (pcDst & pcSelfSide) == 0 ) {
                                    mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                    nGenMoves++;
                                }
                            }
                        }
                        break;
                    case Constant.PIECE_ROOK:
                        for ( i = 0; i < 4; i++ ) {
                            nDelta = Constant.ccKingDelta[i];
                            sqDst = sqSrc + nDelta;
                            while ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                                pcDst = ucpcSquares[sqDst];
                                if ( pcDst == 0 ) {
                                    mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                    nGenMoves++;
                                } else {
                                    if ( (pcDst & pcOppSide) != 0 ) {
                                        mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                        nGenMoves++;
                                    }
                                    break;
                                }
                                sqDst += nDelta;
                            }
                        }
                        break;
                    case Constant.PIECE_CANNON:
                        for ( i = 0; i < 4; i++ ) {
                            nDelta = Constant.ccKingDelta[i];
                            sqDst = sqSrc + nDelta;
                            while ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                                pcDst = ucpcSquares[sqDst];
                                if ( pcDst == 0 ) {
                                    mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                    nGenMoves++;
                                } else {
                                    break;
                                }
                                sqDst += nDelta;
                            }
                            sqDst += nDelta;
                            while ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                                pcDst = ucpcSquares[sqDst];
                                if ( pcDst != 0 ) {
                                    if ( (pcDst & pcOppSide) != 0 ) {
                                        mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                        nGenMoves++;
                                    }
                                    break;
                                }
                                sqDst += nDelta;
                            }
                        }
                        break;
                    case Constant.PIECE_PAWN:
                        sqDst = Chess_LoadUtil.SQUARE_FORWARD(sqSrc, sdPlayer);// 格子水平镜像
                        if ( Chess_LoadUtil.IN_BOARD(sqDst) ) {// 判断棋子是否在棋盘中
                            pcDst = ucpcSquares[sqDst];
                            if ( (pcDst & pcSelfSide) == 0 ) {
                                mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                nGenMoves++;
                            }
                        }
                        if ( Chess_LoadUtil.AWAY_HALF(sqSrc, sdPlayer) ) {
                            for ( nDelta = -1; nDelta <= 1; nDelta += 2 ) {
                                sqDst = sqSrc + nDelta;
                                if ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                                    pcDst = ucpcSquares[sqDst];
                                    if ( (pcDst & pcSelfSide) == 0 ) {
                                        mvs[nGenMoves] = Chess_LoadUtil.MOVE(sqSrc, sqDst);
                                        nGenMoves++;
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            return nGenMoves;
        }

        /// <summary>
        /// 局面评价函数
        /// </summary>
        /// <returns></returns>
        public static int Evaluate ( ) {
            return (sdPlayer == 0 ? vlWhite - vlBlack : vlBlack - vlWhite) + Constant.ADVANCED_VALUE;
        }


        /// <summary>
        /// 判断是否被将军
        /// </summary>
        /// <returns></returns>
        public static bool Checked ( ) {
            int i, j, sqSrc, sqDst;
            int pcSelfSide, pcOppSide, pcDst, nDelta;

            pcSelfSide = Chess_LoadUtil.SIDE_TAG(sdPlayer);// 获得红黑标记(红子是8，黑子是16)
            pcOppSide = Chess_LoadUtil.OPP_SIDE_TAG(sdPlayer);// 获得红黑标记，对方的

            // 找到棋盘上的帅(将)，再做以下判断：

            for ( sqSrc = 0; sqSrc < 256; sqSrc++ ) {
                if ( ucpcSquares[sqSrc] != pcSelfSide + Constant.PIECE_KING ) {
                    continue;
                }

                // 1. 判断是否被对方的兵(卒)将军
                if ( ucpcSquares[Chess_LoadUtil.SQUARE_FORWARD(sqSrc, sdPlayer)] == pcOppSide
                        + Constant.PIECE_PAWN ) {
                    return true;
                }
                for ( nDelta = -1; nDelta <= 1; nDelta += 2 ) {
                    if ( ucpcSquares[sqSrc + nDelta] == pcOppSide + Constant.PIECE_PAWN ) {
                        return true;
                    }
                }

                // 2. 判断是否被对方的马将军(以仕(士)的步长当作马腿)
                for ( i = 0; i < 4; i++ ) {
                    if ( ucpcSquares[sqSrc + Constant.ccAdvisorDelta[i]] != 0 ) {
                        continue;
                    }
                    for ( j = 0; j < 2; j++ ) {
                        int pcDstt = ucpcSquares[sqSrc + Constant.ccKnightCheckDelta[i][j]];
                        if ( pcDstt == pcOppSide + Constant.PIECE_KNIGHT ) {
                            return true;
                        }
                    }
                }

                // 3. 判断是否被对方的车或炮将军(包括将帅对脸)
                for ( i = 0; i < 4; i++ ) {
                    nDelta = Constant.ccKingDelta[i];
                    sqDst = sqSrc + nDelta;
                    while ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                        pcDst = ucpcSquares[sqDst];
                        if ( pcDst != 0 ) {
                            if ( pcDst == pcOppSide + Constant.PIECE_ROOK || pcDst == pcOppSide + Constant.PIECE_KING ) {
                                return true;
                            }
                            break;
                        }
                        sqDst += nDelta;
                    }
                    sqDst += nDelta;
                    while ( Chess_LoadUtil.IN_BOARD(sqDst) ) {
                        pcDst = ucpcSquares[sqDst];
                        if ( pcDst != 0 ) {
                            if ( pcDst == pcOppSide + Constant.PIECE_CANNON ) {
                                return true;
                            }
                            break;
                        }
                        sqDst += nDelta;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 判断是否被杀，该方法在该局面下生成所有的走法，每一个走法走完后，判断是否被将军，
        /// 如果所有的走法下都被将军，说明该方已经被将死，即被杀。只要有一个走法下没有被将军，
        /// 则说明还没有被将死，即没有被杀死。
        /// </summary>
        /// <returns></returns>
        public static bool IsMate ( ) {
            int i, nGenMoveNum, pcCaptured;
            int[ ] mvs = new int[Constant.MAX_GEN_MOVES];

            nGenMoveNum = GenerateMoves(mvs);
            for ( i = 0; i < nGenMoveNum; i++ ) {
                pcCaptured = MovePiece(mvs[i]);
                if ( !Checked( ) ) {
                    UndoMovePiece(mvs[i], pcCaptured);
                    return false;
                } else {
                    UndoMovePiece(mvs[i], pcCaptured);
                }
            }
            return true;
        }

        /// <summary>
        /// 交换走子方
        /// </summary>
        public static void ChangeSide ( ) {
            sdPlayer = 1 - sdPlayer;
        }


    } // end class
} // end namespace
