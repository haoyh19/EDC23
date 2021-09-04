using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    class Court     // 此类提供实际比赛场地中需要使用的尺寸和其他参数
    {
        public const int MAX_SIZE_CM = 270;     // 场地的最大尺寸
        public const int MAZE_SIZE_CM = 210;    // 场地中心区域的尺寸
        public const int BORDER_CM = 30;   // 周边道路的宽度
        public const int HALF_BORDER_CM = 15;   // 周边道路中心线与道路边缘的距离
        public const int DISTANCE_PARKIN_AREA = 120;    // 停车点中心点间距
        public const int COINCIDE_ERR_DIST_CM = 10;  // 判定小车到达某点允许的最大误差距离（碰撞半径）
        public const int MINE_LOWERDIST_CM = 20;        // 金矿间的最小距离
        public const int TOTAL_PARKING_AREA = 8;     // 停车点的总个数
        /* 停车点编号方式：
         *      0 1 2
         *      7   3
         *      6 5 4
         */

        static public Dot ParkID2Dot(int pid)       // 将停车点编号转换为停车点中心坐标
        {
            if (pid < 0 || pid >= TOTAL_PARKING_AREA)
            {
                return new Dot(-10, -10);
            }
            else if (pid <= 2)
            {
                return new Dot(pid * DISTANCE_PARKIN_AREA + HALF_BORDER_CM, HALF_BORDER_CM);
            }
            else if (pid <= 4)
            {
                return new Dot(MAX_SIZE_CM - HALF_BORDER_CM, (pid - 2) * DISTANCE_PARKIN_AREA + HALF_BORDER_CM);
            }
            else if (pid <= 6)
            {
                return new Dot((6 - pid) * DISTANCE_PARKIN_AREA + HALF_BORDER_CM, MAX_SIZE_CM - HALF_BORDER_CM);
            }
            else
            {
                return new Dot(HALF_BORDER_CM, HALF_BORDER_CM + DISTANCE_PARKIN_AREA);
            }
        }
    }
}
