using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 负责棋盘坐标球的生成，棋盘棋子的交互以及对棋子移动的监视
/// </summary>
public class IndexCtrl : MonoBehaviour {

    public Transform m_parent;
    public GameObject m_indexSphere;
    public float m_offsetX = -1.15f;
    public float m_offsetY = 1f;

    /// <summary>
    /// 棋盘上所有棋子
    ///  256(16 * 16)的数组，小于16是红方棋子，大于16为黑方棋子，0表示没棋子。
    ///  用长度为256 的一维数组来表示棋盘的，
    ///  这样是为了方便计算机进行位操作，
    ///  求横坐标只用和1111做与运算，纵坐标右移4 位即可，效率很高。
    /// </summary>
    public readonly static byte[ ] m_qiZiPos = new byte[256]{
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0, 20, 19, 18, 17, 16, 17, 18, 19, 20,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0, 21,  0,  0,  0,  0,  0, 21,  0,  0,  0,  0,  0,
	      0,  0,  0, 22,  0, 22,  0, 22,  0, 22,  0, 22,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0, 14,  0, 14,  0, 14,  0, 14,  0, 14,  0,  0,  0,  0,
	      0,  0,  0,  0, 13,  0,  0,  0,  0,  0, 13,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0, 12, 11, 10,  9,  8,  9, 10, 11, 12,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	      0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0
	    };

    /// <summary>
    /// 所有的下标检测球
    /// </summary>
    List<GameObject> m_indexSpheres = new List<GameObject>( );
    /// <summary>
    /// 选中的棋子在哪个格子
    /// </summary>
    public static byte s_xzgz;


    void Start ( ) {
        // 复制 10 * 9 个坐标球
        initIndexSphere( );
    }

    /// <summary>
    /// 复制 10 * 9 个坐标球
    /// </summary> 
    void initIndexSphere ( ) {
        Vector3 pos = m_indexSphere.transform.localPosition;
        // 在256二维数组的下标
        byte row = 3;

        for ( byte i = 0; i < 10; i++ ) {
            byte column = 3;
            for ( byte j = 0; j < 9; j++ ) {
                GameObject go = Instantiate(m_indexSphere, pos, Quaternion.identity) as GameObject;
                go.transform.parent = m_parent;

                IndexTriger indexTriger = go.GetComponent<IndexTriger>( );
                // 设置下标
                indexTriger.m_i = i;
                indexTriger.m_j = j;
                // 设置在256数组中的下标
                indexTriger.m_row = row;
                indexTriger.m_column = column;

                go.name = "index" + "【" + i + ", " + j + "】，其在256的下标：【" + row + ", " + column + "】";
                go.SetActive(true);
                // 加入到下标检测球列表中去
                m_indexSpheres.Add(go);
                pos.x += m_offsetX;
                column++;
            }
            pos.x = m_indexSphere.transform.localPosition.x;
            pos.y -= m_offsetY;
            row++;
        }
    }

    /// <summary>
    /// 棋盘表示256数组的下标转成10-9数组的下标
    /// </summary>
    /// <param name="index256"></param>
    /// <returns></returns>
    int index256ToIndex10_9 (int index256) {
        return 0;
    }

}