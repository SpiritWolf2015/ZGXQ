using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using com.gzc.zgxq.view;

namespace SocialPoint.Examples.MVC {

    public class GameController {

       
        ChessAiModel m_ai { get; set; }
        PlayerModel m_player { get; set; }

        GameModel m_gameModel { get; set; }
        UguiWindowViewPresenter m_windowViewPresenter { get; set; }
        //玩家走棋检测球VIEW

        public GameController (string windowName) {
            InitGameModel( );
            InitView(windowName);

            InitAiModel( );
            InitPlayerModel( );
        }

        void InitGameModel ( ) {
            // data model
            m_gameModel = new GameModel( );
            // model event
            m_gameModel.NewGameEvent += ( ) => { 
                Debug.Log("NewGame业务逻辑");            
            };
            m_gameModel.StartGameEvent += ( ) => {
                Debug.Log("StartGame业务逻辑");
                //响应玩家的输入
                
                //开始计数器
                //AI下棋
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
            //  电脑走一步棋后，回调让玩家走一步棋
            m_ai.AiMoveEvent += ( ) => {
                m_player.SwitchPlayChess( );
            };
        }

        void InitPlayerModel ( ) {
            m_player = new PlayerModel( );
            //  玩家走一步棋后，回调让电脑走一步棋
            m_player.PlayerOnceMoveFinishEvent += ( ) => {
                m_ai.AiOnceMove( );
            };
        }

    }

}


