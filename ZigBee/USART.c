/*

*/
/********************************
usart.c
	����Ϊ��Ҫ��usart.cĩβ�������������������USART1Ϊ����
	ʹ��ʱ��Ҫ��stm32f1xx_it.c�н��Զ���Ļص�������ӵ���Ӧ���ڵ�IRGHandler()������
**********************************/


void USER_UART_IRQHandler(UART_HandleTypeDef* huart)
{
	if (USART1 == huart->Instance)
	{
		if (RESET != __HAL_UART_GET_FLAG(&huart1, UART_FLAG_IDLE)) // ȷ���Ƿ�Ϊ�����ж�
		{
			__HAL_UART_CLEAR_IDLEFLAG(&huart1); // ��������жϱ�־
			USER_UART_IDLECallback(huart);      // �����жϻص�����
		}
	}
}

void USER_UART_IDLECallback(UART_HandleTypeDef* huart)
{
	extern int zigbeeReceiveLength;
	extern uint8_t zigbeeReceive[];
	HAL_UART_DMAStop(&huart1); //ֹͣDMA����
	uint8_t data_length = zigbeeReceiveLength - __HAL_DMA_GET_COUNTER(&hdma_usart1_rx);  //����������ݳ���
	zigbeeMessageRecord(data_length);  //��������
	memset(zigbeeReceive, 0, zigbeeReceiveLength);        //��ջ�����
	HAL_UART_Receive_DMA(&huart1, zigbeeReceive, zigbeeReceiveLength);
}
/* USER CODE END 1 */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
