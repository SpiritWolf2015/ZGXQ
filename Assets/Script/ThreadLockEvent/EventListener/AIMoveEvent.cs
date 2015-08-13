using UnityEngine;
using System.Collections;

namespace com.gzc.ThreadLockEvent {

    /// <summary>
    /// AI下一步棋，移动棋子事件
    /// </summary>
    public class AIMoveEvent : EventBase {

        // 事件名
        public const string AI_MOVE_EVNET = "ai_move_event";
        // 256数组下标，电脑下棋，棋子从哪走到哪
        public int from;
        public int to;

        // 这个构造函数不能省
        public AIMoveEvent ( ) { }
    }
}