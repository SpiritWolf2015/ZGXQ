using UnityEngine;
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
        Debuger.Log(string.Format("GameModel->ChessMove (  )"));        

        s_stack.Push(onceMove);
        AddMoveCount( );
        SwitchPlayChess( );
        OnChessMove( );
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
