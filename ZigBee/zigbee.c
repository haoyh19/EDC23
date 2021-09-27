#include"zigbee.h"
volatile uint8_t zigbeeReceive[ZIGBEE_MESSAGE_LENTH];	//实时记录收到的信息
volatile uint8_t zigbeeMessage[ZIGBEE_MESSAGE_LENTH];//经过整理顺序后得到的信息
volatile int message_index = 0;
volatile int message_head = -1;
uint8_t zigbeeBuffer[1];

UART_HandleTypeDef* zigbee_huart;

volatile struct BasicInfo Game;//储存比赛时间、比赛状态信息
volatile struct CarInfo CarInfo;//储存车辆信息
volatile struct ParkDotInfo ParkDotInfo;//储存停车点能够存储的金矿种类信息
volatile struct MyBeaconInfo MyBeaconInfo;//储存己方信标充当仓库存储的金矿种类信息
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
uint16_t getMineArrayType(int MineNo) {
	if (MineNo != 0 && MineNo != 1)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)MineInfo.MineArrayType[MineNo];
}
uint16_t getParkDotMineType(int ParkDotNo) {
	if (ParkDotNo < 0 || ParkDotNo > 7)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)ParkDotInfo.ParkDotMineType[ParkDotNo];
}
uint16_t getMyBeaconMineType(int BeaconNo) {
	if (BeaconNo != 0 && BeaconNo != 1 && BeaconNo != 2)
		return (uint16_t)INVALID_ARG;
	else
		return (uint16_t)MyBeaconInfo.MyBeaconMineType[BeaconNo];
}
uint16_t getCarMineSumNum(void) {
	return (uint16_t)CarInfo.MineSumNum;
}
uint16_t getCarMineANum(void) {
	return (uint16_t)CarInfo.MineANum;
}
uint16_t getCarMineBNum(void) {
	return (uint16_t)CarInfo.MineBNum;
}
uint16_t getCarMineCNum(void) {
	return (uint16_t)CarInfo.MineCNum;
}
uint16_t getCarMineDNum(void) {
	return (uint16_t)CarInfo.MineDNum;
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
	CarInfo.MineSumNum = (zigbeeReceive[7] & 0x0F);
	CarInfo.MineANum = (zigbeeReceive[8] & 0xF0) >> 4;
	CarInfo.MineBNum = (zigbeeReceive[8] & 0x0F);
	CarInfo.MineCNum = (zigbeeReceive[9] & 0xF0) >> 4;
	CarInfo.MineDNum = (zigbeeReceive[9] & 0x0F);
	CarInfo.Pos.x = (zigbeeReceive[3] << 8) + zigbeeReceive[4];
	CarInfo.Pos.y = (zigbeeReceive[5] << 8) + zigbeeReceive[6];
	CarInfo.IsCarPosValid = (zigbeeReceive[33] & 0x40) >> 6;
	CarInfo.Zone = (zigbeeReceive[33] & 0x80) >> 7;
	CarInfo.Score = (zigbeeReceive[34] << 8) + zigbeeReceive[35];
	for (int i = 0; i < 2; i++) {
		CarInfo.MineIntensity[i] = zigbeeReceive[11+4*i] << 24 + zigbeeReceive[12 + 4 * i] << 16 + zigbeeReceive[13 + 4 * i] << 8 + zigbeeReceive[14 + 4 * i];
	}
	for (int i = 0; i < 3; i++) {
		CarInfo.DistanceOfMyBeacon[i] = (zigbeeReceive[19+2*i] << 8) + zigbeeReceive[20+2*i];
	}
	for (int i = 0; i < 3; i++) {
		CarInfo.DistanceOfRivalBeacon[i] = (zigbeeReceive[25 + 2 * i] << 8) + zigbeeReceive[26 + 2 * i];
	}
	Carinfo.IsDistanceOfMyBeaconValid[0] = (zigbeeReceive[33] &0x20) >>5 ;
	Carinfo.IsDistanceOfMyBeaconValid[1] = (zigbeeReceive[33] &0x10) >>4 ;
	Carinfo.IsDistanceOfMyBeaconValid[2] = (zigbeeReceive[33] &0x08) >>3 ;
	Carinfo.IsDistanceOfRivalBeaconValid[0] = (zigbeeReceive[33] & 0x04) >> 2;
	Carinfo.IsDistanceOfRivalBeaconValid[1] = (zigbeeReceive[33] & 0x02) >> 1;
	Carinfo.IsDistanceOfRivalBeaconValid[2] = (zigbeeReceive[33] & 0x01);
}
void DecodeParkDotInfo() {
	ParkDotInfo.ParkDotMineType[0] = (zigbeeReceive[31] & 0xC0) >> 6;
	ParkDotInfo.ParkDotMineType[1] = (zigbeeReceive[31] & 0x30) >> 4;
	ParkDotInfo.ParkDotMineType[2] = (zigbeeReceive[31] & 0x0C) >> 2;
	ParkDotInfo.ParkDotMineType[3] = zigbeeReceive[31] & 0x03;
	ParkDotInfo.ParkDotMineType[4] = (zigbeeReceive[32] & 0xC0) >> 6;
	ParkDotInfo.ParkDotMineType[5] = (zigbeeReceive[32] & 0x30) >> 4;
	ParkDotInfo.ParkDotMineType[6] = (zigbeeReceive[32] & 0x0C) >> 2;
	ParkDotInfo.ParkDotMineType[7] = zigbeeReceive[32] & 0x03;
}
void DecodeMyBeaconInfo() {
	MyBeaconInfo.MyBeaconMineType[0] = (zigbeeReceive[10] & 0xC0) >> 6;
	MyBeaconInfo.MyBeaconMineType[1] = (zigbeeReceive[10] & 0x30) >> 4;
	MyBeaconInfo.MyBeaconMineType[2] = (zigbeeReceive[10] & 0x0C) >> 2;
}
void DecodeMineInfo(){
	MineInfo.IsMineIntensityValid[0] = (zigbeeReceive[2] &0x10) >>4 ;
	MineInfo.IsMineIntensityValid[1] = (zigbeeReceive[2] &0x08) >>3 ;
	MineInfo.MineArrayType[0] = (zigbeeReceive[7] & 0xC0) >> 6;
	MineInfo.MineArrayType[1] = (zigbeeReceive[7] & 0x30) >> 4;
}

void Decode() 
{
	DecodeBasicInfo();
	DecodeCarInfo();
	DecodeParkDotInfo();
	DecodeMyBeaconInfo();
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

