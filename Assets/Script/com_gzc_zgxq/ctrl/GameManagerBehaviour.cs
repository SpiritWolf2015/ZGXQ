using UnityEngine;
using System.Collections;
using com.gzc.zgxq.view;

namespace com.gzc.zgxq {

    /// <summary>
    /// 游戏的控制器
    /// </summary>
    public class GameManagerBehaviour : MonoBehaviour {

        /// <summary>
        /// 游戏界面View
        /// </summary>
        GameView gameView;

        //声音处理
        //SoundPool soundPool;


        #region U3D API

        void Start ( ) {
            // 进入游戏界面界面
            this.goToGameView( );
        }

        void OnDestroy ( ) {
            if ( null != gameView ) {
                gameView.surfaceDestroyed( );
            }
            // 停止所有子线程
            ThreadManager.Instance.removeAllWorkThreads( );
        }

        #endregion U3D API

        // 进入游戏界面界面
        void goToGameView ( ) {
            gameView = new GameView( );
            gameView.onTouchEvent( );
            Debuger.Log(this + ", goToGameView");
        }

    }


}

