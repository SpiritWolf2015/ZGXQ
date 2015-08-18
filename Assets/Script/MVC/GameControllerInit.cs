using UnityEngine;
using System.Collections;


namespace SocialPoint.Examples.MVC {
    public class GameControllerInit : MonoBehaviour {

        public string m_windowName = "window";
        void Start ( ) {
            GameController gameController = new GameController(m_windowName);
        }
    }
}

