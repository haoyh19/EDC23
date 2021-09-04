using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;


namespace EDCHOST22
{
    class MineGenerator2
    {
        public const int MINENUM = 2;       // 第二回合存在的金矿数
        public const string FILENAME_1 = "./MineInfo_2.1.txt";    // 一号金矿伪随机位置存储文件名
        public const string FILENAME_2 = "./MineInfo_2.2.txt";    // 二号金矿伪随机位置存储文件名
        public const int LINENUM = 20;       // 两文件各有几行

        /* MineInfo_2.txt文件存储格式：
         * 
         * 每行为一种地图
         * 一行有3个整数，空格分隔，分别表示：金矿X 金矿Y 金矿深度 
         */
        public int LineNow_1;             // 金矿1选择文件的第几行（从1开始，没有第0行）
        public int LineNow_2;             // 金矿2选择文件的第几行（从1开始，没有第0行）
        public Mine[] MineArray;        // 金矿的数组
        public string[] lines1;          //存储文件每一行的string数组
        public string[] lines2;

        public MineGenerator2()         // 构造函数
        {
            LineNow_1 = 0;
            LineNow_2 = 0;
            MineArray = new Mine[MINENUM];
            for (int i = 0; i < MINENUM; i++)
            {
                MineArray[i] = new Mine();
            }
            try
            {
                TextReader reader1 = File.OpenText(FILENAME_1);
                TextReader reader2 = File.OpenText(FILENAME_2);
                for (int i = 1; i <= LINENUM; i++)
                {
                    lines1[i] = reader1.ReadLine();
                    lines2[i] = reader2.ReadLine();
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("无效的金矿文件路径");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("不存在指定的金矿文件");
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("文件路径格式无效");
            }
        }


        //根据矿号更新该金矿位置
        public void Refresh(int mine_id, Dot[] beacon_loc)
        {
            if (mine_id == 1)
            {
                LineNow_1++;
                string[] bits = lines1[LineNow_1].Split(' ');
                int x = int.Parse(bits[0]);
                int y = int.Parse(bits[1]);
                int d = int.Parse(bits[2]);
                Dot temp_dot = new Dot(x, y);
                while (Beacon.Cover(temp_dot, beacon_loc)) //检查生成点是否与信标重合，重合则换下一点。到末行则切回第一行
                {
                    if (LineNow_1 == LINENUM)
                    {
                        LineNow_1 = 1;
                    }
                    else
                    {
                        LineNow_1++;
                    }
                    bits = lines1[LineNow_1].Split(' ');
                    x = int.Parse(bits[0]);
                    y = int.Parse(bits[1]);
                    d = int.Parse(bits[2]);
                    temp_dot = new Dot(x, y);
                }
                MineArray[0].ResetInfo(temp_dot, d);
                Debug.WriteLine("Mine1 Refreshed");
            }
            else if (mine_id == 2)
            {
                LineNow_2++;
                string[] bits = lines2[LineNow_2].Split(' ');
                int x = int.Parse(bits[0]);
                int y = int.Parse(bits[1]);
                int d = int.Parse(bits[2]);
                Dot temp_dot = new Dot(x, y);
                while (Beacon.Cover(temp_dot, beacon_loc)) //检查生成点是否与信标重合，重合则换下一点。到末行则切回第一行
                {
                    if (LineNow_2 == LINENUM)
                    {
                        LineNow_2 = 1;
                    }
                    else
                    {
                        LineNow_2++;
                    }
                    bits = lines2[LineNow_2].Split(' ');
                    x = int.Parse(bits[0]);
                    y = int.Parse(bits[1]);
                    d = int.Parse(bits[2]);
                    temp_dot = new Dot(x, y);
                }
                MineArray[1].ResetInfo(temp_dot, d);
                Debug.WriteLine("Mine2 Refreshed.");
            }
            else
            {
                Debug.WriteLine("Wrong MineID");
                return;
            }
        }

        //Game中直接调用Generate函数，将得到初始的两个矿
        public void Generate(Dot[] beacon_loc)
        {
            Refresh(1, beacon_loc);
            Refresh(2, beacon_loc);
        }
    }
}
