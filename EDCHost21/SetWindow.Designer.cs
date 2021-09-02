namespace EDCHOST22
{
    partial class SetWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_DebugMode = new System.Windows.Forms.CheckBox();
            this.nudAreaL = new System.Windows.Forms.NumericUpDown();
            this.lblAreaL = new System.Windows.Forms.Label();
            this.nudValueL = new System.Windows.Forms.NumericUpDown();
            this.lblValueL = new System.Windows.Forms.Label();
            this.nudSat2L = new System.Windows.Forms.NumericUpDown();
            this.lblSat2L = new System.Windows.Forms.Label();
            this.nudSat1L = new System.Windows.Forms.NumericUpDown();
            this.lblSat1L = new System.Windows.Forms.Label();
            this.nudHue2H = new System.Windows.Forms.NumericUpDown();
            this.nudHue2L = new System.Windows.Forms.NumericUpDown();
            this.lblHue2L = new System.Windows.Forms.Label();
            this.nudHue1H = new System.Windows.Forms.NumericUpDown();
            this.lblHue1H = new System.Windows.Forms.Label();
            this.nudHue1L = new System.Windows.Forms.NumericUpDown();
            this.lblHue1L = new System.Windows.Forms.Label();
            this.button_ConfigSave = new System.Windows.Forms.Button();
            this.button_ConfigLoad = new System.Windows.Forms.Button();
            this.cbPorts1 = new System.Windows.Forms.ComboBox();
            this.lblPort1 = new System.Windows.Forms.Label();
            this.lblCapture = new System.Windows.Forms.Label();
            this.nudCapture = new System.Windows.Forms.NumericUpDown();
            this.checkBox_ShowMask = new System.Windows.Forms.CheckBox();
            this.nudBaudRate = new System.Windows.Forms.NumericUpDown();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.lblPort2 = new System.Windows.Forms.Label();
            this.cbPorts2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbLaby = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValueL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSat2L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSat1L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue2H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue2L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue1H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue1L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_DebugMode
            // 
            this.checkBox_DebugMode.AutoSize = true;
            this.checkBox_DebugMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_DebugMode.Location = new System.Drawing.Point(613, 672);
            this.checkBox_DebugMode.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox_DebugMode.Name = "checkBox_DebugMode";
            this.checkBox_DebugMode.Size = new System.Drawing.Size(175, 43);
            this.checkBox_DebugMode.TabIndex = 74;
            this.checkBox_DebugMode.Text = "调试模式";
            this.checkBox_DebugMode.UseVisualStyleBackColor = true;
            this.checkBox_DebugMode.CheckedChanged += new System.EventHandler(this.checkBox_DebugMode_CheckedChanged);
            // 
            // nudAreaL
            // 
            this.nudAreaL.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudAreaL.Location = new System.Drawing.Point(1116, 116);
            this.nudAreaL.Margin = new System.Windows.Forms.Padding(6);
            this.nudAreaL.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudAreaL.Name = "nudAreaL";
            this.nudAreaL.Size = new System.Drawing.Size(129, 47);
            this.nudAreaL.TabIndex = 90;
            this.nudAreaL.ValueChanged += new System.EventHandler(this.nudAreaL_ValueChanged);
            // 
            // lblAreaL
            // 
            this.lblAreaL.AutoSize = true;
            this.lblAreaL.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAreaL.Location = new System.Drawing.Point(911, 120);
            this.lblAreaL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAreaL.Name = "lblAreaL";
            this.lblAreaL.Size = new System.Drawing.Size(204, 39);
            this.lblAreaL.TabIndex = 89;
            this.lblAreaL.Text = "噪点识别阈值:";
            // 
            // nudValueL
            // 
            this.nudValueL.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudValueL.Location = new System.Drawing.Point(726, 116);
            this.nudValueL.Margin = new System.Windows.Forms.Padding(6);
            this.nudValueL.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudValueL.Name = "nudValueL";
            this.nudValueL.Size = new System.Drawing.Size(141, 47);
            this.nudValueL.TabIndex = 88;
            this.nudValueL.ValueChanged += new System.EventHandler(this.nudValueL_ValueChanged);
            // 
            // lblValueL
            // 
            this.lblValueL.AutoSize = true;
            this.lblValueL.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblValueL.Location = new System.Drawing.Point(604, 120);
            this.lblValueL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblValueL.Name = "lblValueL";
            this.lblValueL.Size = new System.Drawing.Size(119, 39);
            this.lblValueL.TabIndex = 87;
            this.lblValueL.Text = "ValueL:";
            // 
            // nudSat2L
            // 
            this.nudSat2L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudSat2L.Location = new System.Drawing.Point(156, 495);
            this.nudSat2L.Margin = new System.Windows.Forms.Padding(6);
            this.nudSat2L.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSat2L.Name = "nudSat2L";
            this.nudSat2L.Size = new System.Drawing.Size(141, 47);
            this.nudSat2L.TabIndex = 86;
            this.nudSat2L.ValueChanged += new System.EventHandler(this.nudSat2L_ValueChanged);
            // 
            // lblSat2L
            // 
            this.lblSat2L.AutoSize = true;
            this.lblSat2L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSat2L.Location = new System.Drawing.Point(68, 495);
            this.lblSat2L.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSat2L.Name = "lblSat2L";
            this.lblSat2L.Size = new System.Drawing.Size(69, 39);
            this.lblSat2L.TabIndex = 85;
            this.lblSat2L.Text = "Sat:";
            // 
            // nudSat1L
            // 
            this.nudSat1L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudSat1L.Location = new System.Drawing.Point(156, 264);
            this.nudSat1L.Margin = new System.Windows.Forms.Padding(6);
            this.nudSat1L.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSat1L.Name = "nudSat1L";
            this.nudSat1L.Size = new System.Drawing.Size(141, 47);
            this.nudSat1L.TabIndex = 84;
            this.nudSat1L.ValueChanged += new System.EventHandler(this.nudSat1L_ValueChanged);
            // 
            // lblSat1L
            // 
            this.lblSat1L.AutoSize = true;
            this.lblSat1L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSat1L.Location = new System.Drawing.Point(66, 264);
            this.lblSat1L.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSat1L.Name = "lblSat1L";
            this.lblSat1L.Size = new System.Drawing.Size(69, 39);
            this.lblSat1L.TabIndex = 83;
            this.lblSat1L.Text = "Sat:";
            // 
            // nudHue2H
            // 
            this.nudHue2H.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudHue2H.Location = new System.Drawing.Point(382, 407);
            this.nudHue2H.Margin = new System.Windows.Forms.Padding(6);
            this.nudHue2H.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudHue2H.Name = "nudHue2H";
            this.nudHue2H.Size = new System.Drawing.Size(141, 47);
            this.nudHue2H.TabIndex = 82;
            this.nudHue2H.ValueChanged += new System.EventHandler(this.nudHue2H_ValueChanged);
            // 
            // nudHue2L
            // 
            this.nudHue2L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudHue2L.Location = new System.Drawing.Point(156, 407);
            this.nudHue2L.Margin = new System.Windows.Forms.Padding(6);
            this.nudHue2L.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudHue2L.Name = "nudHue2L";
            this.nudHue2L.Size = new System.Drawing.Size(141, 47);
            this.nudHue2L.TabIndex = 80;
            this.nudHue2L.ValueChanged += new System.EventHandler(this.nudHue2L_ValueChanged);
            // 
            // lblHue2L
            // 
            this.lblHue2L.AutoSize = true;
            this.lblHue2L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHue2L.Location = new System.Drawing.Point(68, 411);
            this.lblHue2L.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHue2L.Name = "lblHue2L";
            this.lblHue2L.Size = new System.Drawing.Size(82, 39);
            this.lblHue2L.TabIndex = 79;
            this.lblHue2L.Text = "Hue:";
            // 
            // nudHue1H
            // 
            this.nudHue1H.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudHue1H.Location = new System.Drawing.Point(380, 184);
            this.nudHue1H.Margin = new System.Windows.Forms.Padding(6);
            this.nudHue1H.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudHue1H.Name = "nudHue1H";
            this.nudHue1H.Size = new System.Drawing.Size(141, 47);
            this.nudHue1H.TabIndex = 78;
            this.nudHue1H.ValueChanged += new System.EventHandler(this.nudHue1H_ValueChanged);
            // 
            // lblHue1H
            // 
            this.lblHue1H.AutoSize = true;
            this.lblHue1H.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHue1H.Location = new System.Drawing.Point(64, 184);
            this.lblHue1H.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHue1H.Name = "lblHue1H";
            this.lblHue1H.Size = new System.Drawing.Size(82, 39);
            this.lblHue1H.TabIndex = 77;
            this.lblHue1H.Text = "Hue:";
            // 
            // nudHue1L
            // 
            this.nudHue1L.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudHue1L.Location = new System.Drawing.Point(156, 184);
            this.nudHue1L.Margin = new System.Windows.Forms.Padding(6);
            this.nudHue1L.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudHue1L.Name = "nudHue1L";
            this.nudHue1L.Size = new System.Drawing.Size(141, 47);
            this.nudHue1L.TabIndex = 76;
            this.nudHue1L.ValueChanged += new System.EventHandler(this.nudHue1L_ValueChanged);
            // 
            // lblHue1L
            // 
            this.lblHue1L.AutoSize = true;
            this.lblHue1L.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHue1L.Location = new System.Drawing.Point(55, 116);
            this.lblHue1L.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHue1L.Name = "lblHue1L";
            this.lblHue1L.Size = new System.Drawing.Size(103, 35);
            this.lblHue1L.TabIndex = 75;
            this.lblHue1L.Text = "小车1";
            // 
            // button_ConfigSave
            // 
            this.button_ConfigSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ConfigSave.Location = new System.Drawing.Point(1001, 557);
            this.button_ConfigSave.Margin = new System.Windows.Forms.Padding(6);
            this.button_ConfigSave.Name = "button_ConfigSave";
            this.button_ConfigSave.Size = new System.Drawing.Size(244, 70);
            this.button_ConfigSave.TabIndex = 91;
            this.button_ConfigSave.Text = "保存";
            this.button_ConfigSave.UseVisualStyleBackColor = true;
            this.button_ConfigSave.Click += new System.EventHandler(this.button_ConfigSave_Click);
            // 
            // button_ConfigLoad
            // 
            this.button_ConfigLoad.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_ConfigLoad.Location = new System.Drawing.Point(1001, 667);
            this.button_ConfigLoad.Margin = new System.Windows.Forms.Padding(6);
            this.button_ConfigLoad.Name = "button_ConfigLoad";
            this.button_ConfigLoad.Size = new System.Drawing.Size(244, 70);
            this.button_ConfigLoad.TabIndex = 92;
            this.button_ConfigLoad.Text = "读取";
            this.button_ConfigLoad.UseVisualStyleBackColor = true;
            this.button_ConfigLoad.Click += new System.EventHandler(this.button_ConfigLoad_Click);
            // 
            // cbPorts1
            // 
            this.cbPorts1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cbPorts1.FormattingEnabled = true;
            this.cbPorts1.Location = new System.Drawing.Point(726, 296);
            this.cbPorts1.Margin = new System.Windows.Forms.Padding(6);
            this.cbPorts1.Name = "cbPorts1";
            this.cbPorts1.Size = new System.Drawing.Size(141, 47);
            this.cbPorts1.TabIndex = 99;
            this.cbPorts1.SelectedIndexChanged += new System.EventHandler(this.cbPorts1_SelectedIndexChanged);
            this.cbPorts1.TextChanged += new System.EventHandler(this.cbPorts1_TextChanged);
            // 
            // lblPort1
            // 
            this.lblPort1.AutoSize = true;
            this.lblPort1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPort1.Location = new System.Drawing.Point(606, 296);
            this.lblPort1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPort1.Name = "lblPort1";
            this.lblPort1.Size = new System.Drawing.Size(101, 39);
            this.lblPort1.TabIndex = 100;
            this.lblPort1.Text = "Port1:";
            this.lblPort1.Click += new System.EventHandler(this.lblPort1_Click);
            // 
            // lblCapture
            // 
            this.lblCapture.AutoSize = true;
            this.lblCapture.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCapture.Location = new System.Drawing.Point(606, 473);
            this.lblCapture.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblCapture.Name = "lblCapture";
            this.lblCapture.Size = new System.Drawing.Size(197, 39);
            this.lblCapture.TabIndex = 102;
            this.lblCapture.Text = "摄像头选择：";
            // 
            // nudCapture
            // 
            this.nudCapture.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudCapture.Location = new System.Drawing.Point(799, 469);
            this.nudCapture.Margin = new System.Windows.Forms.Padding(6);
            this.nudCapture.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudCapture.Name = "nudCapture";
            this.nudCapture.Size = new System.Drawing.Size(120, 47);
            this.nudCapture.TabIndex = 103;
            this.nudCapture.ValueChanged += new System.EventHandler(this.nudCapture_ValueChanged);
            // 
            // checkBox_ShowMask
            // 
            this.checkBox_ShowMask.AutoSize = true;
            this.checkBox_ShowMask.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_ShowMask.Location = new System.Drawing.Point(613, 572);
            this.checkBox_ShowMask.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox_ShowMask.Name = "checkBox_ShowMask";
            this.checkBox_ShowMask.Size = new System.Drawing.Size(235, 43);
            this.checkBox_ShowMask.TabIndex = 104;
            this.checkBox_ShowMask.Text = "显示识别蒙版";
            this.checkBox_ShowMask.UseVisualStyleBackColor = true;
            this.checkBox_ShowMask.CheckedChanged += new System.EventHandler(this.checkBox_ShowMask_CheckedChanged);
            // 
            // nudBaudRate
            // 
            this.nudBaudRate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudBaudRate.Location = new System.Drawing.Point(1043, 329);
            this.nudBaudRate.Margin = new System.Windows.Forms.Padding(6);
            this.nudBaudRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudBaudRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBaudRate.Name = "nudBaudRate";
            this.nudBaudRate.Size = new System.Drawing.Size(202, 47);
            this.nudBaudRate.TabIndex = 106;
            this.nudBaudRate.Value = new decimal(new int[] {
            115200,
            0,
            0,
            0});
            this.nudBaudRate.ValueChanged += new System.EventHandler(this.nudBaudRate_ValueChanged);
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBaudRate.Location = new System.Drawing.Point(911, 329);
            this.lblBaudRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(137, 39);
            this.lblBaudRate.TabIndex = 105;
            this.lblBaudRate.Text = "波特率：";
            // 
            // lblPort2
            // 
            this.lblPort2.AutoSize = true;
            this.lblPort2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPort2.Location = new System.Drawing.Point(604, 368);
            this.lblPort2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPort2.Name = "lblPort2";
            this.lblPort2.Size = new System.Drawing.Size(101, 39);
            this.lblPort2.TabIndex = 108;
            this.lblPort2.Text = "Port2:";
            this.lblPort2.Click += new System.EventHandler(this.lblPort2_Click);
            // 
            // cbPorts2
            // 
            this.cbPorts2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cbPorts2.FormattingEnabled = true;
            this.cbPorts2.Location = new System.Drawing.Point(724, 368);
            this.cbPorts2.Margin = new System.Windows.Forms.Padding(6);
            this.cbPorts2.Name = "cbPorts2";
            this.cbPorts2.Size = new System.Drawing.Size(143, 47);
            this.cbPorts2.TabIndex = 107;
            this.cbPorts2.SelectedIndexChanged += new System.EventHandler(this.cbPorts2_SelectedIndexChanged);
            this.cbPorts2.TextChanged += new System.EventHandler(this.cbPorts2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(316, 208);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 47);
            this.label1.TabIndex = 109;
            this.label1.Text = "~";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(316, 284);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 47);
            this.label2.TabIndex = 110;
            this.label2.Text = "~";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(55, 344);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 35);
            this.label3.TabIndex = 111;
            this.label3.Text = "小车2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(318, 429);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 47);
            this.label4.TabIndex = 112;
            this.label4.Text = "~";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(372, 266);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 47);
            this.label5.TabIndex = 113;
            this.label5.Text = "255";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(374, 501);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 47);
            this.label6.TabIndex = 114;
            this.label6.Text = "255";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(318, 519);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 47);
            this.label7.TabIndex = 115;
            this.label7.Text = "~";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(607, 48);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(177, 40);
            this.label8.TabIndex = 116;
            this.label8.Text = "识别变量";
            // 
            // cbLaby
            // 
            this.cbLaby.FormattingEnabled = true;
            this.cbLaby.Location = new System.Drawing.Point(285, 633);
            this.cbLaby.Name = "cbLaby";
            this.cbLaby.Size = new System.Drawing.Size(236, 38);
            this.cbLaby.TabIndex = 117;
            this.cbLaby.SelectedIndexChanged += new System.EventHandler(this.cbLaby_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(607, 220);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(177, 40);
            this.label9.TabIndex = 118;
            this.label9.Text = "通信选项";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(44, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(177, 40);
            this.label10.TabIndex = 119;
            this.label10.Text = "颜色设置";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(66, 631);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(197, 39);
            this.label11.TabIndex = 120;
            this.label11.Text = "障碍物样式：";
            // 
            // SetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 819);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbLaby);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPort2);
            this.Controls.Add(this.cbPorts2);
            this.Controls.Add(this.nudBaudRate);
            this.Controls.Add(this.lblBaudRate);
            this.Controls.Add(this.checkBox_ShowMask);
            this.Controls.Add(this.nudCapture);
            this.Controls.Add(this.lblCapture);
            this.Controls.Add(this.lblPort1);
            this.Controls.Add(this.cbPorts1);
            this.Controls.Add(this.button_ConfigLoad);
            this.Controls.Add(this.button_ConfigSave);
            this.Controls.Add(this.nudAreaL);
            this.Controls.Add(this.lblAreaL);
            this.Controls.Add(this.nudValueL);
            this.Controls.Add(this.lblValueL);
            this.Controls.Add(this.nudSat2L);
            this.Controls.Add(this.lblSat2L);
            this.Controls.Add(this.nudSat1L);
            this.Controls.Add(this.lblSat1L);
            this.Controls.Add(this.nudHue2H);
            this.Controls.Add(this.nudHue2L);
            this.Controls.Add(this.lblHue2L);
            this.Controls.Add(this.nudHue1H);
            this.Controls.Add(this.lblHue1H);
            this.Controls.Add(this.nudHue1L);
            this.Controls.Add(this.lblHue1L);
            this.Controls.Add(this.checkBox_DebugMode);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SetWindow";
            this.Text = "设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetWindow_FormClosing);
            this.Load += new System.EventHandler(this.SetWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValueL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSat2L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSat1L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue2H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue2L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue1H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHue1L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_DebugMode;
        private System.Windows.Forms.NumericUpDown nudAreaL;
        private System.Windows.Forms.Label lblAreaL;
        private System.Windows.Forms.NumericUpDown nudValueL;
        private System.Windows.Forms.Label lblValueL;
        private System.Windows.Forms.NumericUpDown nudSat2L;
        private System.Windows.Forms.Label lblSat2L;
        private System.Windows.Forms.NumericUpDown nudSat1L;
        private System.Windows.Forms.Label lblSat1L;
        private System.Windows.Forms.NumericUpDown nudHue2H;
        private System.Windows.Forms.NumericUpDown nudHue2L;
        private System.Windows.Forms.Label lblHue2L;
        private System.Windows.Forms.NumericUpDown nudHue1H;
        private System.Windows.Forms.Label lblHue1H;
        private System.Windows.Forms.NumericUpDown nudHue1L;
        private System.Windows.Forms.Label lblHue1L;
        private System.Windows.Forms.Button button_ConfigSave;
        private System.Windows.Forms.Button button_ConfigLoad;
        private System.Windows.Forms.ComboBox cbPorts1;
        private System.Windows.Forms.Label lblPort1;
        private System.Windows.Forms.Label lblCapture;
        private System.Windows.Forms.NumericUpDown nudCapture;
        private System.Windows.Forms.CheckBox checkBox_ShowMask;
        private System.Windows.Forms.NumericUpDown nudBaudRate;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.Label lblPort2;
        private System.Windows.Forms.ComboBox cbPorts2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbLaby;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}