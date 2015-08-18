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
            initWindow( );
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

        void initWindow ( ) {
            m_selfGo.AddComponent<GameControllerInit>( );
        }

        void initThreadLockEvent ( ) {
            m_selfGo.AddComponent<EventTickerBehaviour>( );
        }

        #region 游戏逻辑控制

        void onNewGame ( ) { }

        void onStartGame ( ) { }

        void onPauseGame ( ) { }

        // 悔棋
        void onUndo ( ) { }

        void onTimeOver ( ) { }

        void onSound ( ) { }

        // 难度等级
        void onLevelOfDifficulty ( ) { }

        #endregion 游戏逻辑控制

    }
}
