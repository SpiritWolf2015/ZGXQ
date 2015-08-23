using System.Collections.Generic;
using System.Threading;
using System;
using com.gzc.zgxq.game;
using com.gzc.ThreadLockEvent;

namespace com.gzc.zgxq.view {

    /// <summary>
    /// 电脑下棋AI
    /// </summary>
    public class ChessAiModel {

        /// <summary>
        /// 下棋方标志位，false为黑方下棋
        /// </summary>
        bool isRedPlayChess;

        Stack<StackPlayChess> stack = new Stack<StackPlayChess>( );

        /// <summary>
        /// 难度数
        /// </summary>
        int length;
        /// <summary>
        /// 线程是否运行
        /// </summary>
        bool threadFlag = true;
        Thread thread;

        /// <summary>
        /// 触摸是否有效
        /// </summary>
        bool isValidTouch = true;
        /// <summary>
        /// 点击处为拖动
        /// </summary>
        bool dianjiJDT;

        /// <summary>
        /// 点在哪个格子上
        /// </summary>
        int bzcol, bzrow;


        //------------------------------------------------
        /// <summary>
        /// 棋子局势数组
        /// </summary>
        int[ ] ucpcSquares = new int[256];

        //------------------------------------------------

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChessAiModel ( ) {
            // 按钮，开始或者暂停
            ViewConstant.isnoStart = false;
            // 难度数
            length = ViewConstant.nanduXS * 4;
            // 初始化棋盘所有棋子
            AiMoveSearch.Startup( );
            // 初始化数组
            initArrays( );

            // 总时间
            ViewConstant.endTime = ViewConstant.zTime;
            // 线程是否运行
            threadFlag = true;

            //在构造函数里启动新线程
            surfaceCreated( );
        }

        // 初始化数组
        void initArrays ( ) {
            for ( int i = 0; i < 256; i++ ) {
                ucpcSquares[i] = AiMoveSearch.ucpcSquares[i];
            }
        }

        void surfaceCreated ( ) {
            // 新启动一个线程
            newThread( );
        }
        public void surfaceDestroyed ( ) {
            threadFlag = false;        
        }

        void draw ( ) {
            Debuger.Log("GameView draw函数");
            // 如果开始了
            if ( ViewConstant.isnoStart ) {

            }
        }
        // 新启动一个线程
        void newThread ( ) {
            thread = new Thread(new ThreadStart(run));
            //thread.IsBackground = true;
            // 加入线程管理类，启动线程进入就绪状态
            ThreadManager.Instance.addWorkThread(thread);
        }
        void run ( ) {
            if (ViewConstant.isnoStart) {
                if (ViewConstant.endTime - 500 < 0) {
                    // 如果电脑正在下棋，时间多了，则为电脑输了
                    if (!isValidTouch) {
                        aiLose( );
                    } else {
                        // 玩家输了，AI胜利
                        aiWin( );
                    }
                } else {
                    // 游戏正常进行，一直计时
                    ViewConstant.endTime -= 500;
                }
            }            
        }

        // 玩家胜利，AI失败
        void aiLose ( ) {
            ViewConstant.yingJMflag = true;
            AiMoveSearch.Startup( );// 初始化棋盘
            initArrays( );// 初始化数组
            ViewConstant.endTime = ViewConstant.zTime;
            ViewConstant.isnoStart = false;
            dianjiJDT = false;
        }
        // 玩家输了，AI胜利
        void aiWin ( ) {
            ViewConstant.shuJMflag = true;
            AiMoveSearch.Startup( );// 初始化棋盘
            initArrays( );// 初始化数组
            ViewConstant.endTime = ViewConstant.zTime;
            ViewConstant.isnoStart = false;
            dianjiJDT = false;
        }

        void initAI ( ) {
            AiMoveSearch.Startup( );// 初始化棋盘
            initArrays( );// 初始化数组
        }

        public void AiOnceMove ( ) {
        
            // 如果正在进行电脑下棋
            if ( !isValidTouch ) {
                return;
            }

            //...省略Android前面的代码

            // 电脑走棋
            // 启动一个线程进行电脑下棋
            Thread aiThread = new Thread(new ThreadStart( ( ) => {
                Debuger.LogWarning("GameView onTouchEvent函数，电脑走棋线程开始");
                ViewConstant.endTime = ViewConstant.zTime;// 时间初始化

                isRedPlayChess = true;// 正在下棋
                isValidTouch = false;// 正在下棋标志

                draw( );// 重绘方法

                // 电脑走棋
                AiMoveSearch.SearchMain( );

                int sqSrc = Chess_LoadUtil.SRC(AiMoveSearch.mvResult);	// 得到起始位置的数组下标
                int sqDst = Chess_LoadUtil.DST(AiMoveSearch.mvResult); // 得到目标位置的数组下标
                int pcCaptured = ucpcSquares[sqDst];// 得到目的格子的棋子
                Debuger.LogWarning(string.Format("GameView onTouchEvent函数，走法起点={0}，走法终点={1}, 得到目的格子的棋子={2}", sqSrc, sqDst, pcCaptured));

                // 线程安全事件管理器，分发事件，到UI线程处理，棋子的移动
                // AI移动棋子（U3D API），移动到 256数组【sqDst】
                AIMoveEvent aIMoveEvent = new AIMoveEvent( );
                aIMoveEvent.from = sqSrc;
                aIMoveEvent.to = sqDst;
                // 派发事件，AI移动棋子事件
                EventDispatcher.Instance( ).DispatchEvent(AIMoveEvent.AI_MOVE_EVNET, aIMoveEvent);
                Debuger.Log("GameView onTouchEvent函数->派发事件，AI移动棋子事件");

                // 电脑走一步棋
                AiMoveSearch.MakeMove(AiMoveSearch.mvResult, 0);

                StackPlayChess stackplayChess = new StackPlayChess(AiMoveSearch.mvResult, pcCaptured);
                // 下棋步骤入栈
                stack.Push(stackplayChess);

                initArrays( );// 数组操作

                // 如果电脑赢了
                if ( AiMoveSearch.IsMate( ) ) {
                    AiMoveSearch.Startup( );// 初始化棋盘
                    initArrays( );// 初始化数组
                    ViewConstant.shuJMflag = true;
                    //father.playSound(5, 1);// b播放声音,输了
                } else {
                    //father.playSound(2, 1);// b播放声音,电脑下棋了
                    isValidTouch = true;// 下完棋子，玩家可以操控了。
                }
                    
                isRedPlayChess = false;
                ViewConstant.endTime = ViewConstant.zTime;
                draw( );// 重绘方法

                Debuger.LogWarning("电脑走棋线程执行完毕!!");
                
            }));
            //aiThread.IsBackground = true;
            // 加入线程管理，并启动线程
            ThreadManager.Instance.addWorkThread(aiThread);
        }

        public event Action AiMoveFinishEvent;
        public void AiMoveFinish ( ) {
            OnAiMoveFinish( );
        }
        protected virtual void OnAiMoveFinish ( ) {
            //AiOnceMove( );

            if ( null != AiMoveFinishEvent ) {
                AiMoveFinishEvent( );
            }
        }


    }
}