using UnityEngine;
using System.Collections;
using System;
using com.gzc.zgxq.game;

namespace SocialPoint.Examples.MVC {
    /// <summary>
    /// 挂在棋子的脚本，拖动棋子触屏处理
    /// </summary>
    public class PlayerDragViewPresenter : PlayerRootViewPresenter {

        /// <summary>
        /// 棋子所在位置Index
        /// </summary>
        public IndexTrigerBehaviour m_IndexTriger { set; get; }

        const float WORLD_COORDINATE_TOUCH_Z = 5F;

        Transform m_selfTransform;
        GameObject m_selfGameObject;

        // 注册Drag触屏事件处理
        void OnEnable ( ) {
            EasyTouch.On_Drag += On_Drag;
            EasyTouch.On_DragStart += On_DragStart;
            EasyTouch.On_DragEnd += On_DragEnd;
        }

        void OnDisable ( ) {
            // 移除事件处理
            UnsubscribeEvent( );
        }

        void Start ( ) {
            m_selfTransform = this.transform;
            m_selfGameObject = this.gameObject;
        }

        void OnDestroy ( ) {
            // 移除事件处理
            UnsubscribeEvent( );
        }

        // 移除事件处理
        void UnsubscribeEvent ( ) {
            EasyTouch.On_Drag -= On_Drag;
            EasyTouch.On_DragStart -= On_DragStart;
            EasyTouch.On_DragEnd -= On_DragEnd;
        }

        #region Drag触屏事件处理

        // At the drag beginning 
        void On_DragStart (Gesture gesture) {
            if (gesture.pickObject == m_selfGameObject) {
                // 如果是自己的棋子
                m_selfGameObject.renderer.material.color = Color.red;

                byte xzgz = (byte)((m_IndexTriger.m_i + 3) * 16 + m_IndexTriger.m_j + 3);// 选中的棋子的格子是这么多，也就是其在256数组的下标
                IndexCtrlBehaviour.s_xzgz = xzgz;
                Debuger.LogWarning("选中棋子" + this.name + "原始位置为" + m_IndexTriger.name + "位置上！其在的256数组的下标为" + xzgz);

                int sqDst = Chess_LoadUtil.DST(xzgz + ((m_IndexTriger.m_i + 3) * 16 + m_IndexTriger.m_j + 3) * 256);       // 选中的棋子在256数组的下标     
                int pcCaptured = IndexCtrlBehaviour.m_qiZiPos[sqDst];// 通过256数组的下标值，得到选中格子是哪颗棋子
                int mv = xzgz + ((m_IndexTriger.m_i + 3) * 16 + m_IndexTriger.m_j + 3) * 256;   // 走一步棋的走法
                Debuger.LogWarning("sqDst = " + sqDst + ", 得到目的格子的棋子pcCaptured = " + pcCaptured + ", mv = " + mv);
            }
        }

        // During the drag
        void On_Drag (Gesture gesture) {
            if (gesture.pickObject == m_selfGameObject) {
                // the world coordinate from touch for z=5
                Vector3 position = gesture.GetTouchToWordlPoint(WORLD_COORDINATE_TOUCH_Z);
                position.z = 0;
                m_selfTransform.position = position;
            }
        }

        // At the drag end
        void On_DragEnd (Gesture gesture) {
            if (gesture.pickObject == m_selfGameObject) {
                m_selfGameObject.renderer.material.color = Color.white;

                if (null != m_IndexTriger) {
                    //设置棋子落下的位置
                    this.transform.localPosition = m_IndexTriger.transform.localPosition;
                } else {
                    Debuger.LogError("m_IndexTriger is null !!");
                    return;
                }   
              
                byte xzgz = (byte)(16 * (m_IndexTriger.m_i + 3) + (3 + m_IndexTriger.m_j));// 落下棋子的格子是这么多，也就是其在256数组的下标
                IndexCtrlBehaviour.s_xzgz = xzgz;
                Debuger.LogWarning("棋子" + this.name + "落子到了" + m_IndexTriger.name + "位置上！其在的256数组的下标为" + xzgz);

                //copy drag start..
                int sqDst = Chess_LoadUtil.DST(xzgz + ((m_IndexTriger.m_i + 3) * 16 + m_IndexTriger.m_j + 3) * 256);       // 选中的棋子在256数组的下标     
                int pcCaptured = IndexCtrlBehaviour.m_qiZiPos[sqDst];// 通过256数组的下标值，得到选中格子是哪颗棋子
                int mv = xzgz + ((m_IndexTriger.m_i + 3) * 16 + m_IndexTriger.m_j + 3) * 256;   // 走一步棋的走法
                Debuger.LogWarning("sqDst = " + sqDst + ", 得到目的格子的棋子pcCaptured = " + pcCaptured + ", mv = " + mv);

                StackPlayChess stackPlayChess = new StackPlayChess(mv, pcCaptured);
                ChessOnceMove(stackPlayChess);
            }
        }

        #endregion Drag触屏事件处理

        //===========View 对外事件===============

        public event Action<StackPlayChess> ChessMoveOverEvent;

        void ChessOnceMove (StackPlayChess stackPlayChess) {
            OnChessMoveOver(stackPlayChess);
        }

        /// <summary>
        /// 玩家下定一步棋
        /// </summary> 
       protected virtual void OnChessMoveOver (StackPlayChess stackPlayChess) {
            // Do not propagate the click event if the button is disabled
            if (!IsEnabled) {
                return;
            }

            if (ChessMoveOverEvent != null) {
                ChessMoveOverEvent(stackPlayChess);
            }
        }

        public bool IsEnabled {
            get {
                return this.enabled;
            }
        }

        public override void Enable ( ) {
            this.enabled = true;
        }

        public override void Disable ( ) {
            this.enabled = false;
        }

    }
}

