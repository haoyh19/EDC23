namespace EDCHOST22
{
    partial class Tracker
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                capture.Dispose();
                coordCvt.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbCamera = new System.Windows.Forms.PictureBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.label_CarA = new System.Windows.Forms.Label();
            this.label_CarB = new System.Windows.Forms.Label();
            this.button_restart = new System.Windows.Forms.Button();
            this.button_video = new System.Windows.Forms.Button();
            this.button_set = new System.Windows.Forms.Button();
            this.labelBScore = new System.Windows.Forms.Label();
            this.labelAScore = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_BFoul = new System.Windows.Forms.Button();
            this.button_AFoul = new System.Windows.Forms.Button();
            this.label_RedBG = new System.Windows.Forms.Label();
            this.button_Continue = new System.Windows.Forms.Button();
            this.label_AFoulNum = new System.Windows.Forms.Label();
            this.label_BFoulNum = new System.Windows.Forms.Label();
            this.label_BMessage = new System.Windows.Forms.Label();
            this.label_AMessage = new System.Windows.Forms.Label();
            this.label_Debug = new System.Windows.Forms.Label();
            this.timerMsg100ms = new System.Windows.Forms.Timer(this.components);
            this.label_GameCount = new System.Windows.Forms.Label();
            this.label_GameStage = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.AWall = new System.Windows.Forms.Label();
            this.BWall = new System.Windows.Forms.Label();
            this.AFlood = new System.Windows.Forms.Label();
            this.BFlood = new System.Windows.Forms.Label();
            this.SetFlood = new System.Windows.Forms.Button();
            this.NextStage = new System.Windows.Forms.Button();
            this.label_BlueBG = new System.Windows.Forms.Label();
            this.LastStage = new System.Windows.Forms.Button();
            this.CarGetIn = new System.Windows.Forms.Button();
            this.GetOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCamera
            // 
            this.pbCamera.Location = new System.Drawing.Point(450, 200);
            this.pbCamera.Margin = new System.Windows.Forms.Padding(2);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(1020, 720);
            this.pbCamera.TabIndex = 2;
            this.pbCamera.TabStop = false;
            this.pbCamera.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbCamera_MouseClick);
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("微软雅黑 Light", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReset.Location = new System.Drawing.Point(41, 509);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(250, 60);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "重设边界点";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.buttonStart.ForeColor = System.Drawing.Color.Green;
            this.buttonStart.Location = new System.Drawing.Point(1543, 556);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(250, 60);
            this.buttonStart.TabIndex = 27;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPause.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.buttonPause.ForeColor = System.Drawing.Color.Green;
            this.buttonPause.Location = new System.Drawing.Point(1543, 746);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(250, 60);
            this.buttonPause.TabIndex = 28;
            this.buttonPause.Text = "暂停";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // label_CarA
            // 
            this.label_CarA.BackColor = System.Drawing.Color.Transparent;
            this.label_CarA.Font = new System.Drawing.Font("微软雅黑", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CarA.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label_CarA.Location = new System.Drawing.Point(300, 0);
            this.label_CarA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CarA.Name = "label_CarA";
            this.label_CarA.Size = new System.Drawing.Size(300, 100);
            this.label_CarA.TabIndex = 30;
            this.label_CarA.Text = "A车";
            this.label_CarA.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_CarB
            // 
            this.label_CarB.BackColor = System.Drawing.Color.Transparent;
            this.label_CarB.Font = new System.Drawing.Font("微软雅黑", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CarB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_CarB.Location = new System.Drawing.Point(1320, 0);
            this.label_CarB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_CarB.Name = "label_CarB";
            this.label_CarB.Size = new System.Drawing.Size(300, 100);
            this.label_CarB.TabIndex = 31;
            this.label_CarB.Text = "B车";
            // 
            // button_restart
            // 
            this.button_restart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_restart.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.button_restart.ForeColor = System.Drawing.Color.Green;
            this.button_restart.Location = new System.Drawing.Point(1543, 458);
            this.button_restart.Margin = new System.Windows.Forms.Padding(2);
            this.button_restart.Name = "button_restart";
            this.button_restart.Size = new System.Drawing.Size(250, 60);
            this.button_restart.TabIndex = 56;
            this.button_restart.Text = "新比赛";
            this.button_restart.UseVisualStyleBackColor = true;
            this.button_restart.Click += new System.EventHandler(this.button_restart_Click);
            // 
            // button_video
            // 
            this.button_video.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_video.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.button_video.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_video.Location = new System.Drawing.Point(41, 418);
            this.button_video.Margin = new System.Windows.Forms.Padding(2);
            this.button_video.Name = "button_video";
            this.button_video.Size = new System.Drawing.Size(250, 60);
            this.button_video.TabIndex = 74;
            this.button_video.Text = "开始录像";
            this.button_video.UseVisualStyleBackColor = true;
            this.button_video.Click += new System.EventHandler(this.button_video_Click);
            // 
            // button_set
            // 
            this.button_set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_set.Font = new System.Drawing.Font("微软雅黑 Light", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_set.Location = new System.Drawing.Point(41, 598);
            this.button_set.Margin = new System.Windows.Forms.Padding(2);
            this.button_set.Name = "button_set";
            this.button_set.Size = new System.Drawing.Size(250, 60);
            this.button_set.TabIndex = 77;
            this.button_set.Text = "设置";
            this.button_set.UseVisualStyleBackColor = true;
            this.button_set.Click += new System.EventHandler(this.button_set_Click);
            // 
            // labelBScore
            // 
            this.labelBScore.BackColor = System.Drawing.Color.Transparent;
            this.labelBScore.Font = new System.Drawing.Font("微软雅黑", 48F);
            this.labelBScore.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelBScore.Location = new System.Drawing.Point(1020, 0);
            this.labelBScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBScore.Name = "labelBScore";
            this.labelBScore.Size = new System.Drawing.Size(300, 100);
            this.labelBScore.TabIndex = 52;
            this.labelBScore.Text = "0";
            this.labelBScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAScore
            // 
            this.labelAScore.BackColor = System.Drawing.Color.Transparent;
            this.labelAScore.Font = new System.Drawing.Font("微软雅黑", 48F);
            this.labelAScore.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelAScore.Location = new System.Drawing.Point(600, 0);
            this.labelAScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAScore.Name = "labelAScore";
            this.labelAScore.Size = new System.Drawing.Size(300, 100);
            this.labelAScore.TabIndex = 51;
            this.labelAScore.Text = "0";
            this.labelAScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(948, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 52);
            this.label1.TabIndex = 79;
            this.label1.Text = ":";
            // 
            // button_BFoul
            // 
            this.button_BFoul.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleTurquoise;
            this.button_BFoul.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightCyan;
            this.button_BFoul.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_BFoul.Font = new System.Drawing.Font("微软雅黑 Light", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_BFoul.ForeColor = System.Drawing.Color.DodgerBlue;
            this.button_BFoul.Location = new System.Drawing.Point(1641, 339);
            this.button_BFoul.Margin = new System.Windows.Forms.Padding(2);
            this.button_BFoul.Name = "button_BFoul";
            this.button_BFoul.Size = new System.Drawing.Size(128, 58);
            this.button_BFoul.TabIndex = 65;
            this.button_BFoul.Text = "犯规";
            this.button_BFoul.UseVisualStyleBackColor = true;
            this.button_BFoul.Click += new System.EventHandler(this.button_BFoul_Click);
            // 
            // button_AFoul
            // 
            this.button_AFoul.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button_AFoul.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Pink;
            this.button_AFoul.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LavenderBlush;
            this.button_AFoul.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_AFoul.Font = new System.Drawing.Font("微软雅黑 Light", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_AFoul.ForeColor = System.Drawing.Color.Red;
            this.button_AFoul.Location = new System.Drawing.Point(97, 331);
            this.button_AFoul.Margin = new System.Windows.Forms.Padding(2);
            this.button_AFoul.Name = "button_AFoul";
            this.button_AFoul.Size = new System.Drawing.Size(128, 58);
            this.button_AFoul.TabIndex = 86;
            this.button_AFoul.Text = "犯规";
            this.button_AFoul.UseVisualStyleBackColor = true;
            this.button_AFoul.Click += new System.EventHandler(this.button_AFoul_Click);
            // 
            // label_RedBG
            // 
            this.label_RedBG.BackColor = System.Drawing.Color.Red;
            this.label_RedBG.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_RedBG.Location = new System.Drawing.Point(0, 0);
            this.label_RedBG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_RedBG.Name = "label_RedBG";
            this.label_RedBG.Size = new System.Drawing.Size(900, 100);
            this.label_RedBG.TabIndex = 88;
            // 
            // button_Continue
            // 
            this.button_Continue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Continue.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.button_Continue.ForeColor = System.Drawing.Color.Green;
            this.button_Continue.Location = new System.Drawing.Point(1543, 661);
            this.button_Continue.Margin = new System.Windows.Forms.Padding(2);
            this.button_Continue.Name = "button_Continue";
            this.button_Continue.Size = new System.Drawing.Size(250, 60);
            this.button_Continue.TabIndex = 91;
            this.button_Continue.Text = "继续";
            this.button_Continue.UseVisualStyleBackColor = true;
            this.button_Continue.Click += new System.EventHandler(this.button_Continue_Click);
            // 
            // label_AFoulNum
            // 
            this.label_AFoulNum.Font = new System.Drawing.Font("微软雅黑", 24F);
            this.label_AFoulNum.ForeColor = System.Drawing.Color.Red;
            this.label_AFoulNum.Location = new System.Drawing.Point(307, 339);
            this.label_AFoulNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_AFoulNum.Name = "label_AFoulNum";
            this.label_AFoulNum.Size = new System.Drawing.Size(78, 40);
            this.label_AFoulNum.TabIndex = 95;
            this.label_AFoulNum.Text = "0";
            this.label_AFoulNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_BFoulNum
            // 
            this.label_BFoulNum.Font = new System.Drawing.Font("微软雅黑", 24F);
            this.label_BFoulNum.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label_BFoulNum.Location = new System.Drawing.Point(1524, 339);
            this.label_BFoulNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BFoulNum.Name = "label_BFoulNum";
            this.label_BFoulNum.Size = new System.Drawing.Size(78, 58);
            this.label_BFoulNum.TabIndex = 98;
            this.label_BFoulNum.Text = "0";
            this.label_BFoulNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_BMessage
            // 
            this.label_BMessage.BackColor = System.Drawing.Color.DodgerBlue;
            this.label_BMessage.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_BMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.label_BMessage.Location = new System.Drawing.Point(1620, 0);
            this.label_BMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BMessage.Name = "label_BMessage";
            this.label_BMessage.Size = new System.Drawing.Size(300, 100);
            this.label_BMessage.TabIndex = 99;
            this.label_BMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_AMessage
            // 
            this.label_AMessage.BackColor = System.Drawing.Color.Red;
            this.label_AMessage.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_AMessage.ForeColor = System.Drawing.SystemColors.Window;
            this.label_AMessage.Location = new System.Drawing.Point(0, 0);
            this.label_AMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_AMessage.Name = "label_AMessage";
            this.label_AMessage.Size = new System.Drawing.Size(400, 100);
            this.label_AMessage.TabIndex = 100;
            this.label_AMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Debug
            // 
            this.label_Debug.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Debug.ForeColor = System.Drawing.Color.Black;
            this.label_Debug.Location = new System.Drawing.Point(35, 851);
            this.label_Debug.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Debug.Name = "label_Debug";
            this.label_Debug.Size = new System.Drawing.Size(256, 148);
            this.label_Debug.TabIndex = 102;
            // 
            // timerMsg100ms
            // 
            this.timerMsg100ms.Tick += new System.EventHandler(this.timerMsg100ms_Tick);
            // 
            // label_GameCount
            // 
            this.label_GameCount.Font = new System.Drawing.Font("宋体", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_GameCount.Location = new System.Drawing.Point(90, 110);
            this.label_GameCount.Name = "label_GameCount";
            this.label_GameCount.Size = new System.Drawing.Size(200, 60);
            this.label_GameCount.TabIndex = 105;
            this.label_GameCount.Text = "label2";
            this.label_GameCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_GameStage
            // 
            this.label_GameStage.AutoSize = true;
            this.label_GameStage.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_GameStage.Location = new System.Drawing.Point(1670, 110);
            this.label_GameStage.Name = "label_GameStage";
            this.label_GameStage.Size = new System.Drawing.Size(123, 35);
            this.label_GameStage.TabIndex = 106;
            this.label_GameStage.Text = "label2";
            // 
            // time
            // 
            this.time.Font = new System.Drawing.Font("宋体", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.time.Location = new System.Drawing.Point(665, 125);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(600, 50);
            this.time.TabIndex = 108;
            this.time.Text = "label2";
            this.time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AWall
            // 
            this.AWall.AutoSize = true;
            this.AWall.Font = new System.Drawing.Font("宋体", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AWall.Location = new System.Drawing.Point(12, 178);
            this.AWall.Name = "AWall";
            this.AWall.Size = new System.Drawing.Size(131, 37);
            this.AWall.TabIndex = 113;
            this.AWall.Text = "label2";
            // 
            // BWall
            // 
            this.BWall.AutoSize = true;
            this.BWall.Font = new System.Drawing.Font("宋体", 27.75F);
            this.BWall.Location = new System.Drawing.Point(1489, 178);
            this.BWall.Name = "BWall";
            this.BWall.Size = new System.Drawing.Size(131, 37);
            this.BWall.TabIndex = 114;
            this.BWall.Text = "label3";
            // 
            // AFlood
            // 
            this.AFlood.AutoSize = true;
            this.AFlood.Font = new System.Drawing.Font("宋体", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AFlood.Location = new System.Drawing.Point(12, 247);
            this.AFlood.Name = "AFlood";
            this.AFlood.Size = new System.Drawing.Size(131, 37);
            this.AFlood.TabIndex = 115;
            this.AFlood.Text = "label2";
            // 
            // BFlood
            // 
            this.BFlood.AutoSize = true;
            this.BFlood.Font = new System.Drawing.Font("宋体", 27.75F);
            this.BFlood.Location = new System.Drawing.Point(1492, 238);
            this.BFlood.Name = "BFlood";
            this.BFlood.Size = new System.Drawing.Size(131, 37);
            this.BFlood.TabIndex = 116;
            this.BFlood.Text = "label2";
            // 
            // SetFlood
            // 
            this.SetFlood.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetFlood.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.SetFlood.ForeColor = System.Drawing.Color.Green;
            this.SetFlood.Location = new System.Drawing.Point(1543, 851);
            this.SetFlood.Margin = new System.Windows.Forms.Padding(2);
            this.SetFlood.Name = "SetFlood";
            this.SetFlood.Size = new System.Drawing.Size(250, 60);
            this.SetFlood.TabIndex = 117;
            this.SetFlood.Text = "设置隔离区";
            this.SetFlood.UseVisualStyleBackColor = true;
            this.SetFlood.Click += new System.EventHandler(this.SetFlood_Click);
            // 
            // NextStage
            // 
            this.NextStage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextStage.Font = new System.Drawing.Font("微软雅黑 Light", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NextStage.Location = new System.Drawing.Point(41, 685);
            this.NextStage.Margin = new System.Windows.Forms.Padding(2);
            this.NextStage.Name = "NextStage";
            this.NextStage.Size = new System.Drawing.Size(250, 60);
            this.NextStage.TabIndex = 118;
            this.NextStage.Text = "下一个阶段";
            this.NextStage.UseVisualStyleBackColor = true;
            this.NextStage.Click += new System.EventHandler(this.NextStage_Click);
            // 
            // label_BlueBG
            // 
            this.label_BlueBG.BackColor = System.Drawing.Color.DodgerBlue;
            this.label_BlueBG.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label_BlueBG.Location = new System.Drawing.Point(1020, 0);
            this.label_BlueBG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_BlueBG.Name = "label_BlueBG";
            this.label_BlueBG.Size = new System.Drawing.Size(900, 100);
            this.label_BlueBG.TabIndex = 89;
            // 
            // LastStage
            // 
            this.LastStage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LastStage.Font = new System.Drawing.Font("微软雅黑 Light", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LastStage.Location = new System.Drawing.Point(41, 776);
            this.LastStage.Margin = new System.Windows.Forms.Padding(2);
            this.LastStage.Name = "LastStage";
            this.LastStage.Size = new System.Drawing.Size(250, 60);
            this.LastStage.TabIndex = 119;
            this.LastStage.Text = "上一个阶段";
            this.LastStage.UseVisualStyleBackColor = true;
            this.LastStage.Click += new System.EventHandler(this.LastStage_Click);
            // 
            // CarGetIn
            // 
            this.CarGetIn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CarGetIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CarGetIn.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.CarGetIn.ForeColor = System.Drawing.Color.Green;
            this.CarGetIn.Location = new System.Drawing.Point(450, 119);
            this.CarGetIn.Margin = new System.Windows.Forms.Padding(2);
            this.CarGetIn.Name = "CarGetIn";
            this.CarGetIn.Size = new System.Drawing.Size(250, 60);
            this.CarGetIn.TabIndex = 120;
            this.CarGetIn.Text = "进迷宫加分";
            this.CarGetIn.UseVisualStyleBackColor = false;
            this.CarGetIn.Click += new System.EventHandler(this.CarGetIn_Click);
            // 
            // GetOut
            // 
            this.GetOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetOut.Font = new System.Drawing.Font("微软雅黑 Light", 28F);
            this.GetOut.ForeColor = System.Drawing.Color.Green;
            this.GetOut.Location = new System.Drawing.Point(1220, 115);
            this.GetOut.Margin = new System.Windows.Forms.Padding(2);
            this.GetOut.Name = "GetOut";
            this.GetOut.Size = new System.Drawing.Size(250, 60);
            this.GetOut.TabIndex = 121;
            this.GetOut.Text = "回起点加分";
            this.GetOut.UseVisualStyleBackColor = true;
            this.GetOut.Click += new System.EventHandler(this.GetOut_Click);
            // 
            // Tracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.GetOut);
            this.Controls.Add(this.CarGetIn);
            this.Controls.Add(this.LastStage);
            this.Controls.Add(this.NextStage);
            this.Controls.Add(this.SetFlood);
            this.Controls.Add(this.BFlood);
            this.Controls.Add(this.AFlood);
            this.Controls.Add(this.BWall);
            this.Controls.Add(this.AWall);
            this.Controls.Add(this.time);
            this.Controls.Add(this.label_GameStage);
            this.Controls.Add(this.label_GameCount);
            this.Controls.Add(this.label_Debug);
            this.Controls.Add(this.label_AMessage);
            this.Controls.Add(this.label_BMessage);
            this.Controls.Add(this.label_BFoulNum);
            this.Controls.Add(this.label_AFoulNum);
            this.Controls.Add(this.button_Continue);
            this.Controls.Add(this.label_BlueBG);
            this.Controls.Add(this.label_RedBG);
            this.Controls.Add(this.button_AFoul);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_set);
            this.Controls.Add(this.button_video);
            this.Controls.Add(this.button_BFoul);
            this.Controls.Add(this.button_restart);
            this.Controls.Add(this.labelBScore);
            this.Controls.Add(this.labelAScore);
            this.Controls.Add(this.label_CarB);
            this.Controls.Add(this.label_CarA);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.pbCamera);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Tracker";
            this.Text = "EDC22HOST";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Tracker_FormClosed);
            this.Load += new System.EventHandler(this.Tracker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbCamera;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Label label_CarA;
        private System.Windows.Forms.Label label_CarB;
        private System.Windows.Forms.Button button_restart;
        private System.Windows.Forms.Button button_video;
        private System.Windows.Forms.Button button_set;
        private System.Windows.Forms.Label labelBScore;
        private System.Windows.Forms.Label labelAScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_BFoul;
        private System.Windows.Forms.Button button_AFoul;
        private System.Windows.Forms.Label label_RedBG;
        private System.Windows.Forms.Button button_Continue;
        private System.Windows.Forms.Label label_AFoulNum;
        private System.Windows.Forms.Label label_BFoulNum;
        private System.Windows.Forms.Label label_BMessage;
        private System.Windows.Forms.Label label_AMessage;
        private System.Windows.Forms.Label label_Debug;
        private System.Windows.Forms.Timer timerMsg100ms;
        private System.Windows.Forms.Label label_GameCount;
        private System.Windows.Forms.Label label_GameStage;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Label AWall;
        private System.Windows.Forms.Label BWall;
        private System.Windows.Forms.Label AFlood;
        private System.Windows.Forms.Label BFlood;
        private System.Windows.Forms.Button SetFlood;
        private System.Windows.Forms.Button NextStage;
        private System.Windows.Forms.Label label_BlueBG;
        private System.Windows.Forms.Button LastStage;
        private System.Windows.Forms.Button CarGetIn;
        private System.Windows.Forms.Button GetOut;
    }
}

