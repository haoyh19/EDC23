#include"zigbee.h"
volatile uint8_t zigbeeReceive[ZIGBEE_MESSAGE_LENTH];	//实时记录收到的信息
volatile uint8_t zigbeeMessage[ZIGBEE_MESSAGE_LENTH];//经过整理顺序后得到的信息
volatile int message_index = 0;
volatile int message_head = -1;
uint8_t zigbeeBuffer[1];

UART_HandleTypeDef* zigbee_huart;

volatile struct BasicInfo Game;//储存比赛时间、比赛状态信息
volatile struct CarInfo CarInfo;//储存车辆信息
volatile struct ParkPosInfo ParkPos;//储存停车点位置信息
volatile struct MineInfo MineInfo;//储存金矿强度有效性信息

/***********************接口****************************/
void zigbee_Init(UART_HandleTypeDef *huart)
{
	zigbee_huart = huart;
	HAL_UART_Receive_IT(zigbee_huart, zigbeeBuffer, 1);
}
void zigbeeMessageRecord(void)
{
	zigbeeMessage[message_index] = zigbeeBuffer[0];
	message_index = receiveIndexAdd(message_index, 1);    //一个简单的索引数增加函数

	if (zigbeeMessage[receiveIndexMinus(message_index, 2)] == 0x0D
		&& zigbeeMessage[receiveIndexMinus(message_index, 1)] == 0x0A)//一串信息的结尾
	{
		if (receiveIndexMinus(message_index, message_head) == 0)
		{

			int index = message_head;
			for (int i = 0; i < ZIGBEE_MESSAGE_LENTH; i++)
			{
				zigbeeReceive[i] = zigbeeMessage[index];
				index = receiveIndexAdd(index, 1);
			}
			Decode();
		}
		else
		{
			message_head = message_index;
		}
	}
	HAL_UART_Receive_IT(zigbee_huart, zigbeeBuffer, 1);
}
uint16_t getGameTime(void) {
	return Game.Time;
}
enum GameStateEnum getGameState(void)
{
	uint8_t state = Game.GameState;
	if (state == 0)
	{
		return GameNotStart;
	}
	else if (state == 1)
	{
		return GameGoing;
	}
	else if (state == 2)
	{
		return GamePause;
	}
	else if (state == 3)
	{
		return GameOver;
	}
	return GameNotStart;
}
uint16_t getCarTask(void) {
	return (uint16_t)CarInfo.Task;
}
uint16_t getIsMineIntensityValid(int MineNo) {
	if (MineNo != 0 && MineNo != 1)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)MineInfo.IsMineIntensityValid[MineNo];
}
uint16_t getCarMineNum(void) {
	return (uint16_t)CarInfo.MineNum;
}
uint16_t getCarPosX(void) {
	return CarInfo.Pos.x;
}
uint16_t getCarPosY(void) {
	return CarInfo.Pos.y;
}
struct Position getCarPos(void) {
	return CarInfo.Pos;
}
uint32_t getMineIntensity(int MineNo) {
	if (MineNo != 0 && MineNo != 1)
		return (uint16_t)INVALID_ARG;
	else
		return CarInfo.MineIntensity[MineNo];
}
uint16_t getDistanceOfMyBeacon(int BeaconNo) {
	if (BeaconNo != 0 && BeaconNo != 1 && BeaconNo != 2)
		return (uint16_t)INVALID_ARG;
	else
		return CarInfo.DistanceOfMyBeacon[BeaconNo];
}
uint16_t getDistanceOfRivalBeacon(int BeaconNo) {
	if (BeaconNo != 0 && BeaconNo != 1 && BeaconNo != 2)
		return (uint16_t)INVALID_ARG;
	else
		return CarInfo.DistanceOfRivalBeacon[BeaconNo];
}
uint16_t getParkPosX(void) {
	return ParkPos.Pos.x;
}
uint16_t getParkPosY(void) {
	return ParkPos.Pos.y;
}
struct Position getParkPos(void) {
	return ParkPos.Pos;
}
uint16_t getCarZone(void) {
	return (uint16_t)CarInfo.Zone;
}
uint16_t getIsCarPosValid(void) {
	return (uint16_t)CarInfo.IsCarPosValid;
}
uint16_t getIsDistanceOfMyBeaconValid(int BeaconNo) {
	if (BeaconNo != 0 && BeaconNo != 1 && BeaconNo != 2)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)CarInfo.IsDistanceOfMyBeaconValid[BeaconNo];
}
uint16_t getIsDistanceOfRivalBeaconValid(int BeaconNo) {
	if (BeaconNo != 0 && BeaconNo != 1 && BeaconNo != 2)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)CarInfo.IsDistanceOfRivalBeaconValid[BeaconNo];
}
uint16_t getCarScore(void) {
	return CarInfo.Score;
}

/***************************************************/

void DecodeBasicInfo() {
	Game.Time = (zigbeeReceive[0] << 8) + zigbeeReceive[1];
	Game.GameState = (zigbeeReceive[2] & 0xC0) >> 6;
}
void DecodeCarInfo(){
	CarInfo.Task = (zigbeeReceive[2] & 0x20) >> 5;
	CarInfo.MineNum = (zigbeeReceive[2] & 0x07);
	CarInfo.Pos.x = (zigbeeReceive[3] << 8) + zigbeeReceive[4];
	CarInfo.Pos.y = (zigbeeReceive[5] << 8) + zigbeeReceive[6];
	CarInfo.IsCarPosValid = (zigbeeReceive[31] & 0x40) >> 6;
	CarInfo.Zone = (zigbeeReceive[31] & 0x80) >> 7;
	CarInfo.Score = (zigbeeReceive[32] << 8) + zigbeeReceive[33];
	for (int i = 0; i < 2; i++) {
		CarInfo.MineIntensity[i] = zigbeeReceive[7+4*i] << 24 + zigbeeReceive[8 + 4 * i] << 16 + zigbeeReceive[9 + 4 * i] << 8 + zigbeeReceive[10 + 4 * i];
	}
	for (int i = 0; i < 3; i++) {
		CarInfo.DistanceOfMyBeacon[i] = (zigbeeReceive[15+2*i] << 8) + zigbeeReceive[16+2*i];
	}
	for (int i = 0; i < 3; i++) {
		CarInfo.DistanceOfRivalBeacon[i] = (zigbeeReceive[21 + 2 * i] << 8) + zigbeeReceive[22 + 2 * i];
	}
	Carinfo.IsDistanceOfMyBeaconValid[0] = (zigbeeReceive[31] &0x20) >>5 ;
	Carinfo.IsDistanceOfMyBeaconValid[1] = (zigbeeReceive[31] &0x10) >>4 ;
	Carinfo.IsDistanceOfMyBeaconValid[2] = (zigbeeReceive[31] &0x08) >>3 ;
	Carinfo.IsDistanceOfRivalBeaconValid[0] = (zigbeeReceive[31] & 0x04) >> 2;
	Carinfo.IsDistanceOfRivalBeaconValid[1] = (zigbeeReceive[31] & 0x02) >> 1;
	Carinfo.IsDistanceOfRivalBeaconValid[2] = (zigbeeReceive[31] & 0x01);
}
void DecodeParkPosInfo() {
	ParkPos.Pos.x = (zigbeeReceive[27] << 8) + zigbeeReceive[28];
	ParkPos.Pos.y = (zigbeeReceive[29] << 8) + zigbeeReceive[30];
}
void DecodeMineInfo(){
	MineInfo.IsMineIntensityValid[0] = (zigbeeReceive[2] &0x10) >>4 ;
	MineInfo.IsMineIntensityValid[1] = (zigbeeReceive[2] &0x08) >>3 ;
}
void Decode() 
{
	DecodeBasicInfo();
	DecodeCarInfo();
	DecodeParkPosInfo();
	DecodeMineInfo();
}
int receiveIndexMinus(int index_h, int num)
{
	if (index_h - num >= 0)
	{
		return index_h - num;
	}
	else
	{
		return index_h - num + ZIGBEE_MESSAGE_LENTH;
	}
}
int receiveIndexAdd(int index_h, int num)
{
	if (index_h + num < ZIGBEE_MESSAGE_LENTH)
	{
		return index_h + num;
	}
	else
	{
		return index_h + num - ZIGBEE_MESSAGE_LENTH;
	}
}

