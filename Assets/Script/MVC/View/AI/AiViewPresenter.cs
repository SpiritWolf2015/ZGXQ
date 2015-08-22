using UnityEngine;
using System.Collections;
using System;
using com.gzc.ThreadLockEvent;
using com.gzc.zgxq.game;

namespace SocialPoint.Examples.MVC {
    /// <summary>
    /// AI移动棋子
    /// </summary>
    public class AiViewPresenter : BaseViewPresenter {

        public float m_moveTime = 3F;

        #region U3D API

        // 注册AI移动棋子事件处理
        void OnEnable ( ) {
            InitEvent( );
        }

        void OnDisable ( ) {
            // 移除事件处理
            UnsubscribeEvent( );
        }

        void OnDestroy ( ) {
            // 移除事件处理
            UnsubscribeEvent( );
        }

        #endregion U3D API

        // 移除事件处理
        void UnsubscribeEvent ( ) {
            EventDispatcher.Instance( ).UnregistEventListener(AIMoveEvent.AI_MOVE_EVNET, OnAiMove);
            Debuger.Log("移除监听事件:" + AIMoveEvent.AI_MOVE_EVNET);
        }

        //监听事件
        void InitEvent ( ) {
            EventDispatcher.Instance( ).RegistEventListener(AIMoveEvent.AI_MOVE_EVNET, OnAiMove);
            Debuger.Log("监听事件:" + AIMoveEvent.AI_MOVE_EVNET);
        }

        // 响应工作线程发出的AI移动棋子事件，用ITWEEN移动棋子game obj到对应的位置上
        void OnAiMove ( EventBase eb ) {
            AIMoveEvent aIMoveEvent = eb.eventValue as AIMoveEvent;
            Debug.Log(string.Format("事件回调:{0}, from={1}, to={2}", AIMoveEvent.AI_MOVE_EVNET, aIMoveEvent.from, aIMoveEvent.to));

            if (aIMoveEvent != null) {
                // 得到下标检测球GameObject
                GameObject indexSphereGo = IndexCtrlBehaviour.instance.getIndexSphereGo(aIMoveEvent.from);
                if (indexSphereGo == null) {
                    Debuger.LogError(string.Format("该256下标{0} indexSphereGo is null", aIMoveEvent.from));
                    return;
                }

                // 得到下标检测球上的IndexTriger脚本
                IndexTrigerBehaviour indexTriger = indexSphereGo.GetComponent<IndexTrigerBehaviour>( );
                if (indexTriger != null) {
                    // 如果该检测球上有棋子，则移动棋子到该次下棋的终点的检测球位置
                    if (indexTriger.HasQiZi) {
                        Debug.Log(string.Format("AI下棋，将棋子从256数组下标{0}移到下标{1}", aIMoveEvent.from, aIMoveEvent.to));
                        // 用ITWEEN移动棋子game obj
                        TweenUtil.MoveTo(indexTriger.QiZiGameObject, IndexCtrlBehaviour.instance.getIndexSphereGo(aIMoveEvent.to).GetComponent<Transform>( ).localPosition, m_moveTime);

                        Action finishAction = AiOnceMoveFinish;
                        StartCoroutine(DelayToInvokeUtil.DelayToInvokeDo(finishAction, m_moveTime));
                    }
                } else {
                    Debug.LogError("indexTriger is null !!");
                }
            } else {
                Debug.LogError("aIMoveEvent is null !!");
            }
        }

        //===========View 对外事件===============

        public event Action AiOnceMoveFinishEvent;

        void AiOnceMoveFinish (   ) {
            OnAiOnceMoveFinish();
        }

        /// <summary>
        /// 玩家下定一步棋
        /// </summary> 
        protected virtual void OnAiOnceMoveFinish (   ) {
            // Do not propagate the click event if the button is disabled
            if ( !IsEnabled ) {
                return;
            }

            if ( AiOnceMoveFinishEvent != null ) {
                AiOnceMoveFinishEvent();
            }
        }

        public bool IsEnabled {
            get {
                return this.enabled;
            }
        }
        // 启用，暂停ITWEEN移动game obj
        public override void Enable ( ) {
         
        }

        public override void Disable ( ) {
            
        }

    }
}

