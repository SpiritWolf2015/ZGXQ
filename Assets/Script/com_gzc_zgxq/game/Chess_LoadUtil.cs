
namespace com.gzc.zgxq.game {

    public class Chess_LoadUtil {

        /// <summary>
        /// 走法是否符合帅(将)的步长
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static bool KING_SPAN (int sqSrc, int sqDst) {
            return GameConstant.ccLegalSpan[sqDst - sqSrc + 256] == 1;
        }

        /// <summary>
        /// 走法是否符合仕(士)的步长
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static bool ADVISOR_SPAN (int sqSrc, int sqDst) {
            return GameConstant.ccLegalSpan[sqDst - sqSrc + 256] == 2;
        }

        /// <summary>
        /// 走法是否符合相(象)的步长
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns> 
        public static bool BISHOP_SPAN (int sqSrc, int sqDst) {
            return GameConstant.ccLegalSpan[sqDst - sqSrc + 256] == 3;
        }

        /// <summary>
        /// 是否在同一列
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static bool SAME_FILE (int sqSrc, int sqDst) {
            return ((sqSrc ^ sqDst) & 0x0f) == 0;
        }

        /// <summary>
        /// 是否在同一行
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static bool SAME_RANK (int sqSrc, int sqDst) {
            return ((sqSrc ^ sqDst) & 0xf0) == 0;
        }

        /// <summary>
        /// 马腿的位置
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static int KNIGHT_PIN (int sqSrc, int sqDst) {
            return sqSrc + GameConstant.ccKnightPin[sqDst - sqSrc + 256];
        }

        /// <summary>
        /// 相(象)眼的位置
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static int BISHOP_PIN (int sqSrc, int sqDst) {
            return (sqSrc + sqDst) >> 1;
        }

        /// <summary>
        /// 是否在河的同一边
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static bool SAME_HALF (int sqSrc, int sqDst) {
            return ((sqSrc ^ sqDst) & 0x80) == 0;
        }

        /// <summary>
        /// 获得走法的起点
        /// </summary>
        /// <param name="mv"></param>
        /// <returns></returns>
        public static int SRC ( int mv ) {
            return mv & 255;
        }

        /// <summary>
        /// 格子水平镜像
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static int SQUARE_FORWARD ( int sq, int sd ) {
            return sq - 16 + (sd << 5);
        }

        /// <summary>
        /// 是否已过河
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool AWAY_HALF ( int sq, int sd ) {
            return !HOME_HALF(sq, sd);
        }

        /// <summary>
        /// 是否未过河
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static bool HOME_HALF ( int sq, int sd ) {
            return (sq & 0x80) != (sd << 7);
        }

        /// <summary>
        /// 判断棋子是否在棋盘中
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static bool IN_BOARD ( int sq ) {
            return GameConstant.ccInBoard[sq] != 0;
        }

        /// <summary>
        /// 根据起点和终点获得走法
        /// </summary>
        /// <param name="sqSrc"></param>
        /// <param name="sqDst"></param>
        /// <returns></returns>
        public static int MOVE ( int sqSrc, int sqDst ) {
            return sqSrc + sqDst * 256;
        }

        /// <summary>
        /// 判断棋子是否在九宫中
        /// </summary>
        /// <param name="sq"></param>
        /// <returns></returns>
        public static bool IN_FORT ( int sq ) {
            return GameConstant.ccInFort[sq] != 0;
        }

        /// <summary>
        /// 翻转格子
        /// </summary>
        /// <param name="sq">位置下标</param>
        /// <returns></returns>
        public static int SQUARE_FLIP ( int sq ) {
            return 254 - sq;
        }

        /// <summary>
        /// 获得走法的终点
        /// </summary>
        /// <param name="move">走一步棋的走法</param>
        /// <returns>该步走法落子后，棋子位置在256数组中的下标</returns>
        public static int DST ( int move ) {
            return move >> 8;
        }

        /// <summary>
        /// 获得红黑标记(红子是8，黑子是16)
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static int SIDE_TAG ( int sd ) {
            return 8 + (sd << 3);
        }

        /// <summary>
        /// 获得对方红黑标记
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public static int OPP_SIDE_TAG ( int sd ) {
            return 16 - (sd << 3);
        }

    }

}