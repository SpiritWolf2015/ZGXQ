using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace SocialPoint.Examples.MVC {

    public class GameController {

        GameModel m_gameModel { get; set; }
        UguiWindowViewPresenter m_windowViewPresenter { get; set; }

        public GameController (string windowName) {

            #region model
            
            // data model
            m_gameModel = new GameModel( );
            // model event
            m_gameModel.NewGameEvent += ( ) => { Debug.Log("NewGame业务逻辑"); };
            m_gameModel.PauseGameEvent += ( ) => { Debug.Log("PauseGame业务逻辑"); };

            #endregion model

            #region view
            
            // ui
            m_windowViewPresenter = CreateView(windowName).GetComponent<UguiWindowViewPresenter>( );
            // ui event
            m_windowViewPresenter.Button_NewGame.Clicked += ( ) => {
                m_gameModel.NewGame( );
            };

            UnityAction a1 = ( ) => {
                m_gameModel.StartGame( );              
            };
            UnityAction a2 = ( ) => {
                m_gameModel.PauseGame( );               
            }; 
            // start, pause button event
            if (m_windowViewPresenter.Button_StartPauseGame.IsStartGame) {
                m_windowViewPresenter.Button_StartPauseGame.Clicked -= a2;
                m_windowViewPresenter.Button_StartPauseGame.Clicked += a1;
            } else {
                m_windowViewPresenter.Button_StartPauseGame.Clicked -= a1;
                m_windowViewPresenter.Button_StartPauseGame.Clicked += a2;
            }

            #endregion view
        }

        GameObject CreateView ( string viewName ) {
            // Loads the prefab with the view and instantiates it inside the View hierarchy
            return GameObject.Find(viewName);
        }

    }

}


