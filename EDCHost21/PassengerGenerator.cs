using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EDCHOST22
{
    //说明（xhl）：1，目前的规则下，生成passenger应该是读取地图即可生成。同时还要保证上下半场是不变的。
    //2，最终Game需要读取的是PkgList（其中应该包含24个Passenger），所以Game啥的可以先写起来。（外界要用的是GetPkgDot）
    //3，具体如何生成，可以等地图写出来再说。
    //这个类还需要大改动！
    public class PassengerGenerator //存储预备要用的物资信息
    {
        private Dot[] PassengerstartDotArray;
        private Dot[] PassengerfinalDotArray;
        private int Passengernum;
        private int Passenger_idx;
        public PassengerGenerator(int amount) //生成指定数量的人员
        {
            Passenger_idx = 0;
            Passengernum = amount;
            PassengerstartDotArray = new Dot[Passengernum];
            PassengerfinalDotArray = new Dot[Passengernum];
            int nextX1, nextY1;
            int nextX2, nextY2;
            Dot dot1;
            Dot dot2;
            int same;
            Random NRand = new Random();
            for (int i = 0; i < Passengernum; ++i)
            {
                do
                {
                    same = 1;
                    nextX1 = NRand.Next(Game.MAZE_CROSS_NUM);//仍然需要改进Game.MazeCrossNum是上界
                    nextY1 = NRand.Next(Game.MAZE_CROSS_NUM);
                    dot1 = CrossNo2Dot(nextX1, nextY1);
                    PassengerstartDotArray[i] = dot1;
                    nextX2 = NRand.Next(Game.MAZE_CROSS_NUM);//仍然需要改进Game.MazeCrossNum是上界
                    nextY2 = NRand.Next(Game.MAZE_CROSS_NUM);
                    dot2 = CrossNo2Dot(nextX2, nextY2);
                    PassengerfinalDotArray[i] = dot2;
                    if (nextX1 == nextX2 && nextY1 == nextY2)
                    {
                        same = 0;
                    }
                    //需要加上人员与障碍是否重合的判断

                } while (same == 0);
            }

        }
        //返回下一个人员的坐标
        public Passenger Next()
        {
            Dot startpos = PassengerstartDotArray[Passenger_idx];
            Dot finalpos = PassengerfinalDotArray[Passenger_idx];
            Passenger temp = new Passenger(startpos, finalpos);   //xhl不确定C#语法是不是这样的
            Passenger_idx = Passenger_idx + 1;
            return temp;
        }
        public void ResetIndex() { Passenger_idx = 0; } //package_idx复位
        //从格点转化为int，传入坐标，返回Dot
        public static Dot CrossNo2Dot(int CrossNoX, int CrossNoY)
        {
            int x = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoX;
            int y = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoY;
            Dot temp = new Dot(x, y);
            return temp;
        }
    }
}