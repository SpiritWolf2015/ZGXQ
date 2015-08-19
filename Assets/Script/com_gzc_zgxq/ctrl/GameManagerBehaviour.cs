using UnityEngine;
using System.Collections;
using com.gzc.zgxq.view;
using SocialPoint.Examples.MVC;
using com.gzc.ThreadLockEvent;

namespace com.gzc.zgxq {

    /// <summary>
    /// 游戏的控制器
    /// </summary>
    public class GameManagerBehaviour : MonoBehaviour {

        /// <summary>
        /// 游戏界面View
        /// </summary>
        ChessAiModel gameView;
        //声音处理
        //SoundPool soundPool;

        GameObject m_selfGo;


        #region U3D API

        void Start ( ) {
            initValue( );
            initAssetBundle( );
            initGameController( );
            initThreadLockEvent( );
        }

        void OnDestroy ( ) {
            if ( null != gameView ) {
                gameView.surfaceDestroyed( );
            }
            // 停止所有子线程
            ThreadManager.Instance.removeAllWorkThreads( );
        }

        #endregion U3D API

        void initValue ( ) {
            m_selfGo = this.gameObject;
        }

        void initAssetBundle ( ) { }

        void initGameController ( ) {
            m_selfGo.AddComponent<GameControllerInit>( );
        }

        void initThreadLockEvent ( ) {
            m_selfGo.AddComponent<EventTickerBehaviour>( );
        }

    }
}
