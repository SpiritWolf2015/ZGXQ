using UnityEngine;
using System.Collections;
using System;


namespace SocialPoint.Examples.MVC {
    public class GameModel {

        // Use this for initialization
        public GameModel ( ) {

        }


       

       

        public event Action NewGameEvent;
        public event Action StartGameEvent;
        public event Action PauseGameEvent;
        public event Action GameOverEvent;

        public event Action UndoEvent;
        public event Action TimeOverEvent;

        public event Action SoundEvent;
        public event Action LevelOfDifficultyEvent;

        public int GameTime { get; private set; }

        public void NewGame ( ) {
            Debug.Log(string.Format("GameModel->NewGame (  )"));
            OnNewGame( );
        }

        protected virtual void OnNewGame ( ) {
            if ( null != NewGameEvent ) {
                NewGameEvent( );
            }
        }

        public void PauseGame ( ) {
            Debug.Log(string.Format("GameModel->PauseGame (  )"));
            OnPauseGame( );
        }

        protected virtual void OnPauseGame ( ) {
            if ( null != PauseGameEvent ) {
                PauseGameEvent( );
            }
        }

        public void StartGame ( ) {
            Debug.Log(string.Format("GameModel->StartGame (  )"));
            OnStartGame( );
        }

        protected virtual void OnStartGame ( ) {
            if (null != StartGameEvent) {
                StartGameEvent( );
            }
        }

        public void GameOver ( ) {
            Debug.Log(string.Format("GameModel->GameOver (  )"));
            OnGameOver( );
        }

        protected virtual void OnGameOver ( ) {
            if (null != GameOverEvent) {
                GameOverEvent( );
            }
        }

        public void Undo ( ) {
            Debug.Log(string.Format("GameModel->Undo (  )"));
            OnUndo( );
        }

        // 悔棋
        protected virtual void OnUndo ( ) {
            if (null != UndoEvent) {
                UndoEvent( );
            }
        }

        public void TimeOver ( ) {
            Debug.Log(string.Format("GameModel->TimeOver (  )"));
            OnTimeOver( );
        }

        protected virtual void OnTimeOver ( ) {
            if (null != TimeOverEvent) {
                TimeOverEvent( );
            }
        }

        public void Sound ( ) {
            Debug.Log(string.Format("GameModel->Sound (  )"));
            OnSound( );
        }

        protected virtual void OnSound ( ) {
            if (null != SoundEvent) {
                SoundEvent( );
            }
        }

        public void LevelOfDifficulty ( ) {
            Debug.Log(string.Format("GameModel->LevelOfDifficulty (  )"));
            OnLevelOfDifficulty( );
        }

        // 难度等级
        protected virtual void OnLevelOfDifficulty ( ) {
            if (null != LevelOfDifficultyEvent) {
                LevelOfDifficultyEvent( );
            }
        }

    }
}

