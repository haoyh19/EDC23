using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Diagnostics;

namespace EDCHOST22
{
    // 比赛状况：未开始、正常进行中、暂停、结束
    public enum GameState { UNSTART = 0, NORMAL = 1, PAUSE = 2, END = 3 };

    // 人员状况：被困、在小车上且还未到指定点、到达目标点
    public enum PassengerState { TRAPPED, INCAR, RESCUED };

    public enum GameStage { FIRST_1 = 0, FIRST_2, LATTER_1, LATTER_2 ,END};
    public class Game
    {
        public bool DebugMode;                       //调试模式，最大回合数 = 1,000,000
        public const int MAX_SIZE_CM = 254;          //地图大小
        public const int MAZE_CROSS_NUM = 6;         //迷宫由几条线交叉而成
        public const int MAZE_CROSS_DIST_CM = 30;    //间隔的长度
        public const int MAZE_SHORT_BORDER_CM = 32;  //迷宫最短的靠边距离
        public const int MAZE_LONG_BORDER_CM = MAZE_SHORT_BORDER_CM
                                             + MAZE_CROSS_DIST_CM * (MAZE_CROSS_NUM - 1)
                                             + MAZE_SIDE_BORDER_CM * 2;//迷宫最长的靠边距离
        public const int MAZE_SIDE_BORDER_CM = 20;         
        public const double COINCIDE_ERR_DIST_CM = 10;  //判定小车到达某点允许的最大误差距离
        public const int PKG_NUM_perGROUP = 6;       //场上每次刷新package物资的个数
        public GameStage gameStage;//比赛阶段
        public Camp UpperCamp; //当前半场需完成“上半场”任务的一方
        public GameState gameState;//比赛状态        
        public PassengerState psgState; // 目前场上被困人员的状况（同一时间场上最多1个被困人员）
        public Car CarA, CarB;//定义小车
        public Passenger curPsg;//当前被运载的乘客
        public Package[] currentPkgList;//当前场上的物资列表
        public PassengerGenerator psgGenerator;//仅用来生成乘客序列
        public PackageGenerator pkgGenerator; //仅用来生成物资序列
        public int mPackageGroupCount;//用于记录现在的Package是第几波
        public Flood mFlood;
        public Labyrinth mLabyrinth;
        public int mPrevTime;//时间均改为以毫秒为单位,记录上一次的时间，精确到秒，实时更新
        public int mGameTime;//时间均改为以毫秒为单位
        public int mLastWrongDirTime;
        public int mLastOnObstacleTime;
        public FileStream FoulTimeFS;
        public int mLastOnFloodTime;
        public Flood mLastFlood;

        public Game()//构造一个新的Game类 默认为CampA是先上半场上一阶段进行
        {
            Debug.WriteLine("开始执行Game构造函数");
            gameStage = GameStage.FIRST_1;
            UpperCamp = Camp.A;
            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 1);
            gameState = GameState.UNSTART;
            psgState = PassengerState.TRAPPED;
            psgGenerator = new PassengerGenerator(100);//上下半场将都用这一个索引
            pkgGenerator = new PackageGenerator(PKG_NUM_perGROUP * 5);
            currentPkgList = new Package[PKG_NUM_perGROUP];
            for (int i = 0;i<PKG_NUM_perGROUP;i++)
            {
                currentPkgList[i] = new Package();
            }
            curPsg = new Passenger(new Dot(-1, -1), new Dot(-1, -1)); //?
            mFlood = new Flood(0);
            mLastFlood = new Flood(0);
            mPackageGroupCount = 0;
            mLastWrongDirTime = -10;
            mLastOnObstacleTime = -10;
            mLastOnFloodTime = -10;

            mLabyrinth = new Labyrinth();
            Debug.WriteLine("Game构造函数FIRST_1执行完毕");
        }
        #region
        //每到半点自动更新Package信息函数,8.29已更新
        public void UpdatePackage()//更换Package函数,每次都更新，而只在半分钟的时候起作用
        {
            if (gameStage == GameStage.FIRST_1
                || gameStage == GameStage.LATTER_1)
            {
                return;
            }
            int changenum = mGameTime / 30000 + 1;
            if ((gameStage == GameStage.FIRST_2
                || gameStage == GameStage.LATTER_2)
                && mPackageGroupCount < changenum)
            {
                for (int i = 0; i < PKG_NUM_perGROUP; i++)
                {

                    currentPkgList[i]
                        = pkgGenerator.
                        GetPackage(i + PKG_NUM_perGROUP * mPackageGroupCount);
                }
                mPackageGroupCount++;
                Debug.WriteLine("UpdatePackage被触发，并执行完毕");
                Debug.WriteLine("第一个物资位置x{0},y{1}", currentPkgList[0].mPos.x, currentPkgList[0].mPos.y);
                Debug.WriteLine("第二个物资位置x{0},y{1}", currentPkgList[1].mPos.x, currentPkgList[1].mPos.y);
                Debug.WriteLine("第三个物资位置x{0},y{1}", currentPkgList[2].mPos.x, currentPkgList[2].mPos.y);
                Debug.WriteLine("第四个物资位置x{0},y{1}", currentPkgList[3].mPos.x, currentPkgList[3].mPos.y);
                Debug.WriteLine("第五个物资位置x{0},y{1}", currentPkgList[4].mPos.x, currentPkgList[4].mPos.y);
                Debug.WriteLine("第六个物资位置x{0},y{1}", currentPkgList[5].mPos.x, currentPkgList[5].mPos.y);
            }

        }

        //该方法用于返回系统现在的时间。开发者：xhl
        public int GetCurrentTime()
        {
            System.DateTime currentTime = System.DateTime.Now;
            int time = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000;
            //Debug.WriteLine("H, M, S: {0}, {1}, {2}", currentTime.Hour, currentTime.Minute, currentTime.Second);
            //Debug.WriteLine("GetCurrentTime，Time = {0}", time); 
            return time;
        }

        public static double GetDistance(Dot A, Dot B)//得到两个点之间的距离
        {
            return Math.Sqrt((A.x - B.x) * (A.x - B.x)
                + (A.y - B.y) * (A.y - B.y));
        }
        public void SetFloodArea()
        {
            int i, j;
            if(mFlood.num==0)
            {
                for(i=0;i<6;i++)
                {
                    for(j=0;j<6;j++)
                    {
                        Dot judge = new Dot(MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * i, MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * j);
                        if(gameStage==GameStage.FIRST_1)
                        {
                            if(GetDistance(judge,CarA.mPos)<=COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot1 = judge;
                                mFlood.num++;
                            }
                        }
                        if (gameStage == GameStage.LATTER_1)
                        {
                            if (GetDistance(judge, CarB.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot1 = judge;
                                mFlood.num++;
                            }
                        }
                    }
                }
            }
            else if (mFlood.num == 1)
            {
                for (i = 0; i < 6; i++)
                {
                    for (j = 0; j < 6; j++)
                    {
                        Dot judge = new Dot(MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * i, MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * j);
                        if (gameStage == GameStage.FIRST_1)
                        {
                            if (GetDistance(judge, CarA.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot2 = judge;
                                mFlood.num++;
                            }
                        }
                        if (gameStage == GameStage.LATTER_1)
                        {
                            if (GetDistance(judge, CarB.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot2 = judge;
                                mFlood.num++;
                            }
                        }
                    }
                }
            }
            else if (mFlood.num == 2)
            {
                for (i = 0; i < 6; i++)
                {
                    for (j = 0; j < 6; j++)
                    {
                        Dot judge = new Dot(MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * i, MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * j);
                        if (gameStage == GameStage.FIRST_1)
                        {
                            if (GetDistance(judge, CarA.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot3 = judge;
                                mFlood.num++;
                            }
                        }
                        if (gameStage == GameStage.LATTER_1)
                        {
                            if (GetDistance(judge, CarB.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot3 = judge;
                                mFlood.num++;
                            }
                        }
                    }
                }
            }
            else if (mFlood.num == 3)
            {
                for (i = 0; i < 6; i++)
                {
                    for (j = 0; j < 6; j++)
                    {
                        Dot judge = new Dot(MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * i, MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * j);
                        if (gameStage == GameStage.FIRST_1)
                        {
                            if (GetDistance(judge, CarA.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot4 = judge;
                                mFlood.num++;
                            }
                        }
                        if (gameStage == GameStage.LATTER_1)
                        {
                            if (GetDistance(judge, CarB.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot4 = judge;
                                mFlood.num++;
                            }
                        }
                    }
                }
            }
            else if (mFlood.num == 4)
            {
                for (i = 0; i < 6; i++)
                {
                    for (j = 0; j < 6; j++)
                    {
                        Dot judge = new Dot(MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * i, MAZE_SIDE_BORDER_CM + MAZE_SHORT_BORDER_CM + MAZE_CROSS_DIST_CM * j);
                        if (gameStage == GameStage.FIRST_1)
                        {
                            if (GetDistance(judge, CarA.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot5 = judge;
                                mFlood.num++;
                            }
                        }
                        if (gameStage == GameStage.LATTER_1)
                        {
                            if (GetDistance(judge, CarB.mPos) <= COINCIDE_ERR_DIST_CM)
                            {
                                mFlood.dot5 = judge;
                                mFlood.num++;
                            }
                        }
                    }
                }
            }
        }


        //这个函数可能放到dot里面更好
        public void JudgeAIsInMaze()//确定点是否在迷宫内
        {
            //Debug.WriteLine("开始执行JudgeAIsInMaze");
            if (CarA.mPos.x >= MAZE_SHORT_BORDER_CM
                && CarA.mPos.x <= MAZE_LONG_BORDER_CM
                && CarA.mPos.y >= MAZE_SHORT_BORDER_CM
                && CarA.mPos.y <= MAZE_LONG_BORDER_CM)
            {
                //Debug.WriteLine("A 在 Maze 中");
                CarA.mIsInMaze = 1;
            }
            else
            {
                //Debug.WriteLine("A 不在 Maze 中");
                CarA.mIsInMaze = 0;
            }
        }
        public void JudgeBIsInMaze()//确定点是否在迷宫内
        {
            Debug.WriteLine("开始执行JudgeBIsInMaze");
            if (CarB.mPos.x >= MAZE_SHORT_BORDER_CM
                && CarB.mPos.x <= MAZE_LONG_BORDER_CM
                && CarB.mPos.y >= MAZE_SHORT_BORDER_CM
                && CarB.mPos.y <= MAZE_LONG_BORDER_CM)
            {
                Debug.WriteLine("B 在 Maze 中");
                CarB.mIsInMaze = 1;
            }
            else
            {
                Debug.WriteLine("B 不在 Maze 中");
                CarB.mIsInMaze = 0;
            }
        }


        //下面为更新乘客信息函数
        public void UpdatePassenger()//更新乘客信息
        {
            Debug.WriteLine("开始执行 Update Passenger");
            curPsg = psgGenerator.Next();
            Debug.WriteLine("乘客位置x{0},y{1}",curPsg.Start_Dot.x, curPsg.Start_Dot.y);
            Debug.WriteLine("乘客位置x{0},y{1}", curPsg.End_Dot.x, curPsg.End_Dot.y);
            Debug.WriteLine("Next Passenger 成功更新");
        }

        public void CheckNextStage()//从上半场更换到下半场函数
        {
            //判断是否结束
            if (gameStage == GameStage.FIRST_1
                || gameStage == GameStage.LATTER_1)
            {
                if (mGameTime >= 60000)
                {
                    gameState = GameState.UNSTART;
                    gameStage++;
                    Debug.WriteLine("成功进入下一个stage");
                    UpdatePassenger();
                    mLastOnObstacleTime = -10;
                }
            }
            else
            {
                if (mGameTime >= 120000)
                {
                    gameState = GameState.UNSTART;
                    if (gameStage == GameStage.FIRST_2)
                    {
                        Debug.WriteLine("开始执行上下半场转换");
                        UpperCamp = Camp.B;//上半场转换
                        psgGenerator.ResetIndex();//Passenger的索引复位
                        mPackageGroupCount = 0;
                        mLastOnFloodTime = -10;
                        mLastOnObstacleTime = -10;
                        if (FoulTimeFS != null)                                           
                        {
                            byte[] data = Encoding.Default.GetBytes($"nextStage\r\n");
                            FoulTimeFS.Write(data, 0, data.Length);
                            // 如果不加以下两行的话，数据无法写到文件中
                            FoulTimeFS.Flush();
                            //FoulTimeFS.Close();
                        }
                        CarA.mTaskState = 1;//交换A和B的任务
                        CarB.mTaskState = 0;
                        gameStage++;
                        Debug.WriteLine("上下半场转换成功");
                        if(mLastFlood.num==1)
                        {
                            mLastFlood.dot1 = mFlood.dot1;
                        }

                    }
                }
            }
           
        }

        //下面四个为接口
        public void CheckCarAGetPassenger()//小车A接到了乘客
        {
            if (gameStage != GameStage.LATTER_2)
            {
                return;
            }
            if (GetDistance(CarA.mPos, curPsg.Start_Dot)
                <= COINCIDE_ERR_DIST_CM
                && CarA.mIsWithPassenger == 0)
            {
                Debug.WriteLine("A车接到了乘客，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                CarA.SwitchPassengerState();
            }

        }
        public void CheckCarBGetPassenger()//小车B接到了乘客
        {
            if (gameStage != GameStage.FIRST_2)
            {
                return;
            }
            if (GetDistance(CarB.mPos, curPsg.Start_Dot)
                <= COINCIDE_ERR_DIST_CM
                && CarB.mIsWithPassenger == 0)
            {
                Debug.WriteLine("B车接到了乘客，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                CarB.SwitchPassengerState();
            }
        }
        public void CheckCarATransPassenger()//小车A成功运送了乘客
        {
            if (gameStage != GameStage.LATTER_2)
            {
                return;
            }

            if (GetDistance(CarA.mPos, curPsg.End_Dot)
                <= COINCIDE_ERR_DIST_CM
                && CarA.mIsWithPassenger == 1)
            {
                CarA.SwitchPassengerState();
                CarA.AddRescueCount();
                Debug.WriteLine("A车送达了乘客，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                UpdatePassenger();
            }
            
        }
        public void CheckCarBTransPassenger()//小车B成功运送了乘客
        {
            if (gameStage != GameStage.FIRST_2)
            {
                return;
            }
            if (GetDistance(CarB.mPos, curPsg.End_Dot)
                <= COINCIDE_ERR_DIST_CM
                && CarB.mIsWithPassenger == 1)
            {
                CarB.SwitchPassengerState();
                CarB.AddRescueCount();
                Debug.WriteLine("B车送达了乘客，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                UpdatePassenger();
            }
            
        }

        //下面是两个关于包裹的接口
        public void CheckCarAGetpackage()//小车A得到了包裹
        {
            if (gameStage != GameStage.LATTER_2)
            {
                return;
            }
            for (int i = 0; i < PKG_NUM_perGROUP; i++)
            {
                if (GetDistance(CarA.mPos, currentPkgList[i].mPos)
                    <= COINCIDE_ERR_DIST_CM
                    && currentPkgList[i].IsPicked == 0)
                {
                    CarA.AddPickPkgCount();
                    currentPkgList[i].IsPicked = 1;
                    Debug.WriteLine("A车接到了包裹，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                }
            }

        }
        public void CheckCarBGetpackage()//小车B得到了包裹
        {
            if (gameStage != GameStage.FIRST_2)
            {
                return;
            }
            for (int i = 0; i < PKG_NUM_perGROUP; i++)
            {
                if (GetDistance(CarB.mPos, currentPkgList[i].mPos)
                    <= COINCIDE_ERR_DIST_CM
                    && currentPkgList[i].IsPicked == 0)
                {
                    CarB.AddPickPkgCount();
                    currentPkgList[i].IsPicked = 1;
                    Debug.WriteLine("B车接到了包裹，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                }

            }
        }

        public void CheckCarAonObstacle()//小车A到达了障碍上              
        {
            for (int i = 0; i < 8; i++)
            {
                if (mLabyrinth.mpWallList[i].w1.x
                    == mLabyrinth.mpWallList[i].w2.x)//障碍的两个点的横坐标相同
                {
                    if (mLabyrinth.mpWallList[i].w1.y
                        < mLabyrinth.mpWallList[i].w2.y)//障碍1在障碍2的下面
                    {
                        if (mLabyrinth.mpWallList[i].w1.x >= CarA.mPos.x - 5
                            && mLabyrinth.mpWallList[i].w1.x <= CarA.mPos.x + 5
                            && CarA.mPos.y <= mLabyrinth.mpWallList[i].w2.y
                            && mLabyrinth.mpWallList[i].w1.y <= CarA.mPos.y
                            && (mLabyrinth.mpWallList[i].w1.x <= CarA.mLastPos.x - 5
                            || mLabyrinth.mpWallList[i].w1.x >= CarA.mLastPos.x + 5
                            || CarA.mLastPos.y >= mLabyrinth.mpWallList[i].w2.y
                            || mLabyrinth.mpWallList[i].w1.y >= CarA.mLastPos.y)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarA.AddWallPunish();
                            Debug.WriteLine("A车撞到了竖着的墙，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                    if (mLabyrinth.mpWallList[i].w2.y < mLabyrinth.mpWallList[i].w1.y)//障碍2在障碍1的下面
                    {
                        if (mLabyrinth.mpWallList[i].w1.x >= CarA.mPos.x - 5
                            && mLabyrinth.mpWallList[i].w1.x <= CarA.mPos.x + 5
                            && CarA.mPos.y <= mLabyrinth.mpWallList[i].w1.y
                            && mLabyrinth.mpWallList[i].w2.y <= CarA.mPos.y
                            && (mLabyrinth.mpWallList[i].w1.x <= CarA.mLastPos.x - 5
                            || mLabyrinth.mpWallList[i].w1.x >= CarA.mLastPos.x + 5
                            || CarA.mLastPos.y >= mLabyrinth.mpWallList[i].w1.y
                            || mLabyrinth.mpWallList[i].w2.y >= CarA.mLastPos.y)&& mGameTime-mLastOnObstacleTime>=1000)
                        {
                            CarA.AddWallPunish();
                            Debug.WriteLine("A车撞到了竖着的墙，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                }
                if (mLabyrinth.mpWallList[i].w1.y == mLabyrinth.mpWallList[i].w2.y)//障碍的两个点的纵坐标相同
                {
                    if (mLabyrinth.mpWallList[i].w1.x < mLabyrinth.mpWallList[i].w2.x)//障碍1在障碍2的左面
                    {
                        if (mLabyrinth.mpWallList[i].w1.y >= CarA.mPos.y - 5
                            && mLabyrinth.mpWallList[i].w1.y <= CarA.mPos.y + 5
                            && CarA.mPos.x <= mLabyrinth.mpWallList[i].w2.x
                            && mLabyrinth.mpWallList[i].w1.x <= CarA.mPos.x
                            && (mLabyrinth.mpWallList[i].w1.y <= CarA.mLastPos.y - 5
                            || mLabyrinth.mpWallList[i].w1.y >= CarA.mLastPos.y + 5
                            || CarA.mLastPos.x >= mLabyrinth.mpWallList[i].w2.x
                            || mLabyrinth.mpWallList[i].w1.x >= CarA.mLastPos.x)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarA.AddWallPunish();
                            Debug.WriteLine("A车撞到了横着的墙，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                    if (mLabyrinth.mpWallList[i].w2.x < mLabyrinth.mpWallList[i].w1.x)//障碍2在障碍1的
                    {
                        if (mLabyrinth.mpWallList[i].w1.y >= CarA.mPos.y - 5
                            && mLabyrinth.mpWallList[i].w1.y <= CarA.mPos.y + 5
                            && CarA.mPos.x <= mLabyrinth.mpWallList[i].w1.x
                            && mLabyrinth.mpWallList[i].w2.x <= CarA.mPos.x
                            && (mLabyrinth.mpWallList[i].w1.y <= CarA.mLastPos.y - 5
                            || mLabyrinth.mpWallList[i].w1.y >= CarA.mLastPos.y + 5
                            || CarA.mLastPos.x >= mLabyrinth.mpWallList[i].w1.x
                            || mLabyrinth.mpWallList[i].w2.x >= CarA.mLastPos.x)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarA.AddWallPunish();
                            Debug.WriteLine("A车撞到了横着的墙，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }
                    }
                }
            }
        }
        public void CheckCarBonObstacle()//小车B到达了障碍上               
        {
            for (int i = 0; i < 8; i++)
            {
                if (mLabyrinth.mpWallList[i].w1.x
                    == mLabyrinth.mpWallList[i].w2.x)//障碍的两个点的横坐标相同
                {
                    if (mLabyrinth.mpWallList[i].w1.y
                        < mLabyrinth.mpWallList[i].w2.y)//障碍1在障碍2的下面
                    {
                        if (mLabyrinth.mpWallList[i].w1.x >= CarB.mPos.x - 5
                            && mLabyrinth.mpWallList[i].w1.x <= CarB.mPos.x + 5
                            && CarB.mPos.y <= mLabyrinth.mpWallList[i].w2.y
                            && mLabyrinth.mpWallList[i].w1.y <= CarB.mPos.y
                            && (mLabyrinth.mpWallList[i].w1.x <= CarB.mLastPos.x - 5
                            || mLabyrinth.mpWallList[i].w1.x >= CarB.mLastPos.x + 5
                            || CarB.mLastPos.y >= mLabyrinth.mpWallList[i].w2.y
                            || mLabyrinth.mpWallList[i].w1.y >= CarB.mLastPos.y)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarB.AddWallPunish();
                            Debug.WriteLine("B车撞到了竖着的墙，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                    if (mLabyrinth.mpWallList[i].w2.y < mLabyrinth.mpWallList[i].w1.y)//障碍2在障碍1的下面
                    {
                        if (mLabyrinth.mpWallList[i].w1.x >= CarB.mPos.x - 5
                            && mLabyrinth.mpWallList[i].w1.x <= CarB.mPos.x + 5
                            && CarB.mPos.y <= mLabyrinth.mpWallList[i].w1.y
                            && mLabyrinth.mpWallList[i].w2.y <= CarB.mPos.y
                            && (mLabyrinth.mpWallList[i].w1.x <= CarB.mLastPos.x - 5
                            || mLabyrinth.mpWallList[i].w1.x >= CarB.mLastPos.x + 5
                            || CarB.mLastPos.y >= mLabyrinth.mpWallList[i].w1.y
                            || mLabyrinth.mpWallList[i].w2.y >= CarB.mLastPos.y)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarB.AddWallPunish();
                            Debug.WriteLine("B车撞到了竖着的墙，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                }
                if (mLabyrinth.mpWallList[i].w1.y == mLabyrinth.mpWallList[i].w2.y)//障碍的两个点的纵坐标相同
                {
                    if (mLabyrinth.mpWallList[i].w1.x < mLabyrinth.mpWallList[i].w2.x)//障碍1在障碍2的左面
                    {
                        if (mLabyrinth.mpWallList[i].w1.y >= CarB.mPos.y - 5
                            && mLabyrinth.mpWallList[i].w1.y <= CarB.mPos.y + 5
                            && CarB.mPos.x <= mLabyrinth.mpWallList[i].w2.x
                            && mLabyrinth.mpWallList[i].w1.x <= CarB.mPos.x
                            && (mLabyrinth.mpWallList[i].w1.y <= CarB.mLastPos.y - 5
                            || mLabyrinth.mpWallList[i].w1.y >= CarB.mLastPos.y + 5
                            || CarB.mLastPos.x >= mLabyrinth.mpWallList[i].w2.x
                            || mLabyrinth.mpWallList[i].w1.x >= CarB.mLastPos.x)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarB.AddWallPunish();
                            Debug.WriteLine("B车撞到了横着的墙，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }

                    }
                    if (mLabyrinth.mpWallList[i].w2.x < mLabyrinth.mpWallList[i].w1.x)//障碍2在障碍1的
                    {
                        if (mLabyrinth.mpWallList[i].w1.y >= CarB.mPos.y - 5
                            && mLabyrinth.mpWallList[i].w1.y <= CarB.mPos.y + 5
                            && CarB.mPos.x <= mLabyrinth.mpWallList[i].w1.x
                            && mLabyrinth.mpWallList[i].w2.x <= CarB.mPos.x
                            && (mLabyrinth.mpWallList[i].w1.y <= CarB.mLastPos.y - 5
                            || mLabyrinth.mpWallList[i].w1.y >= CarB.mLastPos.y + 5
                            || CarB.mLastPos.x >= mLabyrinth.mpWallList[i].w1.x
                            || mLabyrinth.mpWallList[i].w2.x >= CarB.mLastPos.x)&& mGameTime - mLastOnObstacleTime >= 1000)
                        {
                            CarB.AddWallPunish();
                            Debug.WriteLine("B车撞到了横着的墙，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            mLastOnObstacleTime = mGameTime;
                        }
                    }
                }
            }
        }
        public void CheckCarAonFlood()//A车到大障碍上
        {

            if (CarA.mTaskState == 1)//在下半场的时候才应该判断小车是否经过Flood
            {
                if (mFlood.num == 0)
                {
                }
                else if (mFlood.num == 1)
                {
                    if (GetDistance(CarA.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime-mLastOnFloodTime>=1000)
                    {

                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 2)
                {

                    if (GetDistance(CarA.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 3)
                {

                    if (GetDistance(CarA.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 4)
                {

                    if (GetDistance(CarA.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot4) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot4) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 5)
                {

                    if (GetDistance(CarA.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot4) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot4) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarA.mPos, mFlood.dot5) <= COINCIDE_ERR_DIST_CM && GetDistance(CarA.mLastPos, mFlood.dot5) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarA.AddFloodPunish();
                        Debug.WriteLine("A车撞到了泄洪口，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }

            }
        }
        public void CheckCarBonFlood()
        {
            if (CarB.mTaskState == 1)//在下半场的时候才应该判断小车是否经过Flood
            {
                if (mFlood.num == 0)
                {
                }
                else if (mFlood.num == 1)
                {
                    if (GetDistance(CarB.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {

                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 2)
                {

                    if (GetDistance(CarB.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 3)
                {

                    if (GetDistance(CarB.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 4)
                {

                    if (GetDistance(CarB.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot4) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot4) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }
                else if (mFlood.num == 5)
                {

                    if (GetDistance(CarB.mPos, mFlood.dot1) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot1) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot2) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot2) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot3) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot3) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot4) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot4) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                    if (GetDistance(CarB.mPos, mFlood.dot5) <= COINCIDE_ERR_DIST_CM && GetDistance(CarB.mLastPos, mFlood.dot5) >= COINCIDE_ERR_DIST_CM && mGameTime - mLastOnFloodTime >= 1000)
                    {
                        CarB.AddFloodPunish();
                        Debug.WriteLine("B车撞到了泄洪口，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                        mLastOnFloodTime = mGameTime;
                    }
                }

            }
        }
        //逆行自动判断//目前为思路两次逆行之间间隔时间判断为5s，这5s之间的逆行忽略不计
        public void CheckCarAWrongDirection()
        {
            if (CarA.mLastPos.x < MAZE_SHORT_BORDER_CM && CarA.mPos.x < MAZE_SHORT_BORDER_CM
                && CarA.mLastPos.y > MAZE_SHORT_BORDER_CM && CarA.mLastPos.y < MAZE_LONG_BORDER_CM
                && CarA.mPos.y > MAZE_SHORT_BORDER_CM && CarA.mPos.y < MAZE_LONG_BORDER_CM
                && CarA.mPos.y > CarA.mLastPos.y && mGameTime - mLastWrongDirTime > 5000)
            {
                CarA.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("A车逆行！第{0}次", CarA.mWrongDirCount);
            }
            if (CarA.mLastPos.x > MAZE_LONG_BORDER_CM && CarA.mPos.x > MAZE_LONG_BORDER_CM
                && CarA.mLastPos.y > MAZE_SHORT_BORDER_CM
                && CarA.mLastPos.y < MAZE_LONG_BORDER_CM && CarA.mPos.y > MAZE_SHORT_BORDER_CM
                && CarA.mPos.y < MAZE_LONG_BORDER_CM && CarA.mPos.y < CarA.mLastPos.y 
                && mGameTime - mLastWrongDirTime > 5000)
            {
                CarA.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("A车逆行！第{0}次", CarA.mWrongDirCount);
            }
            if (CarA.mLastPos.y < MAZE_SHORT_BORDER_CM && CarA.mPos.y < MAZE_SHORT_BORDER_CM
                && CarA.mLastPos.x > MAZE_SHORT_BORDER_CM && CarA.mLastPos.x < MAZE_LONG_BORDER_CM
                && CarA.mPos.x > MAZE_SHORT_BORDER_CM && CarA.mPos.x < MAZE_LONG_BORDER_CM
                && CarA.mPos.x < CarA.mLastPos.x && mGameTime - mLastWrongDirTime > 5000)
            {
                CarA.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("A车逆行！第{0}次", CarA.mWrongDirCount);
            }
            if (CarA.mLastPos.y > MAZE_LONG_BORDER_CM && CarA.mPos.y > MAZE_LONG_BORDER_CM
                && CarA.mLastPos.x > MAZE_SHORT_BORDER_CM
                && CarA.mLastPos.x < MAZE_LONG_BORDER_CM && CarA.mPos.x > MAZE_SHORT_BORDER_CM
                && CarA.mPos.x < MAZE_LONG_BORDER_CM && CarA.mPos.x > CarA.mLastPos.x 
                && mGameTime - mLastWrongDirTime > 5000)
            {
                CarA.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("A车逆行！第{0}次", CarA.mWrongDirCount);
            }
        }
        public void CheckCarBWrongDirection()
        {
            if (CarB.mLastPos.x < MAZE_SHORT_BORDER_CM && CarB.mPos.x < MAZE_SHORT_BORDER_CM
                && CarB.mLastPos.y > MAZE_SHORT_BORDER_CM && CarB.mLastPos.y < MAZE_LONG_BORDER_CM
                && CarB.mPos.y > MAZE_SHORT_BORDER_CM && CarB.mPos.y < MAZE_LONG_BORDER_CM
                && CarB.mPos.y > CarB.mLastPos.y && mGameTime - mLastWrongDirTime > 5000)
            {
                CarB.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("B车逆行！第{0}次", CarB.mWrongDirCount);
            }
            if (CarB.mLastPos.x > MAZE_LONG_BORDER_CM && CarB.mPos.x > MAZE_LONG_BORDER_CM
                && CarB.mLastPos.y > MAZE_SHORT_BORDER_CM && CarB.mLastPos.y < MAZE_LONG_BORDER_CM
                && CarB.mPos.y > MAZE_SHORT_BORDER_CM && CarB.mPos.y < MAZE_LONG_BORDER_CM
                && CarB.mPos.y < CarB.mLastPos.y && mGameTime - mLastWrongDirTime > 5000)
            {
                CarB.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("B车逆行！第{0}次", CarB.mWrongDirCount);
            }
            if (CarB.mLastPos.y < MAZE_SHORT_BORDER_CM && CarB.mPos.y < MAZE_SHORT_BORDER_CM
                && CarB.mLastPos.x > MAZE_SHORT_BORDER_CM && CarB.mLastPos.x < MAZE_LONG_BORDER_CM
                && CarB.mPos.x > MAZE_SHORT_BORDER_CM && CarB.mPos.x < MAZE_LONG_BORDER_CM
                && CarB.mPos.x < CarB.mLastPos.x && mGameTime - mLastWrongDirTime > 5000)
            {
                CarB.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("B车逆行！第{0}次", CarB.mWrongDirCount);
            }
            if (CarB.mLastPos.y > MAZE_LONG_BORDER_CM && CarB.mPos.y > MAZE_LONG_BORDER_CM
                && CarB.mLastPos.x > MAZE_SHORT_BORDER_CM && CarB.mLastPos.x < MAZE_LONG_BORDER_CM
                && CarB.mPos.x > MAZE_SHORT_BORDER_CM && CarB.mPos.x < MAZE_LONG_BORDER_CM
                && CarB.mPos.x > CarB.mLastPos.x && mGameTime - mLastWrongDirTime > 5000)
            {
                CarB.AddWrongDirection();
                mLastWrongDirTime = mGameTime;
                Debug.WriteLine("B车逆行！第{0}次", CarB.mWrongDirCount);
            }
        }



        public void UpdateGameTime()
        {
            if (gameState == GameState.NORMAL)
            {
                mGameTime = GetCurrentTime() - mPrevTime + mGameTime;
            }
            mPrevTime = GetCurrentTime();
        }
        #endregion

        public void UpdateCarATransmPos()
        {
            if(gameState == GameState.NORMAL)
            {
                if(CarA.mIsInMaze==0||CarA.mRightPosCount==9)
                {
                    CarA.mTransPos = CarA.mPos;
                    CarA.mRightPosCount = 0;
                    CarA.mRightPos = 1;
                }
                if(CarA.mIsInMaze==1 && CarA.mRightPosCount!=9)
                {
                    CarA.mRightPosCount++;
                    CarA.mRightPos = 0;
                }
            }
        }
        public void UpdateCarBTransmPos()
        {
            if (gameState == GameState.NORMAL)
            {
                if (CarB.mIsInMaze == 0 || CarB.mRightPosCount == 9)
                {
                    CarB.mTransPos = CarB.mPos;
                    CarB.mRightPosCount = 0;
                    CarB.mRightPos = 1;
                }
                if (CarB.mIsInMaze == 1 && CarB.mRightPosCount != 9)
                {
                    CarB.mRightPosCount++;
                    CarB.mRightPos = 0;
                }
            }
        }
        //0.1s
        public void Update()
        {
            if (gameState == GameState.NORMAL)
            {
                UpdateGameTime();
                UpdatePackage();
                if (gameStage == GameStage.FIRST_1 || gameStage == GameStage.LATTER_2)
                {
                    JudgeAIsInMaze();
                    CheckCarAGetpackage();
                    CheckCarAGetPassenger();
                    CheckCarAonFlood();
                    CheckCarAonObstacle();
                    CheckCarATransPassenger();
                    //CheckCarAWrongDirection();
                    Debug.WriteLine("0.1 Update！");
                }
                else
                {
                    JudgeBIsInMaze();
                    CheckCarBGetpackage();
                    CheckCarBGetPassenger();
                    CheckCarBonFlood();
                    CheckCarBonObstacle();
                    CheckCarBTransPassenger();
                    //CheckCarBWrongDirection();
                    Debug.WriteLine("0.1 Update！");
                }
                CheckNextStage();
                UpdateCarATransmPos();
                UpdateCarBTransmPos();
                Debug.WriteLine("小车位置x{0},y{1}", CarB.mPos.x, CarB.mPos.y);
                Debug.WriteLine("物资1 x{0} y{1}", currentPkgList[0].mPos.x, currentPkgList[0].mPos.y);
                Debug.WriteLine("物资2 x{0} y{1}", currentPkgList[1].mPos.x, currentPkgList[1].mPos.y);
                Debug.WriteLine("物资3 x{0} y{1}", currentPkgList[2].mPos.x, currentPkgList[2].mPos.y);
                Debug.WriteLine("物资4 x{0} y{1}", currentPkgList[3].mPos.x, currentPkgList[3].mPos.y);
                Debug.WriteLine("物资5 x{0} y{1}", currentPkgList[4].mPos.x, currentPkgList[4].mPos.y);
                Debug.WriteLine("物资6 x{0} y{1}", currentPkgList[5].mPos.x, currentPkgList[5].mPos.y);
                Debug.WriteLine("泄洪口数{0}", mFlood.num) ;
            }
        }

        #region 1秒区域
        public void UpdateCarLastOneSecondPos()
        {
            if (gameState == GameState.NORMAL)
            {
                CarA.mLastOneSecondPos = CarA.mPos;
                CarB.mLastOneSecondPos = CarB.mPos;
                Debug.WriteLine("Update CarPos A位置 x {0}, y {1}, B位置 x {2}, y {3}", CarA.mPos.x, CarA.mPos.y, CarB.mPos.x, CarB.mPos.y);
            }
        }

        public void SetFlood()
        {
            if (gameStage == GameStage.FIRST_1)
            {
                for (int i = 1; i <= 6; i++)
                {

                    for (int j = 1; j <= 6; j++)
                    {
                        if (GetDistance(CarA.mLastOneSecondPos,
                            new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15)) < COINCIDE_ERR_DIST_CM &&
                            GetDistance(CarA.mPos,
                            new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15)) < COINCIDE_ERR_DIST_CM)
                        {
                            if (mFlood.num == 0)
                            {
                                mFlood.dot1 = new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15);
                                mFlood.num = 1;
                                Debug.WriteLine("A 设置了泄洪口 1，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            }
                            if (mFlood.num == 1)
                            {
                                mFlood.dot2 = new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                                                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15);
                                mFlood.num = 2;
                                Debug.WriteLine("A 设置了泄洪口 2，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                            }
                        }


                    }
                }

            }
            if (gameStage == GameStage.LATTER_1)
            {
                for (int i = 1; i <= 6; i++)
                {

                    for (int j = 1; j <= 6; j++)
                    {
                        if (GetDistance(CarB.mLastOneSecondPos,
                            new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15)) < COINCIDE_ERR_DIST_CM &&
                            GetDistance(CarB.mPos,
                            new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15)) < COINCIDE_ERR_DIST_CM)
                        {
                            if (mFlood.num == 0)
                            {
                                mFlood.dot1 = new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15);
                                mFlood.num = 1;
                                Debug.WriteLine("B 设置了泄洪口 1，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            }
                            if (mFlood.num == 1)
                            {
                                mFlood.dot2 = new Dot(MAZE_SHORT_BORDER_CM + i * MAZE_CROSS_DIST_CM - 15,
                                                            MAZE_CROSS_DIST_CM + j * MAZE_CROSS_DIST_CM - 15);
                                mFlood.num = 2;
                                Debug.WriteLine("B 设置了泄洪口 2，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                            }
                        }

                    }
                }

            }
        }
        #endregion
        #region 按键功能函数
        //点击开始键时调用Start函数 
        //上半场上一阶段、上半场下一阶段、下半场上一阶段、
        //下半场下一阶段开始时需要这一函数都需要调用这一函数来开始
        //暂停不用这个函数开始
        public void Start() //开始比赛上下半场都用这个
        {
            if (gameState == GameState.UNSTART)
            {
                gameState = GameState.NORMAL;
                mGameTime = 0;
                mPrevTime = GetCurrentTime();
                Debug.WriteLine("start");
            }
        }

        //点击暂停比赛键时调用Pause函数
        public void Pause() //暂停比赛
        {
            gameState = GameState.PAUSE;
        }

        //半场交换函数自动调用依照时间控制


        //在暂停后需要摁下继续按钮来继续比赛
        public void Continue()
        {
            gameState = GameState.NORMAL;
            mPrevTime = GetCurrentTime();
        }
        //重置摁键对应的函数
        //@TODO
        public void Reset()
        {
            //Game = new Game();
        }
        #endregion
        public byte[] PackCarAMessage()//已更新到最新通信协议
        {
            byte[] message = new byte[70]; //上位机传递多少信息
            int messageCnt = 0;
            message[messageCnt++] = (byte)((mGameTime/1000) >> 8);
            message[messageCnt++] = (byte)(mGameTime/1000);
            message[messageCnt++] = (byte)( (((byte)gameState << 6) & 0xC0 ) | (((byte)CarA.mTaskState << 5) & 0x20 ) | 
                (((byte)CarA.mIsWithPassenger << 3) & 0x08) | ((byte)mFlood.num & 0x07));
            message[messageCnt++] = (byte)CarA.mTransPos.x;
            message[messageCnt++] = (byte)CarA.mTransPos.y;
            message[messageCnt++] = (byte)mFlood.dot1.x;
            message[messageCnt++] = (byte)mFlood.dot1.y;
            message[messageCnt++] = (byte)mFlood.dot2.x;
            message[messageCnt++] = (byte)mFlood.dot2.y;
            message[messageCnt++] = (byte)mFlood.dot3.x;
            message[messageCnt++] = (byte)mFlood.dot3.y;
            message[messageCnt++] = (byte)mFlood.dot4.x;
            message[messageCnt++] = (byte)mFlood.dot4.y;
            message[messageCnt++] = (byte)mFlood.dot5.x;
            message[messageCnt++] = (byte)mFlood.dot5.y;
            message[messageCnt++] = (byte)curPsg.Start_Dot.x;
            message[messageCnt++] = (byte)curPsg.Start_Dot.y;
            message[messageCnt++] = (byte)curPsg.End_Dot.x;
            message[messageCnt++] = (byte)curPsg.End_Dot.y;
            message[messageCnt++] = (byte)((((byte)currentPkgList[0].IsPicked << 7) & 0x80) | (((byte)currentPkgList[1].IsPicked << 6) & 0x40) 
                | (((byte)currentPkgList[2].IsPicked << 5) & 0x20)
                | (((byte)currentPkgList[3].IsPicked << 4) & 0x10) | (((byte)currentPkgList[4].IsPicked << 3) & 0x08) |
                (((byte)currentPkgList[5].IsPicked << 2)&0x04) | (((byte)CarA.mIsInMaze << 1) & 0x02) | ((byte)CarA.mRightPos & 0x01));
            message[messageCnt++] = (byte)currentPkgList[0].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[0].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[1].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[1].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[2].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[2].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[3].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[3].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[4].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[4].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[5].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[5].mPos.y;
            message[messageCnt++] = (byte)(CarA.MyScore >> 8);
            message[messageCnt++] = (byte)CarA.MyScore;
            message[messageCnt++] = (byte)CarA.mRescueCount;
            message[messageCnt++] = (byte)CarA.mPkgCount;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w2.y;
            message[messageCnt++] = 0x0D;
            message[messageCnt++] = 0x0A;
            return message;
        }
        public byte[] PackCarBMessage()//已更新到最新通信协议
        {
            byte[] message = new byte[70]; //上位机传递多少信息
            int messageCnt = 0;
            message[messageCnt++] = (byte)((mGameTime/1000) >> 8);
            message[messageCnt++] = (byte)(mGameTime/1000);
            message[messageCnt++] = (byte)((((byte)gameState << 6) & 0xC0) | (((byte)CarB.mTaskState << 5) & 0x20) |
                (((byte)CarB.mIsWithPassenger << 3) & 0x08) | ((byte)mFlood.num & 0x07));
            message[messageCnt++] = (byte)CarB.mTransPos.x;
            message[messageCnt++] = (byte)CarB.mTransPos.y;
            message[messageCnt++] = (byte)mFlood.dot1.x;
            message[messageCnt++] = (byte)mFlood.dot1.y;
            message[messageCnt++] = (byte)mFlood.dot2.x;
            message[messageCnt++] = (byte)mFlood.dot2.y;
            message[messageCnt++] = (byte)mFlood.dot3.x;
            message[messageCnt++] = (byte)mFlood.dot3.y;
            message[messageCnt++] = (byte)mFlood.dot4.x;
            message[messageCnt++] = (byte)mFlood.dot4.y;
            message[messageCnt++] = (byte)mFlood.dot5.x;
            message[messageCnt++] = (byte)mFlood.dot5.y;
            message[messageCnt++] = (byte)curPsg.Start_Dot.x;
            message[messageCnt++] = (byte)curPsg.Start_Dot.y;
            message[messageCnt++] = (byte)curPsg.End_Dot.x;
            message[messageCnt++] = (byte)curPsg.End_Dot.y;
            message[messageCnt++] = (byte)((((byte)currentPkgList[0].IsPicked << 7) & 0x80) | (((byte)currentPkgList[1].IsPicked << 6) & 0x40)
                | (((byte)currentPkgList[2].IsPicked << 5) & 0x20)
                | (((byte)currentPkgList[3].IsPicked << 4) & 0x10) | (((byte)currentPkgList[4].IsPicked << 3) & 0x08) |
                (((byte)currentPkgList[5].IsPicked << 2) & 0x04) | (((byte)CarB.mIsInMaze << 1) & 0x02) | ((byte)CarB.mRightPos & 0x01));
            message[messageCnt++] = (byte)currentPkgList[0].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[0].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[1].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[1].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[2].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[2].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[3].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[3].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[4].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[4].mPos.y;
            message[messageCnt++] = (byte)currentPkgList[5].mPos.x;
            message[messageCnt++] = (byte)currentPkgList[5].mPos.y;
            message[messageCnt++] = (byte)(CarB.MyScore >> 8);
            message[messageCnt++] = (byte)CarB.MyScore;
            message[messageCnt++] = (byte)CarB.mRescueCount;
            message[messageCnt++] = (byte)CarB.mPkgCount;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[0].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[1].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[2].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[3].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[4].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[5].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[6].w2.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w1.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w1.y;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w2.x;
            message[messageCnt++] = (byte)mLabyrinth.mpWallList[7].w2.y;
            message[messageCnt++] = 0x0D;
            message[messageCnt++] = 0x0A;
            return message;
        }
    }
}