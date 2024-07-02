namespace ShenYangRemoteSystem.用户控件
{
    partial class UserControl_Page5
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label7 = new System.Windows.Forms.Label();
            this.txtValue2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboDataType2 = new System.Windows.Forms.ComboBox();
            this.txtStartAddress2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtxtDisplay = new System.Windows.Forms.RichTextBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cboDataType = new System.Windows.Forms.ComboBox();
            this.txtStartAddress = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MySqlConnectionLabel = new System.Windows.Forms.Label();
            this.PlcConnectionLabel = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(297, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 19);
            this.label7.TabIndex = 41;
            this.label7.Text = "数据类型";
            // 
            // txtValue2
            // 
            this.txtValue2.Location = new System.Drawing.Point(375, 250);
            this.txtValue2.Name = "txtValue2";
            this.txtValue2.Size = new System.Drawing.Size(100, 21);
            this.txtValue2.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(292, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 19);
            this.label8.TabIndex = 39;
            this.label8.Text = "写入数值：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(250, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 19);
            this.label9.TabIndex = 38;
            this.label9.Text = "起始地址（整型）";
            // 
            // cboDataType2
            // 
            this.cboDataType2.FormattingEnabled = true;
            this.cboDataType2.Items.AddRange(new object[] {
            "Byte(Byte)",
            "Int16(Int)",
            "UInt16(Word)",
            "Int32(DInt)",
            "UInt32(DWord)",
            "Float(Real)",
            "Int64",
            "UInt64",
            "Double"});
            this.cboDataType2.Location = new System.Drawing.Point(375, 213);
            this.cboDataType2.Name = "cboDataType2";
            this.cboDataType2.Size = new System.Drawing.Size(121, 20);
            this.cboDataType2.TabIndex = 37;
            // 
            // txtStartAddress2
            // 
            this.txtStartAddress2.Location = new System.Drawing.Point(375, 175);
            this.txtStartAddress2.Name = "txtStartAddress2";
            this.txtStartAddress2.Size = new System.Drawing.Size(100, 21);
            this.txtStartAddress2.TabIndex = 36;
            this.txtStartAddress2.Text = "1000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(507, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 19);
            this.label6.TabIndex = 35;
            this.label6.Text = "调试报告";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(57, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 19);
            this.label5.TabIndex = 34;
            this.label5.Text = "数据类型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(81, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 19);
            this.label4.TabIndex = 33;
            this.label4.Text = "端口号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(81, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 19);
            this.label3.TabIndex = 32;
            this.label3.Text = "IP地址";
            // 
            // rtxtDisplay
            // 
            this.rtxtDisplay.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxtDisplay.Location = new System.Drawing.Point(509, 38);
            this.rtxtDisplay.Name = "rtxtDisplay";
            this.rtxtDisplay.ReadOnly = true;
            this.rtxtDisplay.Size = new System.Drawing.Size(716, 374);
            this.rtxtDisplay.TabIndex = 31;
            this.rtxtDisplay.Text = "";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(337, 298);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 30;
            this.btnWrite.Text = "单个写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // txtValue
            // 
            this.txtValue.Enabled = false;
            this.txtValue.Location = new System.Drawing.Point(137, 357);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(100, 21);
            this.txtValue.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(67, 358);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 19);
            this.label2.TabIndex = 28;
            this.label2.Text = "读取结果";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.SystemColors.Control;
            this.label10.Location = new System.Drawing.Point(11, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 19);
            this.label10.TabIndex = 27;
            this.label10.Text = "起始地址（整型）";
            // 
            // cboDataType
            // 
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Items.AddRange(new object[] {
            "Byte(Byte)",
            "Int16(Int)",
            "UInt16(Word)",
            "Int32(DInt)",
            "UInt32(DWord)",
            "Float(Real)",
            "Int64",
            "UInt64",
            "Double"});
            this.cboDataType.Location = new System.Drawing.Point(137, 213);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(121, 20);
            this.cboDataType.TabIndex = 26;
            this.cboDataType.Text = "UInt16(Word)";
            // 
            // txtStartAddress
            // 
            this.txtStartAddress.Location = new System.Drawing.Point(137, 175);
            this.txtStartAddress.Name = "txtStartAddress";
            this.txtStartAddress.Size = new System.Drawing.Size(100, 21);
            this.txtStartAddress.TabIndex = 25;
            this.txtStartAddress.Text = "1000";
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(91, 298);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 24;
            this.btnRead.Text = "单个读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(137, 91);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 21);
            this.txtPort.TabIndex = 23;
            this.txtPort.Text = "502";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(137, 51);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 22;
            this.txtIP.Text = "127.0.0.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(337, 66);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 21;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(175)))), ((int)(((byte)(247)))));
            this.label31.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label31.Location = new System.Drawing.Point(23, 16);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(135, 28);
            this.label31.TabIndex = 359;
            this.label31.Text = "PLC手动调试";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.btnConnect);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtIP);
            this.panel2.Controls.Add(this.txtValue2);
            this.panel2.Controls.Add(this.txtPort);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.btnRead);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.txtStartAddress);
            this.panel2.Controls.Add(this.cboDataType2);
            this.panel2.Controls.Add(this.cboDataType);
            this.panel2.Controls.Add(this.txtStartAddress2);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtValue);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnWrite);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.rtxtDisplay);
            this.panel2.Location = new System.Drawing.Point(12, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1242, 427);
            this.panel2.TabIndex = 358;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(84, 539);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(102, 23);
            this.CloseBtn.TabIndex = 383;
            this.CloseBtn.Text = "关闭数据库连接";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(40, 494);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 19);
            this.label1.TabIndex = 42;
            this.label1.Text = "数据库连接状态：";
            // 
            // MySqlConnectionLabel
            // 
            this.MySqlConnectionLabel.AutoSize = true;
            this.MySqlConnectionLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MySqlConnectionLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.MySqlConnectionLabel.Location = new System.Drawing.Point(167, 495);
            this.MySqlConnectionLabel.Name = "MySqlConnectionLabel";
            this.MySqlConnectionLabel.Size = new System.Drawing.Size(45, 19);
            this.MySqlConnectionLabel.TabIndex = 384;
            this.MySqlConnectionLabel.Text = "####";
            // 
            // PlcConnectionLabel
            // 
            this.PlcConnectionLabel.AutoSize = true;
            this.PlcConnectionLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlcConnectionLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.PlcConnectionLabel.Location = new System.Drawing.Point(431, 495);
            this.PlcConnectionLabel.Name = "PlcConnectionLabel";
            this.PlcConnectionLabel.Size = new System.Drawing.Size(45, 19);
            this.PlcConnectionLabel.TabIndex = 386;
            this.PlcConnectionLabel.Text = "####";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.SystemColors.Control;
            this.label12.Location = new System.Drawing.Point(304, 494);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 19);
            this.label12.TabIndex = 385;
            this.label12.Text = "PLC连接状态：";
            // 
            // UserControl_Page5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(78)))), ((int)(((byte)(139)))));
            this.Controls.Add(this.PlcConnectionLabel);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.MySqlConnectionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControl_Page5";
            this.Size = new System.Drawing.Size(1920, 990);
            this.Load += new System.EventHandler(this.UserControl_Page5_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtValue2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboDataType2;
        private System.Windows.Forms.TextBox txtStartAddress2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.RichTextBox rtxtDisplay;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboDataType;
        private System.Windows.Forms.TextBox txtStartAddress;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label MySqlConnectionLabel;
        public System.Windows.Forms.Label PlcConnectionLabel;
        private System.Windows.Forms.Label label12;
    }
}
