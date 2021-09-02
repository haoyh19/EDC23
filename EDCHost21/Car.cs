using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDCHOST22
{
    //队名
    public enum Camp
    {
        NONE = 0, A, B
    };
    public class Car //选手的车
    {
        public const int PKG_CREDIT = 10;        //获取物资可以得到10分;
        public const int RESCUE_CREDIT = 30;     //营救人员可以得到30分;
        public const int FLOOD_PENALTY = 5;     //经过泄洪口惩罚15分;
        public const int OBST_PENALTY = 50;      //经过虚拟障碍物惩罚50分;
        public const int WRONG_DIR_PENALTY = 10; //逆行惩罚10分;
        public const int FOUL_PENALTY = 50;      //犯规扣分50分;


        public Dot mPos;
        public Dot mLastPos;
        public Dot mLastOneSecondPos;
        public Dot mTransPos;
        public Camp MyCamp;               //A or B get、set直接两个封装好的函数
        public int MyScore;               //得分
        public int mPkgCount;             //小车成功收集物资个数
        public int mTaskState;            //小车任务 0为上半场任务，1为下半场任务
        public int mIsWithPassenger;      //小车上是否载人 0未载人 1载人
        public int mRescueCount;          //小车成功运送人个数
        public int mIsInMaze;             //小车所在的区域 0在迷宫外 1在迷宫内
        public int mIsInField;            //小车目前在不在场地内 0不在场地内 1在场地内
        public int mCrossFloodCount;      //小车经过泄洪口的次数
        public int mCrossWallCount;       //小车经过虚拟障碍的次数
        public int mWrongDirCount;        //小车逆行次数
        public int mFoulCount;            //犯规摁键次数
        public int mRightPos;             //小车现在的位置信息是否是正确的，0为不正确的，1为正确的
        public int mRightPosCount;        //用于记录小车位置是否该正确了
        public int WhetherCarIn;          //记录小车是否进入了迷宫
        public int WhetherCarOut;          //记录小车是否会到入口


        public Car(Camp c, int task)
        {
            MyCamp = c;
            mPos = new Dot(0, 0);
            mLastPos = new Dot(0, 0);
            mLastOneSecondPos = new Dot(0, 0);
            mTransPos = new Dot(0, 0);
            MyScore = 0;
            mPkgCount = 0;
            mTaskState = task;
            mIsWithPassenger = 0;
            mRescueCount = 0;
            mIsInMaze = 0;
            mCrossFloodCount = 0;
            mCrossWallCount = 0;
            mWrongDirCount = 0;
            mFoulCount = 0; //xhl 0824 添加
            mRightPos = 1;
            mRightPosCount = 0;
            WhetherCarIn = 0;
            WhetherCarOut = 0;
        }
        public void UpdateLastPos()
        {
            mLastPos = mPos;
        }

        public void SetPos(Dot pos)
        {
            mPos = pos;
        }

        public void AddFloodPunish() //犯规
        {
            mCrossFloodCount++;
            UpdateScore();
        }
        public void AddWallPunish()
        {
            mCrossWallCount++; //前一个版本疑似typo（xhl）
            UpdateScore();
        }
        public void AddWrongDirection()
        {
            mWrongDirCount++;
            UpdateScore();
        }
        public void AddRescueCount()
        {
            mRescueCount++;
            UpdateScore();
        }

        public void AddPickPkgCount()
        {
            mPkgCount++;
            UpdateScore();
        }
        public void AddFoulCount()
        {
            mFoulCount++;
            UpdateScore();
        }
        public void SwitchPassengerState()
        {
            if (mIsWithPassenger == 0)
            {
                mIsWithPassenger = 1;
            }
            else
            {
                mIsWithPassenger = 0;
            }
        }
        public void CarGetIn()
        {
            WhetherCarIn = 1;
            UpdateScore();
        }
        public void CarGetOut()
        {
            WhetherCarOut = 1;
            UpdateScore();
        }
        //8-14 yd将Score后的代码折成多行，便于阅读
        public void UpdateScore()
        {
            MyScore = mPkgCount * PKG_CREDIT
                + mRescueCount * RESCUE_CREDIT
                - mCrossFloodCount * FLOOD_PENALTY
                - OBST_PENALTY * mCrossWallCount
                - mWrongDirCount * WRONG_DIR_PENALTY
                - mFoulCount * FOUL_PENALTY
                + WhetherCarIn * 25 + WhetherCarOut * 25;
        }
    }
}
