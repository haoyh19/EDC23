using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    //说明（xhl）：1，目前的规则下，生成package应该是读取地图即可生成。同时还要保证上下半场是不变的。
    //2，最终Game需要读取的是PkgList（其中应该包含24个Package），所以Game啥的可以先写起来。（外界要用的是GetPkgDot）
    //3，具体如何生成，可以等地图写出来再说。
    public class PackageGenerator //存储预备要用的物资信息
    {
        private Package[] mpPackageList;
        private int PKG_NUM;
        public PackageGenerator(int AMOUNT) //生成指定数量的物资
        {
            PKG_NUM = AMOUNT;
            mpPackageList = new Package[PKG_NUM];
            int nextX, nextY;
            Dot dots;
            Random NRand = new Random();
            for (int i = 0; i < PKG_NUM; ++i)
            {
                nextX = NRand.Next(Game.MAZE_CROSS_NUM);
                nextY = NRand.Next(Game.MAZE_CROSS_NUM);
                dots = CrossNo2Dot(nextX, nextY);
                mpPackageList[i] = new Package(dots);
            }
        }

        //从格点转化为int，传入坐标，返回Dot
        public static Dot CrossNo2Dot(int CrossNoX, int CrossNoY)
        {
            int x = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoX;
            int y = Game.MAZE_SHORT_BORDER_CM + Game.MAZE_SIDE_BORDER_CM + Game.MAZE_CROSS_DIST_CM * CrossNoY;
            Dot temp = new Dot(x, y);
            return temp;
        }
        //返回下标为i的PackageDotArray中的点。开发者：xhl
        public Package GetPackage(int i)
        {
            return mpPackageList[i];
        }
    }
}
