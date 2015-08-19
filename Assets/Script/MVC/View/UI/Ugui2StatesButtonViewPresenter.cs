using UnityEngine;
using System.Collections;

namespace SocialPoint.Examples.MVC {
    // 初始为开始游戏按钮，点击后变为暂停游戏按钮
    public class Ugui2StatesButtonViewPresenter : UguiButtonViewPresenter {
       
        //true=>Start Game, false =>Pause Game
        private bool m_isStartGame;
        public bool IsStartGame { get { return m_isStartGame; } }

        protected override void AwakeUnityMsg ( ) {
            base.AwakeUnityMsg( );

            init( );
        }

        void init ( ) {
            m_isStartGame = true;
        }

        public override void OnButtonClicked ( ) {
            base.OnButtonClicked( );

            reverse( );
            switchText( );
        }

        /// <summary>
        /// start, pause game state Flag reverse
        /// </summary>
        void reverse ( ) {
            //Debuger.Log(string.Format("m_isStartGame={0}", m_isStartGame));
            m_isStartGame = !m_isStartGame;
        }

        const string START_GAME = "start game";
        const string PAUSE_GAME = "pause game";
        void switchText ( ) {
            if (m_isStartGame) {
                base.Text = START_GAME;
            } else {
                base.Text = PAUSE_GAME;
            }
        }

    }
}

