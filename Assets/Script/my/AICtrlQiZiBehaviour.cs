using UnityEngine;
using System.Collections;
using com.gzc.ThreadLockEvent;

/// <summary>
/// AI移动棋子
/// </summary>
public class AICtrlQiZiBehaviour : MonoBehaviour {

    public float m_moveTime = 3F;
	
	void Start () {
        //监听事件       
        EventDispatcher.Instance( ).RegistEventListener(AIMoveEvent.AI_MOVE_EVNET, EventCallback);
        Debug.Log("监听事件:" + AIMoveEvent.AI_MOVE_EVNET);
	}

    void EventCallback (EventBase eb) {        
        AIMoveEvent aIMoveEvent = eb.eventValue as AIMoveEvent;
        Debug.Log(string.Format("事件回调:{0}, from={1}, to={2}", AIMoveEvent.AI_MOVE_EVNET, aIMoveEvent.from, aIMoveEvent.to));

        if (aIMoveEvent != null) {
            // 得到下标检测球GameObject
            GameObject indexSphereGo = IndexCtrl.instance.getIndexSphereGo(aIMoveEvent.from);
            if (indexSphereGo == null) {
                Debuger.LogError(string.Format("该256下标{0} indexSphereGo is null", aIMoveEvent.from));
                return;
            }

            // 得到下标检测球上的IndexTriger脚本
            IndexTriger indexTriger = indexSphereGo.GetComponent<IndexTriger>( );
            if (indexTriger != null) {                
                // 如果该检测球上有棋子，则移动棋子到该次下棋的终点的检测球位置
                if (indexTriger.HasQiZi) {
                    Debug.Log(string.Format("AI下棋，将棋子从256数组下标{0}移到下标{1}", aIMoveEvent.from, aIMoveEvent.to));
                    this.moveTo(indexTriger.QiZiGameObject, IndexCtrl.instance.getIndexSphereGo(aIMoveEvent.to).GetComponent<Transform>( ).localPosition);
                }
            } else {
                Debug.LogError("indexTriger is null !!");
            }
        } else {
            Debug.LogError("aIMoveEvent is null !!");
        }
    }

    public void moveTo (GameObject go, Vector3 toPosition) {
        iTween.MoveTo(go, iTween.Hash("position", toPosition, "time", m_moveTime, "looptype", iTween.LoopType.none, "easeType", iTween.EaseType.easeInBack));
    }

}
