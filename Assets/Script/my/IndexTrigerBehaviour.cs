using UnityEngine;
using System.Collections;

namespace SocialPoint.Examples.MVC {
    /// <summary>
    /// 挂在下标检查球上的脚本，判断棋盘上的某个点是否有棋子
    /// </summary>
    public class IndexTrigerBehaviour : MonoBehaviour {

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


        private PlayerDragViewPresenter m_dragQiZi;
        /// <summary>
        /// 该检查球这有棋子
        /// </summary>
        private bool m_hasQiZi;
        public bool HasQiZi { get { return m_hasQiZi; } }
        /// <summary>
        /// 该检查球上的棋子
        /// </summary>
        private GameObject m_qiZiGameObject;
        public GameObject QiZiGameObject { get { return m_qiZiGameObject; } }


        private GameObject m_selfGameObject;

        void Awake ( ) {
            m_hasQiZi = false;
            m_selfGameObject = this.gameObject;
        }

        const string TAG_PLAYER = "Player";
        void OnTriggerEnter (Collider other) {
            if (other.CompareTag(TAG_PLAYER)) {
                m_hasQiZi = true;
                m_qiZiGameObject = other.gameObject;
                //Debuger.Log("棋子" + other.name + "落在坐标检测球" + this.name + "上!");

                if (null != other.GetComponent<PlayerDragViewPresenter>( )) {
                    m_dragQiZi = other.GetComponent<PlayerDragViewPresenter>( );
                    m_dragQiZi.m_IndexTriger = this;
                    Debuger.Log(string.Format("m_dragQiZi棋子GameObject={0}", other.name));
                } else {
                    Debuger.LogError(string.Format("{0}棋子GameObject上没有DragQiZi脚本", other.name));
                }
            }
        }

        void OnTriggerExit (Collider other) {
            m_hasQiZi = false;
            m_qiZiGameObject = null;

            if (m_dragQiZi != null) {
                //m_dragQiZi.m_IndexTriger = null;
                m_dragQiZi = null;
            }
        }

        /// <summary>
        /// 16*16的2维数组下标转换为256的1维数组中的下标
        /// </summary>
        /// <returns></returns>
        public int toIndex256 ( ) {
            // 二维数组 a[INDEX1][INDEX2] 转换成一维数组b[INDEX1*INDEX2]，则a[i][j] 对应一维数组的索引 b[INDEX2*i+j]
            // 只与列数有关，与行数无关。
            int index256 = 16 * m_row + m_column;
            return index256;
        }

    }
}
