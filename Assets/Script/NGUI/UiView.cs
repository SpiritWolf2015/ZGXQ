using UnityEngine;
using com.gzc.zgxq.view;

public class UiView : MonoBehaviour {

    public UILabel m_time;
    public UILabel m_buttonStart;
	
	void Start () {
        //m_time.text = ViewConstant.endTime.ToString( );
	}
    void Update ( ) {
        m_time.text = ViewConstant.endTime.ToString( );
    }

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



}
