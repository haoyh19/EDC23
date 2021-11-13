/*

*/
/********************************
usart.c
	以下为需要在usart.c末尾定义的两个函数（均以USART1为例）
	使用时需要在stm32f1xx_it.c中将自定义的回调函数添加到对应串口的IRGHandler()函数中
**********************************/


void USER_UART_IRQHandler(UART_HandleTypeDef* huart)
{
	if (USART1 == huart->Instance)
	{
		if (RESET != __HAL_UART_GET_FLAG(&huart1, UART_FLAG_IDLE)) // 确认是否为空闲中断
		{
			__HAL_UART_CLEAR_IDLEFLAG(&huart1); // 清除空闲中断标志
			USER_UART_IDLECallback(huart);      // 调用中断回调函数
		}
	}
}

void USER_UART_IDLECallback(UART_HandleTypeDef* huart)
{
	extern int zigbeeReceiveLength;
	extern uint8_t zigbeeReceive[];
	HAL_UART_DMAStop(&huart1); //停止DMA接收
	uint8_t data_length = zigbeeReceiveLength - __HAL_DMA_GET_COUNTER(&hdma_usart1_rx);  //计算接收数据长度
	zigbeeMessageRecord(data_length);  //处理数据
	memset(zigbeeReceive, 0, zigbeeReceiveLength);        //清空缓冲区
	HAL_UART_Receive_DMA(&huart1, zigbeeReceive, zigbeeReceiveLength);
}
/* USER CODE END 1 */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
