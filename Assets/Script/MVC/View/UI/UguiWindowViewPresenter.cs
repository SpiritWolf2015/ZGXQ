using UnityEngine;
using System.Collections;

namespace SocialPoint.Examples.MVC {
    public class UguiWindowViewPresenter : UguiViewPresenter {

        public UguiButtonViewPresenter Button_NewGame;
        public Ugui2StatesButtonViewPresenter Button_StartPauseGame;        

        public UguiButtonViewPresenter Button_Undo;
        public UguiButtonViewPresenter Button_Sound;

        public override void Show ( ) {
            Button_NewGame.Show( );
            Button_StartPauseGame.Show( );      

            Button_Undo.Show( );
            Button_Sound.Show( );

            base.Show( );
        }

        public override void Hide ( ) {
            Button_NewGame.Hide( );
            Button_StartPauseGame.Hide( );  

            Button_Undo.Hide( );
            Button_Sound.Hide( );

            base.Hide( );
        }
    }
}

