using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    class Mine      // 此类第一回合、第二回合通用，两个回合中用同一个MineGenerator生成矿
    {
        public const double A = 100;    // 金矿强度值计算参数
        public Dot Pos;       // 金矿初始位置
        public Dot FinalDot;         // 需要运输到的点 // hyh认为不需要，只有第一回合随机生成一个停车点，写到MineGenerator里即可
                                                       //ytz认为需要，需要放到ResetInfo中当参数
        public int Depth;           // 金矿初始深度

        //构造函数（含参）
        public Mine(Dot start_dt, int depth_)
        {
            Pos = start_dt;
            Depth = depth_;
            FinalDot = new Dot(0, 0);    //by ytz 需要运输的点初始设为0,0
        }

        // 构造函数（无参）
        public Mine()
        {
            Dot temp = new Dot(0, 0);
            Pos = temp;
            Depth = 0;
            FinalDot = new Dot(0, 0);
        }

        //用于修改点位的接口  //by ytz:传终点的位置，进来构造终点，把终点打散为两个坐标是为了写参数默认值
        public void ResetInfo(Dot start_dt, int depth_, int final_dt_x = 0, int final_dt_y = 0)
        {
            Pos = start_dt;
            Depth = depth_;
            FinalDot = new Dot(final_dt_x, final_dt_y);
        }
        
        // 获取某金矿对任意点处的强度
        static public double GetIntensity(Mine m, Dot d)
        {
            return A/(Math.Pow(m.Depth, 2) + Math.Pow(m.Pos.x - d.x, 2) + Math.Pow(m.Pos.y - d.y, 2));
        }
    }
}
