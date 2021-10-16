/********************************
zigbee.h
������λ��������
����˵������USART2Ϊ����
    �ڳ���ʼ��ʱ��ʹ��zigbee_Init(&huart2)���г�ʼ��;
    �ڻص�������ʹ��zigbeeMessageRecord(void)��¼���ݣ������¿����ж�

����˵��
     struct BasicInfo Game;//�������ʱ�䡢����״̬��Ϣ
     struct CarInfo CarInfo;//���泵����Ϣ
     struct ParkDotInfo ParkDotInfo;//����ͣ�����ܹ��洢�Ľ��������Ϣ
     struct MyBeaconInfo MyBeaconInfo;//���漺���ű�䵱�ֿ�洢�Ľ��������Ϣ
     struct MineInfo MineInfo;//������ǿ����Ч����Ϣ
    ͨ���ӿڻ�ȡ����
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
    uint8_t GameState; //����״̬��δ��ʼ00��������01����ͣ10������11
    uint16_t Time; //����ʱ�䣬��0.1sΪ��λ
};

struct CarInfo {
    uint8_t Task;//��������0�ϰ볡����1�°볡����
    uint8_t MineSumNum;//����Ŀǰ���н�������Ŀ
    uint8_t MineANum;//����Ŀǰ���н��A����Ŀ
    uint8_t MineBNum;//����Ŀǰ���н��B����Ŀ
    uint8_t MineCNum;//����Ŀǰ���н��C����Ŀ
    uint8_t MineDNum;//����Ŀǰ���н��D����Ŀ
    struct Position Pos;//С��λ��
    uint8_t IsCarPosValid;//����λ����Ϣ�Ƿ���Ч ��Ϣ��ЧΪ1����ЧΪ0
    uint8_t Zone;//0Ϊ���ܱߵ�·����1Ϊ���������
    uint16_t Score;//�÷�  


    uint32_t MineIntensity[2];//С�����Ĵ��������ǿ��
    uint16_t DistanceOfMyBeacon[3];//С��������3���ű�ľ���
    uint8_t IsDistanceOfMyBeaconValid[3];//С��������3���ű�����Ƿ���Ч ��Ϣ��ЧΪ1����ЧΪ0
    uint16_t DistanceOfRivalBeacon[3];//С�����Է�3���ű�ľ���
    uint8_t IsDistanceOfRivalBeaconValid[3];//С�����Է�3���ű�����Ƿ���Ч ��Ϣ��ЧΪ1����ЧΪ0
    
};

struct ParkDotInfo {
    uint8_t ParkDotMineType[8];//8��ͣ�����ܹ��洢�Ľ������
};

struct MyBeaconInfo{
    uint8_t MyBeaconMineType[3];//����3���ű�䵱�Ĵ洢�ֿ�洢�������
};

struct MineInfo
{
    uint8_t IsMineIntensityValid[2]; //���ǿ���Ƿ���Ч ��ЧΪ1����ЧΪ0
    uint8_t MineArrayType[2];//�������
};

enum GameStateEnum
{
    GameNotStart,	//δ��ʼ
    GameGoing,		//������
    GamePause,		//��ͣ��
    GameOver			//�ѽ���
};


/**************�ӿ�*************************/
void zigbee_Init(UART_HandleTypeDef* huart);    //��ʼ��
void zigbeeMessageRecord(void);							//ʵʱ��¼��Ϣ����ÿ�ν�����ɺ�������ݣ����¿����ж�

uint16_t getGameTime(void);	                                //��ȡ����ʱ�䣬��λΪ0.1s
enum GameStateEnum getGameState(void);			//��ȡ����״̬
uint16_t getCarTask(void);                                        //��ȡ��������
uint16_t getIsMineIntensityValid(int MineNo);          //��ȡ���ǿ���Ƿ���Ч ��ЧΪ1����ЧΪ0
uint16_t getMineArrayType(int MineNo);                 //��ȡ����2����������
uint16_t getParkDotMineType(int ParkDotNo);                 //��ȡ8��ͣ�����ܹ��洢�Ľ������
uint16_t getMyBeaconMineType(int BeaconNo);                 //��ȡ3�������ű�䵱�ֿ�ɴ洢�Ľ������
uint16_t getCarMineSumNum(void);                                //��ȡ����Ŀǰ���н�������Ŀ
uint16_t getCarMineANum(void);                                //��ȡ����Ŀǰ���н��A����Ŀ
uint16_t getCarMineBNum(void);                                //��ȡ����Ŀǰ���н��B����Ŀ
uint16_t getCarMineCNum(void);                                //��ȡ����Ŀǰ���н��C����Ŀ
uint16_t getCarMineDNum(void);                                //��ȡ����Ŀǰ���н��D����Ŀ
uint16_t getCarPosX(void);		                                //��ȡС��x����
uint16_t getCarPosY(void);			                            //��ȡС��y����
struct Position getCarPos(void);	                            //��ȡС��λ��
uint32_t getMineIntensity(int MineNo);                   //��ȡС�����Ĵ��������ǿ��
uint16_t getDistanceOfMyBeacon(int BeaconNo); //��ȡС��������3���ű�ľ���
uint16_t getDistanceOfRivalBeacon(int BeaconNo); //��ȡС�����Է�3���ű�ľ���
uint16_t getCarZone(void);                                       //��ȡ��������������Ϣ
uint16_t getIsCarPosValid(void);                              //��ȡ����λ����Ϣ�Ƿ���Ч
uint16_t getIsDistanceOfMyBeaconValid(int BeaconNo);       //��ȡС��������3���ű�����Ƿ���Ч
uint16_t getIsDistanceOfRivalBeaconValid(int BeaconNo);    //��ȡС�����Է�3���ű�����Ƿ���Ч
uint16_t getCarScore(void);                                   //��ȡ�����÷�


void Decode();
int receiveIndexMinus(int index_h, int num);
int receiveIndexAdd(int index_h, int num);

#endif // V0_5_H_INCLUDED
