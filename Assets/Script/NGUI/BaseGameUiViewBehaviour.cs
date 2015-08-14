using UnityEngine;
using System.Collections;

/// <summary>
/// 主游戏界面接口
/// </summary>
public abstract class BaseGameUiViewBehaviour : MonoBehaviour {

    #region 委托

    public delegate void NewGameHandler ( );
    public delegate void StartGameHandler ( );
    public delegate void PauseGameHandler ( );
    public delegate void UndoHandler ( );
    public delegate void TimeOverHandler ( );
    public delegate void SoundHandler ( );
    public delegate void LevelOfDifficultyHandler ( );

    #endregion 委托

    #region 事件

    public static event NewGameHandler On_NewGame;
    public static event StartGameHandler On_StartGame;
    public static event PauseGameHandler On_PauseGame;
    public static event UndoHandler On_Undo;
    public static event TimeOverHandler On_TimeOver;
    public static event SoundHandler On_Sound;
    public static event LevelOfDifficultyHandler On_LevelOfDifficulty;

    #endregion 事件

}
