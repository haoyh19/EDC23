using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    //Package,物资。
    //包括：位置Dot Pos，是否已经被获取bool
    public class Package
    {
        public Dot mPos; //物资生成地点
        public int IsPicked; //是否已经被获取.//cyy改成int了，因为bool转换不到byte型，0为没有拾取，1为已拾取
        public Package(Dot aPos)
        {
            mPos = aPos;
            IsPicked = 0;
        }
        public Dot GetDot()
        {
            return mPos;
        }
        public Package()
        {
            mPos = new Dot(0, 0);
            IsPicked = 0;
        }
    }
}
