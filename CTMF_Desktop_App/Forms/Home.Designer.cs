namespace CTMF_Desktop_App.Forms
{
	partial class Home
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
			this.btnAddDevice = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.flpnlDevices = new System.Windows.Forms.FlowLayoutPanel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblTransactionCount = new System.Windows.Forms.Label();
			this.lblLoginTime = new System.Windows.Forms.Label();
			this.lblDeviceCount = new System.Windows.Forms.Label();
			this.lblTransactionToServerCount = new System.Windows.Forms.Label();
			this.lblFingerPrintCount = new System.Windows.Forms.Label();
			this.lblUserCount = new System.Windows.Forms.Label();
			this.lblUsername = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblUpdateServingTime = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblServingTime = new System.Windows.Forms.Label();
			this.lblDate = new System.Windows.Forms.Label();
			this.dgvScheduleMealSet = new System.Windows.Forms.DataGridView();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.pnlHiddenEatingInfo = new System.Windows.Forms.Panel();
			this.label11 = new System.Windows.Forms.Label();
			this.btnSetServingTime = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvScheduleMealSet)).BeginInit();
			this.pnlHiddenEatingInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnAddDevice
			// 
			this.btnAddDevice.Location = new System.Drawing.Point(191, 5);
			this.btnAddDevice.Name = "btnAddDevice";
			this.btnAddDevice.Size = new System.Drawing.Size(125, 23);
			this.btnAddDevice.TabIndex = 5;
			this.btnAddDevice.Text = "Thêm thiết bị";
			this.btnAddDevice.UseVisualStyleBackColor = true;
			this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Danh sách thiết bị đang hoạt động:";
			// 
			// flpnlDevices
			// 
			this.flpnlDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flpnlDevices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flpnlDevices.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpnlDevices.Location = new System.Drawing.Point(12, 31);
			this.flpnlDevices.Name = "flpnlDevices";
			this.flpnlDevices.Size = new System.Drawing.Size(544, 554);
			this.flpnlDevices.TabIndex = 6;
			this.flpnlDevices.WrapContents = false;
			this.flpnlDevices.SizeChanged += new System.EventHandler(this.flpnlDevices_SizeChanged);
			this.flpnlDevices.MouseEnter += new System.EventHandler(this.flpnlDevices_MouseEnter);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lblTransactionCount);
			this.groupBox1.Controls.Add(this.lblLoginTime);
			this.groupBox1.Controls.Add(this.lblDeviceCount);
			this.groupBox1.Controls.Add(this.lblTransactionToServerCount);
			this.groupBox1.Controls.Add(this.lblFingerPrintCount);
			this.groupBox1.Controls.Add(this.lblUserCount);
			this.groupBox1.Controls.Add(this.lblUsername);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(562, 15);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(426, 232);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Thông tin hệ thống";
			// 
			// lblTransactionCount
			// 
			this.lblTransactionCount.AutoSize = true;
			this.lblTransactionCount.Location = new System.Drawing.Point(238, 161);
			this.lblTransactionCount.Name = "lblTransactionCount";
			this.lblTransactionCount.Size = new System.Drawing.Size(101, 13);
			this.lblTransactionCount.TabIndex = 13;
			this.lblTransactionCount.Text = "lblTransactionCount";
			// 
			// lblLoginTime
			// 
			this.lblLoginTime.AutoSize = true;
			this.lblLoginTime.Location = new System.Drawing.Point(238, 52);
			this.lblLoginTime.Name = "lblLoginTime";
			this.lblLoginTime.Size = new System.Drawing.Size(66, 13);
			this.lblLoginTime.TabIndex = 12;
			this.lblLoginTime.Text = "lblLoginTime";
			// 
			// lblDeviceCount
			// 
			this.lblDeviceCount.AutoSize = true;
			this.lblDeviceCount.Location = new System.Drawing.Point(238, 80);
			this.lblDeviceCount.Name = "lblDeviceCount";
			this.lblDeviceCount.Size = new System.Drawing.Size(79, 13);
			this.lblDeviceCount.TabIndex = 11;
			this.lblDeviceCount.Text = "lblDeviceCount";
			// 
			// lblTransactionToServerCout
			// 
			this.lblTransactionToServerCount.AutoSize = true;
			this.lblTransactionToServerCount.Location = new System.Drawing.Point(238, 192);
			this.lblTransactionToServerCount.Name = "lblTransactionToServerCout";
			this.lblTransactionToServerCount.Size = new System.Drawing.Size(139, 13);
			this.lblTransactionToServerCount.TabIndex = 10;
			this.lblTransactionToServerCount.Text = "lblTransactionToServerCout";
			// 
			// lblFingerPrintCount
			// 
			this.lblFingerPrintCount.AutoSize = true;
			this.lblFingerPrintCount.Location = new System.Drawing.Point(238, 134);
			this.lblFingerPrintCount.Name = "lblFingerPrintCount";
			this.lblFingerPrintCount.Size = new System.Drawing.Size(95, 13);
			this.lblFingerPrintCount.TabIndex = 9;
			this.lblFingerPrintCount.Text = "lblFingerPrintCount";
			// 
			// lblUserCount
			// 
			this.lblUserCount.AutoSize = true;
			this.lblUserCount.Location = new System.Drawing.Point(238, 107);
			this.lblUserCount.Name = "lblUserCount";
			this.lblUserCount.Size = new System.Drawing.Size(67, 13);
			this.lblUserCount.TabIndex = 8;
			this.lblUserCount.Text = "lblUserCount";
			// 
			// lblUsername
			// 
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(238, 28);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(65, 13);
			this.lblUsername.TabIndex = 7;
			this.lblUsername.Text = "lblUsername";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(24, 107);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(198, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "Số lượng người dùng có trong hệ thông :\r\n";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(24, 134);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(162, 13);
			this.label7.TabIndex = 5;
			this.label7.Text = "Số lượng vân tay được sử dụng :";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(186, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Số lượng thiết bị đang được sử dụng :";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 192);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(211, 13);
			this.label5.TabIndex = 3;
			this.label5.Text = "Số lượng giao dịch đã được gửi lên server :";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(24, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(109, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Thời gian đăng nhập:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 161);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(200, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Số lần thực hiện giao dịch trong lần này :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Tên đăng nhập :";
			// 
			// lblUpdateServingTime
			// 
			this.lblUpdateServingTime.Location = new System.Drawing.Point(325, 28);
			this.lblUpdateServingTime.Name = "lblUpdateServingTime";
			this.lblUpdateServingTime.Size = new System.Drawing.Size(94, 20);
			this.lblUpdateServingTime.TabIndex = 8;
			this.lblUpdateServingTime.Text = "Thiết đặt lại";
			this.lblUpdateServingTime.UseVisualStyleBackColor = true;
			this.lblUpdateServingTime.Click += new System.EventHandler(this.lblUpdateServingTime_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.pnlHiddenEatingInfo);
			this.groupBox2.Controls.Add(this.lblServingTime);
			this.groupBox2.Controls.Add(this.lblDate);
			this.groupBox2.Controls.Add(this.dgvScheduleMealSet);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.lblUpdateServingTime);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Location = new System.Drawing.Point(563, 253);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(425, 332);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Thông tin bữa ăn";
			// 
			// lblServingTime
			// 
			this.lblServingTime.AutoSize = true;
			this.lblServingTime.Location = new System.Drawing.Point(76, 66);
			this.lblServingTime.Name = "lblServingTime";
			this.lblServingTime.Size = new System.Drawing.Size(76, 13);
			this.lblServingTime.TabIndex = 11;
			this.lblServingTime.Text = "lblServingTime";
			// 
			// lblDate
			// 
			this.lblDate.AutoSize = true;
			this.lblDate.Location = new System.Drawing.Point(76, 35);
			this.lblDate.Name = "lblDate";
			this.lblDate.Size = new System.Drawing.Size(40, 13);
			this.lblDate.TabIndex = 10;
			this.lblDate.Text = "lblDate";
			// 
			// dgvScheduleMealSet
			// 
			this.dgvScheduleMealSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvScheduleMealSet.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.dgvScheduleMealSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvScheduleMealSet.GridColor = System.Drawing.SystemColors.Control;
			this.dgvScheduleMealSet.Location = new System.Drawing.Point(15, 92);
			this.dgvScheduleMealSet.Name = "dgvScheduleMealSet";
			this.dgvScheduleMealSet.Size = new System.Drawing.Size(404, 234);
			this.dgvScheduleMealSet.TabIndex = 9;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(23, 66);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(47, 13);
			this.label10.TabIndex = 1;
			this.label10.Text = "Bữa ăn :";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(23, 35);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(38, 13);
			this.label9.TabIndex = 0;
			this.label9.Text = "Ngày :";
			// 
			// pnlHiddenEatingInfo
			// 
			this.pnlHiddenEatingInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHiddenEatingInfo.Controls.Add(this.btnSetServingTime);
			this.pnlHiddenEatingInfo.Controls.Add(this.label11);
			this.pnlHiddenEatingInfo.Location = new System.Drawing.Point(6, 19);
			this.pnlHiddenEatingInfo.Name = "pnlHiddenEatingInfo";
			this.pnlHiddenEatingInfo.Size = new System.Drawing.Size(413, 307);
			this.pnlHiddenEatingInfo.TabIndex = 12;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
			this.label11.Location = new System.Drawing.Point(96, 83);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(230, 25);
			this.label11.TabIndex = 0;
			this.label11.Text = "Chưa có thông tin bữa ăn";
			// 
			// btnSetServingTime
			// 
			this.btnSetServingTime.Location = new System.Drawing.Point(140, 111);
			this.btnSetServingTime.Name = "btnSetServingTime";
			this.btnSetServingTime.Size = new System.Drawing.Size(141, 23);
			this.btnSetServingTime.TabIndex = 1;
			this.btnSetServingTime.Text = "Thiết đặt thông tin bữa ăn";
			this.btnSetServingTime.UseVisualStyleBackColor = true;
			this.btnSetServingTime.Click += new System.EventHandler(this.btnSetServingTime_Click);
			// 
			// Home
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1000, 597);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.flpnlDevices);
			this.Controls.Add(this.btnAddDevice);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Home";
			this.Text = "Manager";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceManager_FormClosed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvScheduleMealSet)).EndInit();
			this.pnlHiddenEatingInfo.ResumeLayout(false);
			this.pnlHiddenEatingInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAddDevice;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FlowLayoutPanel flpnlDevices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button lblUpdateServingTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblLoginTime;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.Label lblTransactionToServerCount;
        private System.Windows.Forms.Label lblFingerPrintCount;
        private System.Windows.Forms.Label lblUserCount;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblTransactionCount;
		private System.Windows.Forms.DataGridView dgvScheduleMealSet;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblServingTime;
		private System.Windows.Forms.Label lblDate;
		private System.Windows.Forms.Panel pnlHiddenEatingInfo;
		private System.Windows.Forms.Button btnSetServingTime;
		private System.Windows.Forms.Label label11;


	}
}