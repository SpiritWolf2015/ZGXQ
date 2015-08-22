using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using com.gzc.zgxq.view;
using com.gzc.zgxq.game;

namespace SocialPoint.Examples.MVC {

    public class GameController {

        #region MODEL

        /// <summary>
       /// AI走棋
       /// </summary>
        ChessAiModel m_ai { get; set; }
        PlayerModel m_playerModel { get; set; }

        GameModel m_gameModel { get; set; }

        #endregion MODEL

        #region VIEW

        UguiWindowViewPresenter m_windowViewPresenter { get; set; }
        PlayerRootViewPresenter m_playerRootViewPresenter { get; set; }
        AiViewPresenter m_aiViewPresenter { get; set; }
        //玩家走棋检测球VIEW

        #endregion VIEW

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windowName"></param>
        public GameController (string windowName) {
            InitView(windowName);

            InitGameModel( );
            InitAiModel( );
            InitPlayerModel( );
        }

        void InitGameModel ( ) {
            // data model
            m_gameModel = new GameModel( );
            // model event
            m_gameModel.NewGameEvent += ( ) => { 
                Debuger.Log("NewGame业务逻辑");            
            };
            m_gameModel.StartGameEvent += ( ) => {
                Debuger.Log("StartGame业务逻辑");
                // 响应玩家的输入
                m_playerRootViewPresenter.Enable( );
                PlayerChessOnceMove( );

                // 开始计时器
            
            };
            m_gameModel.PauseGameEvent += ( ) => {
                Debuger.Log("PauseGame业务逻辑");
                // 停止响应玩家的输入
                m_playerRootViewPresenter.Disable( );
                // 暂停计时器
                // 停止AI下棋
            };
        }
            
        void InitAiModel ( ) {
            m_ai = new ChessAiModel( );
            //  电脑走一步棋后，回调让玩家走一步棋
            //m_ai.AiMoveFinishEvent += ( ) => {
            //    m_playerModel.SwitchPlayChess( );
            //    // 启用玩家走棋drag棋子输入
            //    m_playerRootViewPresenter.Enable( );
            //};
            AiChessOnceMove( );
        }

        void InitPlayerModel ( ) {
            m_playerModel = new PlayerModel( );
            //  玩家走一步棋后，回调让电脑走一步棋
            m_playerModel.PlayerOnceMoveFinishEvent += ( ) => {
                m_ai.AiOnceMove( );
                // 禁用玩家走棋drag棋子输入
                m_playerRootViewPresenter.Disable( );
            };
        }
        
        void InitView ( string windowName ) {
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
                if ( m_windowViewPresenter.Button_StartPauseGame.IsStartGame ) {
                    m_windowViewPresenter.Button_StartPauseGame.Clicked -= pauseAction;
                    m_windowViewPresenter.Button_StartPauseGame.Clicked += startAction;
                } else {
                    m_windowViewPresenter.Button_StartPauseGame.Clicked -= startAction;
                    m_windowViewPresenter.Button_StartPauseGame.Clicked += pauseAction;
                }
            };

            // player drag chess piece input view
            m_playerRootViewPresenter = CreateView("Red").GetComponent<PlayerRootViewPresenter>( );
            // ai move chess piece game obj view
            m_aiViewPresenter = CreateView("Script").GetComponent<AiViewPresenter>( );
        }
        
        // 注册玩家下完一步棋事件的回调处理
        void PlayerChessOnceMove ( ) {
            Action<StackPlayChess> playerOnceMoveFinish = (stackPlayChess) => {
                m_playerModel.ChessMove(stackPlayChess);
            };

            foreach (BoxCollider qiziBox in m_playerRootViewPresenter.HashPlayerQiZis.Values) {
                GameObject qizi = qiziBox.gameObject;
                PlayerDragViewPresenter playerDragViewPresenter = qizi.GetComponent<PlayerDragViewPresenter>( );
                playerDragViewPresenter.ChessMoveFinishEvent += playerOnceMoveFinish;
            }
        }

        // 电脑下完一步棋
        void AiChessOnceMove ( ) {
            m_aiViewPresenter.AiOnceMoveFinishEvent += ( ) => {
                m_playerModel.SwitchPlayChess( );
                // 启用玩家走棋drag棋子输入
                m_playerRootViewPresenter.Enable( );
            };
        }
        
        GameObject CreateView ( string viewName ) {
            // Loads the prefab with the view and instantiates it inside the View hierarchy
            return GameObject.Find(viewName);
        }
        
    }
}