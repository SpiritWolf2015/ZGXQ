using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using com.gzc.zgxq.view;

namespace SocialPoint.Examples.MVC {

    public class GameController {

        /// <summary>
        /// 游戏界面View
        /// </summary>
        ChessAiModel m_ai;

        GameModel m_gameModel { get; set; }
        UguiWindowViewPresenter m_windowViewPresenter { get; set; }

        public GameController (string windowName) {
            InitGameModel( );
            InitView(windowName);

            InitAiModel( );
        }

        void InitGameModel ( ) {
            // data model
            m_gameModel = new GameModel( );
            // model event
            m_gameModel.NewGameEvent += ( ) => { 
                Debug.Log("NewGame业务逻辑");

                InitAiModel( );
            };
            m_gameModel.StartGameEvent += ( ) => {
                Debug.Log("StartGame业务逻辑");
            };
            m_gameModel.PauseGameEvent += ( ) => { 
                Debug.Log("PauseGame业务逻辑"); 
            };
        }

        void InitView (string windowName) {
            // ui game obj
            m_windowViewPresenter = CreateView(windowName).GetComponent<UguiWindowViewPresenter>( );
            // ui event
            m_windowViewPresenter.Button_NewGame.Clicked += ( ) => {
                m_gameModel.NewGame( );
            };

            UnityAction startAction = ( ) => {
                m_gameModel.StartGame( );
            };
            UnityAction pauseAction = ( ) => {
                m_gameModel.PauseGame( );
            };

            m_windowViewPresenter.Button_StartPauseGame.Clicked += ( ) => {
                // start, pause button event switch
                if (m_windowViewPresenter.Button_StartPauseGame.IsStartGame) {
                    m_windowViewPresenter.Button_StartPauseGame.Clicked -= pauseAction;
                    m_windowViewPresenter.Button_StartPauseGame.Clicked += startAction;
                } else {
                    m_windowViewPresenter.Button_StartPauseGame.Clicked -= startAction;
                    m_windowViewPresenter.Button_StartPauseGame.Clicked += pauseAction;
                }
            };           
        }

        GameObject CreateView ( string viewName ) {
            // Loads the prefab with the view and instantiates it inside the View hierarchy
            return GameObject.Find(viewName);
        }

        // 进入游戏界面界面
        void InitAiModel ( ) {
            m_ai = new ChessAiModel( );
            //m_ai.AiOnceMove( );
        }

    }

}


