using UnityEngine;
using System.Collections;

/// <summary>
/// AI移动棋子
/// </summary>
public class AICtrlQiZiBehaviour : MonoBehaviour {

    public float m_moveTime = 3F;

	// Use this for initialization
	void Start () {
	
	}

    public void moveTo (GameObject go) {
        iTween.MoveTo(go, iTween.Hash("position", Vector3.zero, "time", m_moveTime, "looptype", iTween.LoopType.none, "easeType", iTween.EaseType.easeInBack));
    }

}
