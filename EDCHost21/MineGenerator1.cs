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
    class MineGenerator1    // 第一回合的金矿，伪随机生成（从文件读取）
    {
        public const int MINENUM = 2;       // 第一回合存在的金矿数
        public const string FILENAME = "./MineInfo_1.txt";    // 伪随机金矿位置存储文件名//ytz在这里改了一下文件名
        public const int LINENUM = 8;       // 文件共有几行，即有几种可选地图（金矿分布）

        /* MineInfo.txt文件存储格式：
         * 
         * 每行为一种地图
         * 一行有8个整数，空格分隔，分别表示：金矿1初始X 金矿1初始Y 金矿1初始深度 金矿1指定停车点编号 金矿2初始X 金矿2初始Y  金矿2初始深度 金矿2指定停车点编号
         * 停车点编号方式：
         *      0 1 2
         *      7   3
         *      6 5 4
         * 
         */
        public int LineNow;             // 选择文件的第几行，即读取第几种地图（金矿分布）（从1开始，没有第0行）
        public Mine[] MineArray;        // 第一回合设置金矿的数组
        public bool IsMineSet;          // 金矿是否已设置
        
        public MineGenerator1()         // 构造函数
        {
            LineNow = 0;
            IsMineSet = false;
            MineArray = new Mine[MINENUM]; 
            for (int i = 0; i < MINENUM; i++)
            {
                MineArray[i] = new Mine();
            }
        }

        public void ReadFromFile(int line)
        {
            try
            {
                IsMineSet = false;
                TextReader reader = File.OpenText(FILENAME);
                for (int i = 0; i < line - 1; i++)
                {
                    reader.ReadLine();
                }
                string text = reader.ReadLine();
                string[] bits = text.Split(' ');
                int x1 = int.Parse(bits[0]);
                int y1 = int.Parse(bits[1]);
                int d1 = int.Parse(bits[2]);
                int c1 = int.Parse(bits[3]);
                Dot p1 = Court.ParkID2Dot(c1);
                int x2 = int.Parse(bits[4]);
                int y2 = int.Parse(bits[5]);
                int d2 = int.Parse(bits[6]);
                int c2 = int.Parse(bits[7]);
                Dot p2 = Court.ParkID2Dot(c2);
                MineArray[0].ResetInfo(new Dot(x1, y1), d1, p1.x, p1.y);
                MineArray[1].ResetInfo(new Dot(x2, y2), d2, p2.x, p2.y);

                // 金矿成功设置
                IsMineSet = true;

                Debug.WriteLine("Mine Created from text.");
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
            LineNow = line;
        }
        
        //Game中直接调用Generate函数，将得到一个做好的MineGenerator对象，从中拿mine的数据即可。Game中，不同车第一回合中用同一个MineGenerator对象
        public void Generate()
        {
            Random ran = new Random();
            ReadFromFile(ran.Next(1, LINENUM));
        }
    }
}
