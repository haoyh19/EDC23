using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    class Mine      // 此类第一回合、第二回合通用，两个回合分别使用不同的generator生成金矿
    {
        public const double A = 100;    // 金矿强度值计算参数
        public Dot StartDot;       // 金矿初始位置
        public Dot FinalDot;         // 需要运输到的点（停车区域的中心点）
        public int StartDepth;           // 金矿初始深度

        //构造函数（含参）
        public Mine(Dot start_dt, Dot end_dt, int depth_)
        {
            StartDot = start_dt;
            FinalDot = end_dt;
            StartDepth = depth_;
        }

        // 构造函数（无参）
        public Mine()
        {
            Dot temp = new Dot(0, 0);
            StartDot = temp;
            FinalDot = temp;
            StartDepth = 0;
        }

        //用于修改点位的接口
        public void ResetInfo(Dot start_dt, Dot end_dt, int depth_)
        {
            StartDot = start_dt;
            FinalDot = end_dt;
            StartDepth = depth_;
        }
        
        // 获取某金矿对任意点处的强度
        static public double GetIntensity(Mine m, Dot d)
        {
            return A/(Math.Pow(m.StartDepth, 2) + Math.Pow(m.StartDot.x - d.x, 2) + Math.Pow(m.StartDot.y - d.y, 2));
        }
    }
}
