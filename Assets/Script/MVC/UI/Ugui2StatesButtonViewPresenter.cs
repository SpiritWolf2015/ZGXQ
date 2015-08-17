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
        }

        /// <summary>
        /// start, pause game state Flag reverse
        /// </summary>
        public void reverse ( ) {
            Debuger.Log("状态Flag取反");
            m_isStartGame = !m_isStartGame;
        }

    }
}

