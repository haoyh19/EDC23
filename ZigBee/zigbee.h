/********************************
zigbee.h
接受上位机的数据
接收说明（以USART2为例）
    在程序开始的时候使用zigbee_Init(&huart2)进行初始化;
    在回调函数中使用zigbeeMessageRecord(void)记录数据，并重新开启中断

数据说明
     struct BasicInfo Game;//储存比赛时间、比赛状态信息
     struct CarInfo CarInfo;//储存车辆信息
     struct ParkDotInfo ParkDotInfo;//储存停车点能够存储的金矿种类信息
     struct MyBeaconInfo MyBeaconInfo;//储存己方信标充当仓库存储的金矿种类信息
     struct MineInfo MineInfo;//储存金矿强度有效性信息
    通过接口获取数据
**********************************/

#ifndef ZIGBEE_H
#define ZIGBEE_H
#include "stm32f1xx_hal.h"
#define INVALID_ARG -1
#define ZIGBEE_MESSAGE_LENTH 38

struct Position
{
    uint16_t x;
    uint16_t y;
};

struct BasicInfo
{
    uint8_t GameState; //比赛状态：未开始00，进行中01，暂停10，结束11
    uint16_t Time; //比赛时间，以0.1s为单位
};

struct CarInfo {
    uint8_t Task;//车辆任务：0上半场任务，1下半场任务
    uint8_t MineSumNum;//车辆目前载有金矿的总数目
    uint8_t MineANum;//车辆目前载有金矿A的数目
    uint8_t MineBNum;//车辆目前载有金矿B的数目
    uint8_t MineCNum;//车辆目前载有金矿C的数目
    uint8_t MineDNum;//车辆目前载有金矿D的数目
    struct Position Pos;//小车位置
    uint8_t IsCarPosValid;//车辆位置信息是否有效 信息有效为1，无效为0
    uint8_t Zone;//0为在周边道路区域，1为在中央矿区
    uint16_t Score;//得分  


    uint32_t MineIntensity[2];//小车中心处的两金矿强度
    uint16_t DistanceOfMyBeacon[3];//小车到己方3个信标的距离
    uint8_t IsDistanceOfMyBeaconValid[3];//小车到己方3个信标距离是否有效 信息有效为1，无效为0
    uint16_t DistanceOfRivalBeacon[3];//小车到对方3个信标的距离
    uint8_t IsDistanceOfRivalBeaconValid[3];//小车到对方3个信标距离是否有效 信息有效为1，无效为0
    
};

struct ParkDotInfo {
    uint8_t ParkDotMineType[8];//8个停车点能够存储的金矿种类
};

struct MyBeaconInfo{
    uint8_t MyBeaconMineType[3];//己方3个信标充当的存储仓库存储金矿种类
};

struct MineInfo
{
    uint8_t IsMineIntensityValid[2]; //金矿强度是否有效 有效为1，无效为0
    uint8_t MineArrayType[2];//金矿种类
};

enum GameStateEnum
{
    GameNotStart,	//未开始
    GameGoing,		//进行中
    GamePause,		//暂停中
    GameOver			//已结束
};


/**************接口*************************/
void zigbee_Init(UART_HandleTypeDef* huart);    //初始化
void zigbeeMessageRecord(void);							//实时记录信息，在每次接收完成后更新数据，重新开启中断

uint16_t getGameTime(void);	                                //获取比赛时间，单位为0.1s
enum GameStateEnum getGameState(void);			//获取比赛状态
uint16_t getCarTask(void);                                        //获取车辆任务
uint16_t getIsMineIntensityValid(int MineNo);          //获取金矿强度是否有效 有效为1，无效为0
uint16_t getMineArrayType(int MineNo);                 //获取场上2个金矿的种类
uint16_t getParkDotMineType(int ParkDotNo);                 //获取8个停车点能够存储的金矿种类
uint16_t getMyBeaconMineType(int BeaconNo);                 //获取3个己方信标充当仓库可存储的金矿类型
uint16_t getCarMineSumNum(void);                                //获取车辆目前载有金矿的总数目
uint16_t getCarMineANum(void);                                //获取车辆目前载有金矿A的数目
uint16_t getCarMineBNum(void);                                //获取车辆目前载有金矿B的数目
uint16_t getCarMineCNum(void);                                //获取车辆目前载有金矿C的数目
uint16_t getCarMineDNum(void);                                //获取车辆目前载有金矿D的数目
uint16_t getCarPosX(void);		                                //获取小车x坐标
uint16_t getCarPosY(void);			                            //获取小车y坐标
struct Position getCarPos(void);	                            //获取小车位置
uint32_t getMineIntensity(int MineNo);                   //获取小车中心处的两金矿强度
uint16_t getDistanceOfMyBeacon(int BeaconNo); //获取小车到己方3个信标的距离
uint16_t getDistanceOfRivalBeacon(int BeaconNo); //获取小车到对方3个信标的距离
uint16_t getCarZone(void);                                       //获取车辆所在区域信息
uint16_t getIsCarPosValid(void);                              //获取车辆位置信息是否有效
uint16_t getIsDistanceOfMyBeaconValid(int BeaconNo);       //获取小车到己方3个信标距离是否有效
uint16_t getIsDistanceOfRivalBeaconValid(int BeaconNo);    //获取小车到对方3个信标距离是否有效
uint16_t getCarScore(void);                                   //获取车辆得分


void Decode();
int receiveIndexMinus(int index_h, int num);
int receiveIndexAdd(int index_h, int num);

#endif // V0_5_H_INCLUDED
