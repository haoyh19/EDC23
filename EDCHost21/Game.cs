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
    // 比赛状态：未开始、正常进行中、暂停、结束
    public enum GameState { UNSTART = 0, NORMAL = 1, PAUSE = 2, END = 3 };

    // 比赛阶段：第一回合A车，第一回合B车，第二回合A车，第二回合B车，比赛结束
    public enum GameStage { FIRST_A = 0, FIRST_B, SECOND_B, SECOND_A ,END};

    public class Game
    {
        public const int MINE_COUNT_MAX = 2;        // 场上最多同时有2个矿

        public bool DebugMode;          // 调试模式，最大回合数 = 1,000,000
        public GameState mGameState;     // 比赛状态
        public GameStage mGameStage;     // 比赛阶段
        public Camp UpperCamp;          // 当前回合需先上场的一方
        public int mMineState;          // 场上有金矿的数目
        public Car CarA, CarB;          // 定义小车
        public Beacon mBeacon;          // 信标
        public int mPrevTime;           // 时间均改为以毫秒为单位,记录上一次的时间，精确到秒，实时更新
        public int mGameTime;           // 时间均改为以毫秒为单位
        public FileStream FoulTimeFS;
        public int mLastOnBeaconTime;
        public Beacon mLastBeacon;

        // 矿相关的变量
        // TODO


        // 构造一个新的Game类，默认为CampA是先上半场上一阶段进行
        // TODO
        public Game()
        {
            Debug.WriteLine("开始执行Game构造函数");
            mGameState = GameState.UNSTART;
            mGameStage = GameStage.FIRST_A;
            UpperCamp = Camp.A;
            mMineState = 0;
            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 0);
            mBeacon = new Beacon();
            mPrevTime = GetCurrentTime();
            mGameTime = 0;
            mLastOnBeaconTime = -10;
            mLastBeacon = new Beacon();
            Debug.WriteLine("Game构造函数FIRST_A执行完毕");

            // 矿相关变量的初始化
        }

        #region 辅助函数

        // 获取当前时间（秒）
        public int GetCurrentTime()
        {
            System.DateTime currentTime = System.DateTime.Now;
            int time = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000;
            //Debug.WriteLine("H, M, S: {0}, {1}, {2}", currentTime.Hour, currentTime.Minute, currentTime.Second);
            //Debug.WriteLine("GetCurrentTime，Time = {0}", time); 
            return time;
        }

        // 获取有效Beacon的数组
        // flag：0返回A和B设置的全部有效信标；1返回A设置的有效信标；2返回B设置的有效信标
        public Dot[] GetBeacon(int flag)
        {
            if (flag == 0)
            {
                Dot[] ret = new Dot[mBeacon.CarABeaconNum + mBeacon.CarBBeaconNum];
                int idx = 0;
                for (int i = 0; i < mBeacon.CarABeaconNum; i++)
                {
                    ret[idx] = new Dot(mBeacon.CarABeacon[i].x, mBeacon.CarABeacon[i].y);
                    idx++;
                }
                for (int i = 0; i< mBeacon.CarBBeaconNum; i++)
                {
                    ret[idx++] = new Dot(mBeacon.CarBBeacon[i].x, mBeacon.CarBBeacon[i].y);
                    idx++;
                }
                return ret;
            }
            else if (flag == 1)
            {
                Dot[] ret = new Dot[mBeacon.CarABeaconNum];
                int idx = 0;
                for (int i = 0; i < mBeacon.CarABeaconNum; i++)
                {
                    ret[idx] = new Dot(mBeacon.CarABeacon[i].x, mBeacon.CarABeacon[i].y);
                    idx++;
                }
                return ret;

            }
            else if (flag == 2)
            {
                Dot[] ret = new Dot[mBeacon.CarBBeaconNum];
                int idx = 0;
                for (int i = 0; i < mBeacon.CarBBeaconNum; i++)
                {
                    ret[idx] = new Dot(mBeacon.CarBBeacon[i].x, mBeacon.CarBBeacon[i].y);
                    idx++;
                }
                return ret;
            }
            else
            {
                return new Dot[0];
            }
        }

        #endregion


        #region 自动更新

        // 判断车是否在中心矿区内（及进入矿区自动加分）
        public void JudgeAIsInMaze()
        {
            //Debug.WriteLine("开始执行JudgeAIsInMaze");
            if (CarA.mPos.x >= Court.BORDER_CM
                && CarA.mPos.x <= Court.BORDER_CM + Court.MAZE_SIZE_CM
                && CarA.mPos.y >= Court.BORDER_CM
                && CarA.mPos.y <= Court.BORDER_CM + Court.MAZE_SIZE_CM)
            {
                //Debug.WriteLine("A 在 Maze 中");
                CarA.mIsInMaze = 1;
                if (CarA.WhetherCarIn == 0)
                {
                    CarA.SetCarIn();
                }
            }
            else
            {
                //Debug.WriteLine("A 不在 Maze 中");
                CarA.mIsInMaze = 0;
            }
        }
        public void JudgeBIsInMaze()
        {
            //Debug.WriteLine("开始执行JudgeAIsInMaze");
            if (CarB.mPos.x >= Court.BORDER_CM
                && CarB.mPos.x <= Court.BORDER_CM + Court.MAZE_SIZE_CM
                && CarB.mPos.y >= Court.BORDER_CM
                && CarB.mPos.y <= Court.BORDER_CM + Court.MAZE_SIZE_CM)
            {
                //Debug.WriteLine("A 在 Maze 中");
                CarB.mIsInMaze = 1;
                if (CarB.WhetherCarIn == 0)
                {
                    CarB.SetCarIn();
                }
            }
            else
            {
                //Debug.WriteLine("A 不在 Maze 中");
                CarB.mIsInMaze = 0;
            }
        }

        // 判断是否在场地内
        public void JudgeAIsInField()
        {
            //Debug.WriteLine("开始执行JudgeAIsInField");
            if (CarA.mPos.x >= 0
                && CarA.mPos.x <= Court.MAX_SIZE_CM
                && CarA.mPos.y >= 0
                && CarA.mPos.y <= Court.MAX_SIZE_CM)
            {
                //Debug.WriteLine("A 在 Field 中");
                CarA.mIsInField = 1;
            }
            else
            {
                //Debug.WriteLine("A 不在 Field 中");
                CarA.mIsInField = 0;
            }
        }
        public void JudgeBIsInField()
        {
            //Debug.WriteLine("开始执行JudgeBIsInField");
            if (CarB.mPos.x >= 0
                && CarB.mPos.x <= Court.MAX_SIZE_CM
                && CarB.mPos.y >= 0
                && CarB.mPos.y <= Court.MAX_SIZE_CM)
            {
                //Debug.WriteLine("B 在 Field 中");
                CarB.mIsInField = 1;
            }
            else
            {
                //Debug.WriteLine("B 不在 Field 中");
                CarB.mIsInField = 0;
            }
        }

        // 更新游戏时间
        public void UpdateGameTime()
        {
            if (mGameState == GameState.NORMAL)
            {
                mGameTime = GetCurrentTime() - mPrevTime + mGameTime;
            }
            mPrevTime = GetCurrentTime();
        }

        // 更新车的发送位置
        public void UpdateCarATransPos()
        {
            if (mGameState == GameState.NORMAL)
            {
                if (mGameStage == GameStage.FIRST_A)
                {
                    CarA.mTransPos = CarA.mPos;
                }
                else if (mGameStage == GameStage.SECOND_A && CarA.mIsInMaze != 1)
                {
                    CarA.mTransPos = CarA.mPos;
                }
                else
                {
                    CarA.mTransPos.SetInfo(-10, -10);
                }
            }
        }
        public void UpdateCarBTransPos()
        {
            if (mGameState == GameState.NORMAL)
            {
                if (mGameStage == GameStage.FIRST_B)
                {
                    CarB.mTransPos = CarB.mPos;
                }
                else if (mGameStage == GameStage.SECOND_B && CarB.mIsInMaze != 1)
                {
                    CarB.mTransPos = CarB.mPos;
                }
                else
                {
                    CarB.mTransPos.SetInfo(-10, -10);
                }
            }
        }

        // 更新车碰到对家信标
        public void CheckCarAOnBeacon()
        {
            if (mGameStage == GameStage.SECOND_A)
            {
                Dot[] beaconArray = GetBeacon(2);
                if (Dot.InCollisionZones(CarA.mPos, beaconArray) && 
                    (!Dot.InCollisionZones(CarA.mLastPos, beaconArray)))
                {
                    CarA.AddCrossBeacon();
                    Debug.WriteLine("A车撞到了B放置的信标，位置 x {0}, y {1}", CarA.mPos.x, CarA.mPos.y);
                    mLastOnBeaconTime = mGameTime;
                }
            }
        }
        public void CheckCarBOnBeacon()
        {
            if (mGameStage == GameStage.SECOND_B)
            {
                Dot[] beaconArray = GetBeacon(1);
                if (Dot.InCollisionZones(CarB.mPos, beaconArray) &&
                    (!Dot.InCollisionZones(CarB.mLastPos, beaconArray)))
                {
                    CarB.AddCrossBeacon();
                    Debug.WriteLine("B车撞到了A放置的信标，位置 x {0}, y {1}", CarB.mPos.x, CarB.mPos.y);
                    mLastOnBeaconTime = mGameTime;
                }
            }
        }

        #endregion


        #region 按键触发

        // 开始按键
        public void Start()
        {
            if (mGameState == GameState.UNSTART)
            {
                mGameState = GameState.NORMAL;
                mGameTime = 0;
                mPrevTime = GetCurrentTime();
                Debug.WriteLine("start");
            }
        }

        // 设置信标
        public void SetBeacon()
        {
            if (mGameState == GameState.END || mGameState == GameState.PAUSE || mGameState == GameState.UNSTART ||
                mGameStage == GameStage.END || mGameStage == GameStage.SECOND_A || mGameStage == GameStage.SECOND_B)
            {
                return;
            }
            if (UpperCamp == Camp.A && mGameStage == GameStage.FIRST_A)
            {
                if (mBeacon.CarABeaconNum >= mBeacon.MaxBeaconNum)
                {
                    return;
                }
                if (CarA.mIsInMaze != 1)
                {
                    return;
                }
                mBeacon.CarAAddBeacon(CarA.mPos);
                CarA.AddBeaconCount();
            }
            else if (UpperCamp == Camp.B && mGameStage == GameStage.FIRST_B)
            {
                if (mBeacon.CarBBeaconNum >= mBeacon.MaxBeaconNum)
                {
                    return;
                }
                if (CarB.mIsInMaze != 1)
                {
                    return;
                }
                mBeacon.CarBAddBeacon(CarB.mPos);
                CarB.AddBeaconCount();
            }
        }

        // 暂停按键
        public void Pause()
        {
            mGameState = GameState.PAUSE;
        }

        // 继续按键
        public void Continue()
        {
            mGameState = GameState.NORMAL;
            mPrevTime = GetCurrentTime();
        }

        // 重置按键，初始化比赛
        // TODO
        public void Reset()
        {
            Debug.WriteLine("初始化比赛");
            mGameState = GameState.UNSTART;
            mGameStage = GameStage.FIRST_A;
            UpperCamp = Camp.A;
            mMineState = 0;
            CarA = new Car(Camp.A, 0);
            CarB = new Car(Camp.B, 0);
            mBeacon = new Beacon();
            mPrevTime = GetCurrentTime();
            mGameTime = 0;
            mLastOnBeaconTime = -10;
            mLastBeacon = new Beacon();
            Debug.WriteLine("Game构造函数FIRST_A执行完毕");

            // 矿相关构造函数
        }

        #endregion





        ////////////////////////////////////// 下面的是去年代码


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