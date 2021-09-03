using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    class Mine      // 此类第一回合、第二回合通用，两个回合分别使用不同的generator生成金矿
    {
        public Dot Start_Dot;       // 金矿初始位置
        public Dot End_Dot;         // 需要运输到的点（停车区域的中心点）

        //构造函数
        public Mine(Dot start_dt, Dot end_dt)
        {
            Start_Dot = start_dt;
            End_Dot = end_dt;
        }

        //用于修改点位的接口
        public void ResetDot(Dot start_dt, Dot end_dt)
        {
            Start_Dot = start_dt;
            End_Dot = end_dt;
        }

    }
}
