namespace Win_Test
{
    partial class Form1
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
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.rtxtDisplay = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "端口号：";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(343, 10);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(67, 21);
            this.textBox4.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "调试区：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "IP地址：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(188, 21);
            this.textBox1.TabIndex = 17;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(508, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "断开连接";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "数据类型：";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(84, 54);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(36, 21);
            this.textBox3.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "命令类型：";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(224, 54);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(36, 21);
            this.textBox5.TabIndex = 27;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(401, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(42, 26);
            this.button2.TabIndex = 29;
            this.button2.Text = "发送";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "命令名：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(273, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 34;
            this.label7.Text = "命令内容：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "ROTATE_LEFT_1",
            "ROTATE_RIGHT_1",
            "ROTATE_STOP_1",
            "ELEVATE_UP_1",
            "ELEVATE_DOWN_1",
            "ELEVATE_STOP_1",
            "MOVE_FORWARD_1",
            "MOVE_BACKWARD_1",
            "MOVE_STOP_1",
            "ROTATE_LEFT_2",
            "ROTATE_RIGHT_2",
            "ROTATE_STOP_2",
            "ELEVATE_UP_2",
            "ELEVATE_DOWN_2",
            "ELEVATE_STOP_2",
            "MOVE_FORWARD_2",
            "MOVE_BACKWARD_2",
            "MOVE_STOP_2",
            "",
            "",
            "SUPPLYPOWER_ON",
            "SUPPLYPOWER_OFF",
            "CONTROLPOWER_ON",
            "CONTROLPOWER_OFF",
            "LIGHTPOWER_ON",
            "LIGHTPOWER_OFF",
            "ROTATE_LEFT",
            "ROTATE_RIGHT",
            "ROTATE_STOP",
            "ROTATE_MODE",
            "STACKING_START",
            "STACKING_STOP",
            "REVERSE_LEFT",
            "REVERSE_RIGHT",
            "REVERSE_STOP",
            "REVERSE_MODE",
            "ELEVATE_UP",
            "ELEVATE_DOWN",
            "ELEVATE_STOP",
            "ELEVATE_MODE",
            "SCRAPER_CONTROL",
            "SCRAPER_STOP_BUTTON",
            "STARTUP_ALARM",
            "FAULT_RESET",
            "EMERGENCY_STOP",
            "BYPASS_BUTTON",
            "SYSTEM_UNLOCK",
            "SYSTEM_LOCK",
            "STACKING_MODE_HM",
            "STACKING_MODE_AUTO",
            "PICKUPING_MODE_HM",
            "PICKUPING_MODE_AUTO",
            "STACKING_MID_LUB_BUTTON",
            "STACKING_MID_STOP_BUTTON",
            "STACKING_UP_LUB_BUTTON",
            "STACKING_UP_STOP_BUTTON",
            "PICKUPING_WR_LUB_BUTTON",
            "PICKUPING_WR_STOP_BUTTON",
            "PICKUPING_RO_LUB_BUTTON",
            "PICKUPING_RO_STOP_BUTTON",
            "SCRAPER_LUB_BUTTON",
            "SCRAPER_STOP_BUTTON",
            "SCRAPER_FR_LUB_BUTTON",
            "SCRAPER_FR_STOP_BUTTON",
            "PICKUPING_BE_LUB_BUTTON",
            "PICKUPING_BE_STOP_BUTTON",
            "SCREEN_TM_START_BUTTON",
            "SCREEN_TM_STOP_BUTTON",
            "PICKUPING_START_ANGLE",
            "PICKUPING_STOP_ANGLE",
            "STACKING_START_ANGLE",
            "STACKING_STOP_ANGLE",
            "STACKING_HEIGHT_SET",
            "PICKUPING_HEIGHT_SET",
            "ROTATION_ENTRY"});
            this.comboBox1.Location = new System.Drawing.Point(113, 89);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(154, 20);
            this.comboBox1.TabIndex = 35;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox2.Location = new System.Drawing.Point(343, 89);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(34, 20);
            this.comboBox2.TabIndex = 36;
            // 
            // rtxtDisplay
            // 
            this.rtxtDisplay.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxtDisplay.Location = new System.Drawing.Point(12, 113);
            this.rtxtDisplay.Name = "rtxtDisplay";
            this.rtxtDisplay.ReadOnly = true;
            this.rtxtDisplay.Size = new System.Drawing.Size(582, 420);
            this.rtxtDisplay.TabIndex = 37;
            this.rtxtDisplay.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 545);
            this.Controls.Add(this.rtxtDisplay);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "子系统通信接口测试程序";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        public System.Windows.Forms.RichTextBox rtxtDisplay;
    }
}

