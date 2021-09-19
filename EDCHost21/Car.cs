﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    // 队名
    public enum Camp
    {
        NONE = 0, A, B
    };
    public class Car // 选手的车
    {
        public const int CARIN_CREDIT = 10;         // 车进入中心矿区可以得到10分；
        public const int LOAD1_CREDIT = 20;        // 第一回合，走到一个金矿处可以得到20分;
        public const int UNLOAD1_CREDIT = 20;     // 第一回合，将所有金矿运送到指定地点可以得到20分;
        public const int LOAD2_CREDIT = 5;       // 第二回合，收集到一个金矿可以得到5分；
        public const int UNLOAD2_CREDIT = 20;     // 第二回合，将装满的金矿运送到指定地点可以得到20分；
        public const int AHEAD_CREDIT_PS = 1;         // 提前完成第一回合任务，每提前1sec加1分；
        public const int BEACON_CREDIT = 5;      // 放置信标可以得到5分；
        public const int BEACON_PENALTY = 50;     // 触碰到信标惩罚50分；
        public const int FOUL_PENALTY = 50;      // 犯规扣分50分；
        public const int BEACON_LIMIT = 3;      // 最多放置3个信标；
        public const int MINE_LIMIT = 5;        // 最多载5个金矿；
        public const int PORT_COUNT_LIMIT = 5;  // 小车在第二回合最多成功运5次（得100分）

        


        public Dot mPos;                // 当前位置
        public Dot mLastPos;            // 上一帧的位置，用于判断碰撞
        public Dot mTransPos;           // 发送的位置
        // public Dot mLastOneSecondPos;      // 上一秒的位置，便于实现1Hz发送
        // public Dot mTransPos;               // 最终发送的位置，10Hz则为mPos，1Hz则为mLastOneSecondPos
        public Camp MyCamp;               // A or B get、set直接两个封装好的函数
        public int MyScore;               // 得分
        public int mMine1Load;             // 小车在第一回合成功收集金矿个数
        public int mMine1Unload;           // 小车在第一回合成功运送金矿的次数
        public int mMine2Load;              // 小车在第二回合成功收集金矿的个数
        public int mMine2Unload;            // 小车在第二回合成功运送金矿的次数
        public int mAheadSec;              // 第一回合提前完成的秒数
        public int mTaskState;            // 小车任务，0为第一回合，1为第二回合
        public int mMineState;             // 小车上载有金矿的个数
        public int mIsInMaze;             // 小车所在的区域 0在迷宫外 1在迷宫内
        public int mIsInField;            // 小车目前在不在场地内 0不在场地内 1在场地内
        public int mCrossBeaconCount;      // 小车触碰信标的次数
        public int mFoulCount;            // 犯规按键次数
        //public int mRightPos;             // 小车现在的位置信息是否是正确的，0为不正确的，1为正确的
        //public int mRightPosCount;        // 用于记录小车位置是否该正确了（实现1Hz）
        public int WhetherCarIn;          // 记录小车是否进入了迷宫
        public int mBeaconCount;          // 记录小车放置信标数目
        public int mRightPos;           // 车的坐标是否正确


        public Car(Camp c, int task)
        {

            MyCamp = c;
            mPos = new Dot(0, 0);
            mLastPos = new Dot(0, 0);
            MyScore = 0;
            mMine1Load = 0;
            mMine1Unload = 0;
            mMine2Load = 0;
            mBeaconCount = 0;
            mMine2Unload = 0;
            mAheadSec = 0;
            mTaskState = task;
            mMineState = 0;
            mIsInField = 0;
            mIsInMaze = 0;
            mCrossBeaconCount = 0;
            mFoulCount = 0;
            WhetherCarIn = 0;
            mTransPos = new Dot();
            mRightPos = 0;
        }
        public void UpdateLastPos() // 更新上一帧位置
        {
            mLastPos = mPos;
        }

        public void SetPos(Dot pos) // 设置当前位置
        {
            mPos = pos;
        }

        public void AddCrossBeacon() // 触碰信标次数
        {
            mCrossBeaconCount++;
            UpdateScore();
        }
        public void AddMineLoad(int round)  // 根据回合数(round=0代表回合1，round=1代表回合2），增加收集矿物次数1次
        {
            if (round == 0)
            {
                mMine1Load ++;
            }
            else
            {
                mMine2Load ++;
            }
            UpdateScore();
        }

        public void AddMineUnload(int round)   // 根据回合数(round=0代表回合1，round=1代表回合2），增加运送矿物次数1次
        {
            if (round == 0)
            {
                mMine1Unload += mMine1Unload >= 1 ? 0 : 1;
            }
            else
            {
                mMine2Unload += mMine2Unload >= PORT_COUNT_LIMIT ? 0 : 1;
            }
            UpdateScore();
        }
        public void AddFoulCount()
        {
            mFoulCount++;
            UpdateScore();
        }
        
        public void AddBeaconCount()         // 放置了1个信标
        {
            mBeaconCount += mBeaconCount >= BEACON_LIMIT ? 0 : 1;
            UpdateScore();
        }
        public void SetAheadSec(int _sec)   // 设置提前完成第一回合任务的秒数
        {
            mAheadSec = _sec;
            UpdateScore();
        }

        public bool IsMineStateFull()        // 是否满载
        {
            return mMineState >= MINE_LIMIT;
        }
        public void ClearMineState()  // 清空载矿
        {
            mMineState = 0;
        }
        public void AddMineState()  // 增加当前载矿数
        {
            mMineState += IsMineStateFull() ? 0 : 1;
        }
        public void SetCarIn()          // 车进入中心矿区
        {
            WhetherCarIn = 1;
            UpdateScore();
        }
               
        public void UpdateScore()
        {
            MyScore = mMine1Load * LOAD1_CREDIT + mMine2Load * LOAD2_CREDIT         // 收集到矿的分数
                + mMine1Unload * UNLOAD1_CREDIT + mMine2Unload * UNLOAD2_CREDIT     // 运送成功的分数
                - mCrossBeaconCount * BEACON_PENALTY                    // 触碰信标惩罚的分数
                - mFoulCount * FOUL_PENALTY                             // 犯规扣分
                + WhetherCarIn * CARIN_CREDIT                           // 车进入中心矿区的分数
                + mAheadSec * AHEAD_CREDIT_PS                          // 提前完成第一回合任务的分数
                + mBeaconCount * BEACON_CREDIT;                         // 放置信标的分数
        }
    }
}
