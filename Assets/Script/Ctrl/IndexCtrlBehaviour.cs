using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 负责棋盘坐标球的生成，棋盘棋子的交互以及对棋子移动的监视
/// </summary>
public class IndexCtrlBehaviour : MonoSingleton<IndexCtrlBehaviour> {

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
    /// 选中的棋子在哪个格子
    /// </summary>
    public static byte s_xzgz;

    /// <summary>
    /// 【棋盘表示256一维数组下标——下标检测球GameObject】，所有的下标检测球GameObject都加入其中
    /// </summary>
    private Dictionary<int, GameObject> m_hashIndex256QiZiPos = new Dictionary<int, GameObject>( );

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

                IndexTrigerBehaviour indexTriger = go.GetComponent<IndexTrigerBehaviour>( );
                // 设置下标
                indexTriger.m_i = i;
                indexTriger.m_j = j;
                // 设置在256（16*16）2维数组中的下标
                indexTriger.m_row = row;
                indexTriger.m_column = column;
                // 设置在256  1维数组中的下标
                int index256 = indexTriger.toIndex256( );

                go.name = string.Format("index【{0}, {1}】，其在256的下标：【{2}】，256的二维下标【{3}, {4}】", i, j, index256, indexTriger.m_row, indexTriger.m_column);
                // 将该下标检测球GameObject加到HASH中去，方便AI移动棋子等操作快速找到位置。
                m_hashIndex256QiZiPos.Add(index256, go);

                go.SetActive(true);               
                pos.x += m_offsetX;
                column++;
            }
            pos.x = m_indexSphere.transform.localPosition.x;
            pos.y -= m_offsetY;
            row++;
        }
    }

    public GameObject getIndexSphereGo (int index256) {
        if (m_hashIndex256QiZiPos.ContainsKey(index256)) {
            return m_hashIndex256QiZiPos[index256];
        }
        return null;
    }

    //==============棋盘表示数组下标转换==============

    #region 10-9转256

    /// <summary>
    /// 棋盘表示10-9数组下标，转成256（16*16二维数组表示）数组下标，返回256（16*16二维数组表示）数组的第几行坐标
    /// </summary>
    /// <param name="row">棋盘表示10-9数组第几行下标</param>
    /// <param name="col">棋盘表示10-9数组第几列下标</param>
    /// <returns>返回256（16*16二维数组表示）数组的第几行坐标</returns>
    public static int index10_9ToIndex256_RowIndex (int row,int col) {
        int index256RowIndex = (16 * (3 + row) + 3 + col) / 16;
        return index256RowIndex;
    }

    /// <summary>
    /// 棋盘表示10-9数组下标，转成256（16*16二维数组表示）数组下标，返回256（16*16二维数组表示）数组的第几列坐标
    /// </summary>
    /// <param name="row">棋盘表示10-9数组第几行下标</param>
    /// <param name="col">棋盘表示10-9数组第几列下标</param>
    /// <returns>返回256（16*16二维数组表示）数组的第几列坐标</returns>
    public static int index10_9ToIndex256_ColIndex (int row, int col) {
        int index256ColIndex = (16 * (3 + row) + 3 + col) % 8;
        return index256ColIndex;
    }

    #endregion 10-9转256

    //==============棋盘表示数组下标转换==============

}