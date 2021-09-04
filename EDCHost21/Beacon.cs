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
            if (CarBBeaconNum < 3)
            {
                CarBBeacon[CarBBeaconNum] = Pos;
                CarBBeaconNum++;
            }
        }
        //得到CarA同六个信标的距离，返回6个数字，前三个为自己放置的信标，后三个为另一组放置的信标，
        //如果某一方未放置够3个信标，未放置的信标所返回的距离为-1
        public double[] GetCarADistance(int Posx, int Posy)
        {
            double[] Distance = new double[MaxBeaconNum * 2];
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarABeaconNum)
                {
                    Distance[i] = Math.Sqrt((Posx - CarABeacon[i].x) * (Posx - CarABeacon[i].x) + (Posy - CarABeacon[i].y) * (Posy - CarABeacon[i].y));
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
                    Distance[i + MaxBeaconNum] = Math.Sqrt((Posx - CarBBeacon[i].x) * (Posx - CarBBeacon[i].x) + (Posy - CarBBeacon[i].y) * (Posy - CarBBeacon[i].y));
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
        public double[] GetCarBDistance(int Posx, int Posy)
        {
            double[] Distance = new double[MaxBeaconNum * 2];
            for (int i = 0; i < MaxBeaconNum; i++)
            {
                if (i < CarBBeaconNum)
                {
                    Distance[i] = Math.Sqrt((Posx - CarBBeacon[i].x) * (Posx - CarBBeacon[i].x) + (Posy - CarBBeacon[i].y) * (Posy - CarBBeacon[i].y));
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
                    Distance[i + MaxBeaconNum] = Math.Sqrt((Posx - CarABeacon[i].x) * (Posx - CarABeacon[i].x) + (Posy - CarABeacon[i].y) * (Posy - CarABeacon[i].y));
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
