using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    class Beacon    // 信标
    {
        public const int TOTAL = 6; // 每场比赛最多设置6个信标

        public int num;     // 已设置信标的个数
        public Dot[] dots;  // 设置信标的位置信息，0≤小标≤num-1的数据有效
        public void ResetBeacon()   // 重置信标个数为0
        {
            num = 0;
        }
        public Beacon(int Num = 0)      // 构造函数
        {
            num = Num;
            dots = new Dot[TOTAL];
            Dot temp = new Dot();
            for (int i = 0; i < TOTAL; i++)
            {
                dots[i] = temp;
            }
        }
    }
}
