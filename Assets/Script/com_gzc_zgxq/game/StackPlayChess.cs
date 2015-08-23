
namespace com.gzc.zgxq.game {

    /// <summary>
    /// 记录走法的类。
    /// 将对弈时，走法和在该走法下目标位置吃掉的棋子记录起来，放入栈，
    /// 在悔棋时取出来，撤销该走法即可。
    /// </summary>
    public class StackPlayChess {

        /// <summary>
        /// 得到目的格子的棋子
        /// </summary>
        public int pcCaptured { get; private set; }
        /// <summary>
        /// 电脑走的一步棋
        /// </summary>
        public int mvResult { get; private set; }

        public StackPlayChess ( int mvResult, int pcCaptured ) {
            this.pcCaptured = pcCaptured;
            this.mvResult = mvResult;
        }

        public override string ToString ( ) {
            return string.Format("得到目的格子的棋子pcCaptured = {0}, 电脑走的一步棋mvResult = {1}", pcCaptured, mvResult);
        }
    }

}