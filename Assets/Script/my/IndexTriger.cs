using UnityEngine;
using System.Collections;

/// <summary>
/// 判断棋盘上的某个点是否有棋子
/// </summary>
public class IndexTriger : MonoBehaviour {

    /// <summary>
    /// 在10-9二维数的下标是第几行
    /// </summary>
    public byte m_i;
    /// <summary>
    /// 在10-9二维数的下标是第几列
    /// </summary>
    public byte m_j;

    /// <summary>
    /// 在256（16*16）二维数的下标是第几行
    /// </summary>
    public byte m_row;
    /// <summary>
    /// 在256（16*16）二维数的下标是第几列
    /// </summary>
    public byte m_column;

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

    public int toIndex256 ( ) {         
        // 二维数组 a[INDEX1][INDEX2] 转换成一维数组b[INDEX1*INDEX2]，则a[i][j] 对应一维数组的索引 b[INDEX2*i+j]
        // 只与列数有关，与行数无关。
        int index256 = 16 * m_row + m_column;
        return index256;
    }

}
