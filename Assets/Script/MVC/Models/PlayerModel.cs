﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using com.gzc.zgxq.game;

public class PlayerModel {

    /// <summary>
    /// 下棋方标志位，false为黑方(AI)下棋
    /// </summary>
    static bool s_isRedPlayChess = true;

    // 所有走棋步骤，包括玩家和电脑的。
    static Stack<StackPlayChess> s_stack = new Stack<StackPlayChess>( );

    // 走棋，从哪走
    public int MoveFrom256Index { get; set; }
    // 走棋，走到哪
    public int MoveTo256Index { get; set; }
    // 玩家总共走了几步棋
    public int MoveCount { get; private set; }

    public PlayerModel ( ) {
        s_isRedPlayChess = true;
    }

    /// <summary>
    /// 切换下棋方
    /// </summary>
    public void SwitchPlayChess ( ) {
        s_isRedPlayChess = !s_isRedPlayChess;
    }

    #region about event

    // 走一步棋事件
    public event Action PlayerOnceMoveFinishEvent;
    // 悔一步棋事件
    public event Action UndoChessMoveEvent;

    public void ChessMove (StackPlayChess onceMove) {
        // 如果下棋符合规则
        if (AiMoveSearch.LegalMove(onceMove.mvResult)) {
            Debuger.Log(string.Format("GameModel->ChessMove (  ) 下棋符合规则,mvResult= {0}, pcCaptured= {1}", onceMove.mvResult, onceMove.pcCaptured));

            // 如果没有被将军
            if (AiMoveSearch.MakeMove(onceMove.mvResult, 0)) {
                //father.playSound(2, 1);// 播放声音玩家走棋

                s_stack.Push(onceMove);
                AddMoveCount( );
                SwitchPlayChess( );
                OnChessMove( );
            }
        } else {
            Debuger.Log(string.Format("GameModel->ChessMove (  ) 下棋不符合规则,mvResult= {0}, pcCaptured= {1}", onceMove.mvResult, onceMove.pcCaptured));
        }

        //AiMoveSearch.ChangeSide( );
    }

    protected virtual void OnChessMove ( ) {
        if (null != PlayerOnceMoveFinishEvent) {
            PlayerOnceMoveFinishEvent();
        }
    }

    public void UndoChessMove (StackPlayChess onceMove) {
        Debuger.Log(string.Format("GameModel->UndoChessMove (  )"));

        s_stack.Pop( );
        SubMoveCount( );
        SwitchPlayChess( );
        OnUndoChessMove( );
    }

    protected virtual void OnUndoChessMove ( ) {
        if (null != UndoChessMoveEvent) {
            UndoChessMoveEvent( );
        }
    }

    #endregion about event

    void AddMoveCount ( ) {
        MoveCount++;
    }

    void SubMoveCount ( ) {
        MoveCount--;
    }

    

}
