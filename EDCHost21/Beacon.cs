using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    public class Beacon
    {
        //CarA放置的信标
        public Dot[] CarABeacon;
        //CarB放置的信标
        public Dot[] CarBBeacon;
        //一辆车最大允许放置的信标
        public int MaxBeaconNum;
        //CarA放置的信标数量
        public int CarABeaconNum;
        //CarB放置的信标数量
        public int CarBBeaconNum;
        //构造函数
        public Beacon()
        {
            //一辆车放置的信标数量最大为3
            MaxBeaconNum = 3;
            CarABeacon = new Dot[MaxBeaconNum];
            CarBBeacon = new Dot[MaxBeaconNum];
            CarABeaconNum = 0;
            CarBBeaconNum = 0;
        }

        //检查某个点是否与一系列信标重合
        static public bool Cover(Dot dot, Dot[] beacon_loc)
        {
            bool flag = true;
            foreach (Dot beacon_dot in beacon_loc)
            {
                if (Math.Pow(dot.x - beacon_dot.x, 2) + Math.Pow(dot.y - beacon_dot.y, 2) <= Math.Pow(Court.COINCIDE_ERR_DIST_CM, 2))
                {
                    flag = false;
                }
            }
            return flag;
        }
        //重设信标
        public void Reset()
        {
            CarABeaconNum = 0;
            CarBBeaconNum = 0;
        }
        //CarA放置信标
        public void CarAAddBeacon(Dot Pos)
        {
            //放置的信标不多于MaxBeaconNum
            if (CarABeaconNum < MaxBeaconNum)
            {
                CarABeacon[CarABeaconNum] = Pos;
                CarABeaconNum++;
            }
        }
        //CarB放置信标
        public void CarBAddBeacon(Dot Pos)
        {
            if (CarBBeaconNum < MaxBeaconNum)
            {
                CarBBeacon[CarBBeaconNum] = Pos;
                CarBBeaconNum++;
            }
        }
        //得到CarA同六个信标的距离，返回6个数字，前三个为自己放置的信标，后三个为另一组放置的信标，
        //如果某一方未放置够3个信标，未放置的信标所返回的距离为-1
        public double[] GetCarADistance(Dot Pos)
        {
            double[] Distance = new double[MaxBeaconNum * 2];
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarABeaconNum)
                {
                    Distance[i] = Dot.GetDistance(Pos, CarABeacon[i]);
                }
                else
                {
                    Distance[i] = -1;
                }
            }
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarBBeaconNum)
                {
                    Distance[i + MaxBeaconNum] = Dot.GetDistance(Pos, CarBBeacon[i]);
                }
                else
                {
                    Distance[i + MaxBeaconNum] = -1;
                }
            }
            return Distance;
        }
        //得到CarB同六个信标的距离，返回6个数字，前三个为自己放置的信标，后三个为另一组放置的信标，
        //如果某一方未放置够3个信标，未放置的信标所返回的距离为-1
        public double[] GetCarBDistance(Dot Pos)
        {
            double[] Distance = new double[MaxBeaconNum * 2];
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarBBeaconNum)
                {
                    Distance[i] = Dot.GetDistance(Pos, CarBBeacon[i]);
                }
                else
                {
                    Distance[i] = -1;
                }
            }
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarABeaconNum)
                {
                    Distance[i + MaxBeaconNum] = Dot.GetDistance(Pos, CarABeacon[i]);
                }
                else
                {
                    Distance[i + MaxBeaconNum] = -1;
                }
            }
            return Distance;
        }
    }
}
