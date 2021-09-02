 /********************************
zigbee.h
接受上位机的数据
接收说明（以USART2为例）
	在程序开始的时候使用zigbee_Init(&huart2)进行初始化;
	在回调函数中使用zigbeeMessageRecord(void)记录数据，并重新开启中断

数据说明
    struct BasicInfo Game;储存比赛状态、时间、泄洪口信息
    struct CarInfo CarInfo;//储存车辆信息
    struct PassengerInfo Passenger;//储存人员的信息、位置和送达位置
    struct PackageInfo Package[6];//储存物资的信息
    struct StopInfo Stop[2];//储存隔离点位置信息
    struct ObstacleInfo Obstacle[8];//储存虚拟障碍物信息
    通过接口获取数据
**********************************/
#ifndef ZIGBEE_H
#define ZIGBEE_H
#include "stm32f1xx_hal.h"
#define INVALID_ARG -1
#define ZIGBEE_MESSAGE_LENTH 70

struct Position
{
    unsigned int X;
    unsigned int Y;
};

struct BasicInfo
{
    uint8_t GameState;	//游戏状态：00未开始，01进行中，10暂停，11结束
    uint16_t Time;	//比赛时间，以0.1s为单位
    uint8_t stop;   //泄洪口开启信息
};
struct CarInfo
{
    struct Position pos;    //小车位置
    uint16_t score;         //得分
    uint8_t picknum;         //小车成功收集物资个数
    uint8_t task;           //小车任务，0上半场，1下半场
    uint8_t transport;         //小车上是否有人
    uint8_t transportnum;      //小车运送人的个数
    uint8_t area;    //小车所在的区域
    uint8_t WhetherRightPos;//小车这次位置信息是否是正确的，1是正确的，0是不正确的.
};
struct PassengerInfo
{
    struct Position startpos;   //人员初始位置
    struct Position finalpos;   //人员要到达的位置
};
struct PackageInfo
{
    uint8_t No;               //物资编号
    struct Position pos;
    uint8_t whetherpicked;    //物资是否已被拾取
};
struct FloodInfo
{
    uint8_t FloodNo;
    struct Position pos;
};
struct ObstacleInfo
{
    uint8_t ObstacleNo;
    struct Position posA;
    struct Position posB;
};
enum GameStateEnum
{
	GameNotStart,	//未开始
	GameGoing,		//进行中
	GamePause,		//暂停中
	GameOver			//已结束
};
/**************接口*************************/
void zigbee_Init(UART_HandleTypeDef *huart);//初始化
void zigbeeMessageRecord(void);							//实时记录信息，在每次接收完成后更新数据，重新开启中断

enum GameStateEnum getGameState(void);			//比赛状态
uint16_t getGameTime(void);	                  //比赛时间，单位为0.1s
uint16_t getPassengerstartposX(void);			//人员初始位置
uint16_t getPassengerstartposY(void);
struct Position getPassengerstartpos(void);
uint16_t getPassengerfinalposX(void);           //人员需到达位置
uint16_t getPassengerfinalposY(void);
struct Position getPassengerfinalpos(void);
uint16_t getGameFlood(void);               //隔离点开启信息
uint16_t getFloodposX(int FloodNo);			//隔离点位置X
uint16_t getFloodposY(int FloodNo);           //隔离点位置Y
struct Position getFloodpos(int FloodNo);     //隔离点位置
uint16_t getCarposX();		    //小车x坐标
uint16_t getCarposY();			//小车y坐标
struct Position getCarpos();	//小车位置
//uint16_t getCarWhetherRightPos();//小车这次位置信息是否是正确的
uint16_t getPackageposX(int PackNo);		    //物资x坐标
uint16_t getPackageposY(int PackNo);			//物资y坐标
uint16_t getPackagewhetherpicked(int PackNo);   //物资是否已被收集
struct Position getPackagepos(int PackNo);	//物资位置
uint16_t getCarpicknum();//小车收集数
uint16_t getCartransportnum();//小车运送人员数
uint16_t getCartransport();//小车是否正在运送人员
uint16_t getCarscore();//小车得分
uint16_t getCartask();//小车任务
uint16_t getCararea();//小车区域
uint16_t getObstacleAposX(int ObstacleNo);		    //虚拟障碍Ax坐标
uint16_t getObstacleAposY(int ObstacleNo);			//虚拟障碍Ay坐标
uint16_t getObstacleBposX(int ObstacleNo);          //虚拟障碍Bx坐标
uint16_t getObstacleBposY(int ObstacleNo);          //虚拟障碍By坐标
struct Position getObstacleApos(int ObstacleNo);	//虚拟障碍A位置
struct Position getObstacleBpos(int ObstacleNo);	//虚拟障碍B位置
void Decode();
int receiveIndexMinus(int index_h, int num);
int receiveIndexAdd(int index_h, int num);
#endif // V0_5_H_INCLUDED
