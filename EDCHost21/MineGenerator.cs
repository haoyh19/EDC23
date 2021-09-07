using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    public class MineGenerator    //进行各回合金矿的生成
    {
        public const int COURTMINENUM = 2;       // 同时存在的金矿数
        public const int MINELISTNUM = 30;      //第二回合可取用的金矿总数

        public Mine[] MineArray1;        // 第一回合设置金矿的数组
        public int ParkPoint;            // 第一回合停车点
        public Mine[] MineArray2;        // 第二回合金矿数组
        public int Mine_id;              // 第二回合该取下标为Mine_id的金矿了

        public MineGenerator()         // 构造函数
        {
            Mine_id = 0;

            MineArray1 = new Mine[COURTMINENUM];
            for (int i = 0; i < COURTMINENUM; i++)
            {
                MineArray1[i] = new Mine();
            }
            MineArray2 = new Mine[MINELISTNUM];
            for (int i = 0; i < MINELISTNUM; i++)
            {
                MineArray2[i] = new Mine();
            }
            Random ran = new Random();

            ParkPoint = ran.Next(0, 8);        //双参数Next函数不含上限

            //生成第一回合要用到的两个矿
            int stage1_mine1_x = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
            int stage1_mine1_y = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
            int stage1_mine1_d = ran.Next(Court.MAX_MINE_DEPTH);   //单参数Next含上界
            Dot stage1_mine1_xy = new Dot(stage1_mine1_x, stage1_mine1_y);
            Mine stage1_mine1 = new Mine(stage1_mine1_xy, stage1_mine1_d);

            int stage1_mine2_x = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
            int stage1_mine2_y = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
            Dot stage1_mine2_xy = new Dot(stage1_mine2_x, stage1_mine2_y);
            int stage1_mine2_d = ran.Next(Court.MAX_MINE_DEPTH);
            while (Dot.InCollisionZone(stage1_mine1_xy, stage1_mine2_xy, Court.MINE_LOWERDIST_CM))
            {
                stage1_mine2_x = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                stage1_mine2_y = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                stage1_mine2_xy = new Dot(stage1_mine2_x, stage1_mine2_y);
            }
            Mine stage1_mine2 = new Mine(stage1_mine2_xy, stage1_mine2_d);

            MineArray1[0] = stage1_mine1;
            MineArray1[1] = stage1_mine2;

        }

        //返回停车点
        public int GetParkPoint()
        {
            return ParkPoint;
        }

        //返回第一回合的金矿组
        public Mine[] GetStage1Mine()
        {
            return MineArray1;
        }

        //检查i号金矿与前四个大于金矿最小距离
        public bool MinesApart(Dot mine_xy, int i)
        {
            bool flag = true;
            for (int j = i >= 4 ? i - 4 : 0; j < i; j++)
            {
                if (Dot.InCollisionZone(mine_xy, MineArray2[j].Pos, Court.MINE_LOWERDIST_CM))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }


        //生成第二回合的金矿组
        public void GenerateStage2(Beacon beacon)
        {
            Random ran = new Random();
            Dot[] beacon_loc = new Dot[beacon.CarABeaconNum + beacon.CarBBeaconNum];
            int count = 0;
            for (int i = 0; i < beacon.CarABeaconNum; i++)
            {
                beacon_loc[count++] = beacon.CarABeacon[i];
            }
            for (int i = 0; i < beacon.CarBBeaconNum; i++)
            {
                beacon_loc[count++] = beacon.CarBBeacon[i];
            }

            for (int i = 0; i < MINELISTNUM; i++)
            {
                int stage2_mine_x = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                int stage2_mine_y = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                int stage2_mine_d = ran.Next(Court.MAX_MINE_DEPTH);
                Dot stage2_mine_xy = new Dot(stage2_mine_x, stage2_mine_y);
                while (!MinesApart(stage2_mine_xy, i) || Dot.InCollisionZones(stage2_mine_xy, beacon_loc))
                {
                    stage2_mine_x = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                    stage2_mine_y = ran.Next(Court.BORDER_CM, Court.MAX_SIZE_CM + 1 - Court.BORDER_CM);
                    stage2_mine_xy = new Dot(stage2_mine_x, stage2_mine_y);
                }
                Mine stage2_mine = new Mine(stage2_mine_xy, stage2_mine_d);
                MineArray2[i] = stage2_mine;
            }
        }

        //第二回合中，返回下一个列表中的金矿
        public Mine GetNextMine()
        {
            return MineArray2[Mine_id++];
        }

        //恢复mine_id为0供B车使用
        public void Reset()
        {
            Mine_id = 0;
        }
    }
}
