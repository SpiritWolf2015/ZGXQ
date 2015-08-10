using UnityEngine;
using System.Collections;

/// <summary>
/// 判断棋盘上的某个点是否有棋子
/// </summary>
public class IndexTriger : MonoBehaviour {

    /// <summary>
    /// 在10-9二维数的下标是多少
    /// </summary>
    public byte m_i, m_j;
    /// <summary>
    /// 在256二维数的下标是多少
    /// </summary>
    public byte m_row, m_column;


    DragQiZi m_dragQiZi;
    bool m_hasQiZi;

    void Start ( ) {        
        m_hasQiZi = false;
    }

    const string TAG_PLAYER = "Player";
    void OnTriggerEnter ( Collider other ) {
        if ( other.CompareTag(TAG_PLAYER) ) {            
            if ( null != other.GetComponent<DragQiZi>( ) ) {
                m_dragQiZi = other.GetComponent<DragQiZi>( );
                m_dragQiZi.m_IndexTriger = this;
                m_hasQiZi = true;
               
                Debuger.Log("棋子" + m_dragQiZi.name + "在" + this.name + "上!" );
            }         
        }       
    }

    void OnTriggerExit ( Collider other ) {       
        m_hasQiZi = false;
        m_dragQiZi = null;
    }

}
