using UnityEngine;
using com.gzc.zgxq.view;

public class GameUiViewBehaviour : MonoBehaviour {

    public UILabel m_time;
    public UILabel m_buttonStart;

    #region U3D API

    void Start () {
        //m_time.text = ViewConstant.endTime.ToString( );
	}

    void Update ( ) {
        m_time.text = ViewConstant.endTime.ToString( );
    }

    #endregion U3D API

    #region NGUI事件消息处理
    
    public void buttonNewGame ( ) {        
        Debuger.Log("点击新局按钮");        
    }

    public void buttonStart ( ) {
        ViewConstant.isnoStart = !ViewConstant.isnoStart;
        Debuger.Log("点击开始暂停按钮，ViewConstant.isnoStart  = " + ViewConstant.isnoStart);
        // 如果开始了，则把按钮的字改成暂停，暂停了则改为开始
        if ( ViewConstant.isnoStart ) {
            m_buttonStart.text = ViewConstant.PAUSE;
        } else {
            m_buttonStart.text = ViewConstant.START;
        }
    }

    public void buttonUndo ( ) {
        Debuger.Log("点击悔棋按钮");
    }

    public void buttonSound ( ) {
        Debuger.Log("点击音效按钮");
    }

    #endregion NGUI事件消息处理

}
