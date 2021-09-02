using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    //0813xhl把struct改成了class
    public class Dot //点
    {
        public int x;
        public int y;
        //构造函数
        //8-14 yd添加了默认构造值
        public Dot(int _x = 0, int _y = 0) { x = _x; y = _y; }

        //运算符重载
        public static bool operator ==(Dot a, Dot b)
        {
            return (a.x == b.x) && (a.y == b.y);
        }

        public static bool operator !=(Dot a, Dot b)
        {
            return !(a == b);
        }

    }
}
