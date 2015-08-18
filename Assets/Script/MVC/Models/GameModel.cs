using UnityEngine;
using System.Collections;
using System;


namespace SocialPoint.Examples.MVC {
    public class GameModel {

        // Use this for initialization
        public GameModel ( ) {
            this.ResetTimer( );
        }

        #region about event

        public event Action NewGameEvent;
        public event Action StartGameEvent;
        public event Action PauseGameEvent;
        public event Action GameOverEvent;

        public event Action UndoEvent;
        public event Action TimeOverEvent;

        public event Action SoundEvent;
        public event Action LevelOfDifficultyEvent;        

        public void NewGame ( ) {
            Debuger.Log(string.Format("GameModel->NewGame (  )"));
            OnNewGame( );
        }

        protected virtual void OnNewGame ( ) {
            if ( null != NewGameEvent ) {
                NewGameEvent( );
            }
        }

        public void PauseGame ( ) {
            Debuger.Log(string.Format("GameModel->PauseGame (  )"));
            OnPauseGame( );
        }

        protected virtual void OnPauseGame ( ) {
            if ( null != PauseGameEvent ) {
                PauseGameEvent( );
            }
        }

        public void StartGame ( ) {
            Debuger.Log(string.Format("GameModel->StartGame (  )"));
            OnStartGame( );
        }

        protected virtual void OnStartGame ( ) {
            if (null != StartGameEvent) {
                StartGameEvent( );
            }
        }

        public void GameOver ( ) {
            Debuger.Log(string.Format("GameModel->GameOver (  )"));
            OnGameOver( );
        }

        protected virtual void OnGameOver ( ) {
            if (null != GameOverEvent) {
                GameOverEvent( );
            }
        }

        public void Undo ( ) {
            Debuger.Log(string.Format("GameModel->Undo (  )"));
            OnUndo( );
        }

        // 悔棋
        protected virtual void OnUndo ( ) {
            if (null != UndoEvent) {
                UndoEvent( );
            }
        }

        public void TimeOver ( ) {
            Debuger.Log(string.Format("GameModel->TimeOver (  )"));
            OnTimeOver( );
        }

        protected virtual void OnTimeOver ( ) {
            if (null != TimeOverEvent) {
                TimeOverEvent( );
            }

            this.ResetTimer( );
        }

        public void Sound ( ) {
            Debuger.Log(string.Format("GameModel->Sound (  )"));
            OnSound( );
        }

        protected virtual void OnSound ( ) {
            if (null != SoundEvent) {
                SoundEvent( );
            }
        }

        public void LevelOfDifficulty ( ) {
            Debuger.Log(string.Format("GameModel->LevelOfDifficulty (  )"));
            OnLevelOfDifficulty( );
        }

        // 难度等级
        protected virtual void OnLevelOfDifficulty ( ) {
            if (null != LevelOfDifficultyEvent) {
                LevelOfDifficultyEvent( );
            }
        }

        #endregion about event

        public int GameTime { get; private set; }

        void InitTimer ( ) { 
       
        }

        void ResetTimer(){
        
        }

        void StartTimer ( ) { }

        void PauseTimer ( ) { }

    }
}

