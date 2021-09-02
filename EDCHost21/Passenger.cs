using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    //救援的人员只有起点/终点两个数据Dot。可能需要添加是否到达终点（enum）的状态等。
    public class Passenger//等待救援的人员位置信息
    {
        public Dot Start_Dot;
        public Dot End_Dot;
        //构造函数
        public Passenger(Dot startDot, Dot finalDot)
        {
            Start_Dot = startDot;
            End_Dot = finalDot;
        }
        //public Passenger() : this(new Dot(0, 0), new Dot(0, 0)) { } //0809xhl修正了缺少的括号
        //预留一个修改的接口，目前没用上
        public void ResetInfo(Dot startDot, Dot finalDot)//重新生成位置
        {
            Start_Dot = startDot;
            End_Dot = finalDot;
        }
    }
}
