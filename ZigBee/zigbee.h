 /********************************
zigbee.h
������λ��������
����˵������USART2Ϊ����
	�ڳ���ʼ��ʱ��ʹ��zigbee_Init(&huart2)���г�ʼ��;
	�ڻص�������ʹ��zigbeeMessageRecord(void)��¼���ݣ������¿����ж�

����˵��
    struct BasicInfo Game;�������״̬��ʱ�䡢й�����Ϣ
    struct CarInfo CarInfo;//���泵����Ϣ
    struct PassengerInfo Passenger;//������Ա����Ϣ��λ�ú��ʹ�λ��
    struct PackageInfo Package[6];//�������ʵ���Ϣ
    struct StopInfo Stop[2];//��������λ����Ϣ
    struct ObstacleInfo Obstacle[8];//���������ϰ�����Ϣ
    ͨ���ӿڻ�ȡ����
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
    uint8_t GameState;	//��Ϸ״̬��00δ��ʼ��01�����У�10��ͣ��11����
    uint16_t Time;	//����ʱ�䣬��0.1sΪ��λ
    uint8_t stop;   //й��ڿ�����Ϣ
};
struct CarInfo
{
    struct Position pos;    //С��λ��
    uint16_t score;         //�÷�
    uint8_t picknum;         //С���ɹ��ռ����ʸ���
    uint8_t task;           //С������0�ϰ볡��1�°볡
    uint8_t transport;         //С�����Ƿ�����
    uint8_t transportnum;      //С�������˵ĸ���
    uint8_t area;    //С�����ڵ�����
    uint8_t WhetherRightPos;//С�����λ����Ϣ�Ƿ�����ȷ�ģ�1����ȷ�ģ�0�ǲ���ȷ��.
};
struct PassengerInfo
{
    struct Position startpos;   //��Ա��ʼλ��
    struct Position finalpos;   //��ԱҪ�����λ��
};
struct PackageInfo
{
    uint8_t No;               //���ʱ��
    struct Position pos;
    uint8_t whetherpicked;    //�����Ƿ��ѱ�ʰȡ
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
	GameNotStart,	//δ��ʼ
	GameGoing,		//������
	GamePause,		//��ͣ��
	GameOver			//�ѽ���
};
/**************�ӿ�*************************/
void zigbee_Init(UART_HandleTypeDef *huart);//��ʼ��
void zigbeeMessageRecord(void);							//ʵʱ��¼��Ϣ����ÿ�ν�����ɺ�������ݣ����¿����ж�

enum GameStateEnum getGameState(void);			//����״̬
uint16_t getGameTime(void);	                  //����ʱ�䣬��λΪ0.1s
uint16_t getPassengerstartposX(void);			//��Ա��ʼλ��
uint16_t getPassengerstartposY(void);
struct Position getPassengerstartpos(void);
uint16_t getPassengerfinalposX(void);           //��Ա�赽��λ��
uint16_t getPassengerfinalposY(void);
struct Position getPassengerfinalpos(void);
uint16_t getGameFlood(void);               //����㿪����Ϣ
uint16_t getFloodposX(int FloodNo);			//�����λ��X
uint16_t getFloodposY(int FloodNo);           //�����λ��Y
struct Position getFloodpos(int FloodNo);     //�����λ��
uint16_t getCarposX();		    //С��x����
uint16_t getCarposY();			//С��y����
struct Position getCarpos();	//С��λ��
//uint16_t getCarWhetherRightPos();//С�����λ����Ϣ�Ƿ�����ȷ��
uint16_t getPackageposX(int PackNo);		    //����x����
uint16_t getPackageposY(int PackNo);			//����y����
uint16_t getPackagewhetherpicked(int PackNo);   //�����Ƿ��ѱ��ռ�
struct Position getPackagepos(int PackNo);	//����λ��
uint16_t getCarpicknum();//С���ռ���
uint16_t getCartransportnum();//С��������Ա��
uint16_t getCartransport();//С���Ƿ�����������Ա
uint16_t getCarscore();//С���÷�
uint16_t getCartask();//С������
uint16_t getCararea();//С������
uint16_t getObstacleAposX(int ObstacleNo);		    //�����ϰ�Ax����
uint16_t getObstacleAposY(int ObstacleNo);			//�����ϰ�Ay����
uint16_t getObstacleBposX(int ObstacleNo);          //�����ϰ�Bx����
uint16_t getObstacleBposY(int ObstacleNo);          //�����ϰ�By����
struct Position getObstacleApos(int ObstacleNo);	//�����ϰ�Aλ��
struct Position getObstacleBpos(int ObstacleNo);	//�����ϰ�Bλ��
void Decode();
int receiveIndexMinus(int index_h, int num);
int receiveIndexAdd(int index_h, int num);
#endif // V0_5_H_INCLUDED
