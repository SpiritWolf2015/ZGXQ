using UnityEngine;
using System.Collections;

public static class TweenUtil {

    const string ITWEEN_ARGS_POSITION = "position";
    const string ITWEEN_ARGS_TIME = "time";
    const string ITWEEN_ARGS_EASETYPE = "easeType";
    const string ITWEEN_ARGS_LOOPTYPE = "looptype";

    /// <summary>
    /// 对iTween.MoveTo函数的封装
    /// </summary>
    /// <param name="go"></param>
    /// <param name="toPosition"></param>
    /// <param name="moveTime"></param>
    public static void MoveTo (GameObject go, Vector3 toPosition, float moveTime) {
        iTween.MoveTo(go, iTween.Hash(ITWEEN_ARGS_POSITION, toPosition, ITWEEN_ARGS_TIME, moveTime, ITWEEN_ARGS_LOOPTYPE, iTween.LoopType.none, ITWEEN_ARGS_EASETYPE, iTween.EaseType.linear));
    }
}
