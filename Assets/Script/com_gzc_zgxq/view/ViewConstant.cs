using UnityEngine;
using System.Collections;

namespace com.gzc.zgxq.view {

    /// <summary>
    ///  View常量，工具方法
    /// </summary>
    public static class ViewConstant {

        /// <summary>
        /// 是否为开始或者暂停
        /// </summary>
        public static bool isnoStart = false;

        public const string START = "开始";
        public const string PAUSE = "暂停";

        /// <summary>
        /// 难度系数
        /// </summary>
        public static byte nanduXS = 1;
        /// <summary>
        /// 单位：毫秒，即15分钟
        /// </summary>
        public static int zTime = 900 * 1000; 
        /// <summary>
        /// 总时间
        /// </summary>
        public static int endTime = zTime;
        /// <summary>
        /// 赢界面标志
        /// </summary>
        public static bool yingJMflag;
        /// <summary>
        /// 输界面标志
        /// </summary>
        public static bool shuJMflag;
        /// <summary>
        /// 电脑下棋思考时长(分钟)
        /// </summary>
        public static float thinkDeeplyTime = 0.00015F;
    }

}
