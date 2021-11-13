##### zigbee使用说明



此处仅为使用说明，不做原理解释。具体的原理解释请参考第六讲课程。

1. 在CubeMx中打开USART1_Rx的DMA中断（用于zigbee接收）,并开启USART1的全局中断。

2. 将zigbee.c放到Src文件夹中（这里面应该还能看到main.c），把zigbee.h放到Inc文件夹中（这里面有main.h）

3. 打开刚刚新建工程的makefile文件，找到“*# C sources*”，并且在最后添加zigbee.c的相对路径（如"  \ Src/zigbee.c  " ）

4. 在工程文件usart.c 末尾添加USART.c中的两个函数，并在工程文件stm32f1xx_it.c中找到以下函数，并添加一段代码

   ```C
   void USART1_IRQHandler(void)
   {
     /* USER CODE BEGIN USART1_IRQn 0 */
   
     /* USER CODE END USART1_IRQn 0 */
     HAL_UART_IRQHandler(&huart1);
     /* USER CODE BEGIN USART1_IRQn 1 */
     USER_UART_IDLECallback(&huart1);    //需要添加的代码
     /* USER CODE END USART1_IRQn 1 */
   }
   ```

5. 在main函数的USER CODE BEGIN 2 中添加 zigbee_Init(&huart1);

6. 以上已完成必要的配置，具体的接口调用可参见zigbee.h中的注释。

   

注：以上教程可能缺少一些include，选手可根据编译提示自行解决。









