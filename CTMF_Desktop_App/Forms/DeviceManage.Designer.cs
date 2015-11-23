namespace CTMF_Desktop_App.Forms
{
	partial class DeviceManage
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
			this.SuspendLayout();
			// 
			// btnAddDevice
			// 
			this.btnAddDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddDevice.Location = new System.Drawing.Point(863, 2);
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
			this.flpnlDevices.Size = new System.Drawing.Size(976, 554);
			this.flpnlDevices.TabIndex = 6;
			this.flpnlDevices.WrapContents = false;
			this.flpnlDevices.SizeChanged += new System.EventHandler(this.flpnlDevices_SizeChanged);
			this.flpnlDevices.MouseEnter += new System.EventHandler(this.flpnlDevices_MouseEnter);
			// 
			// DeviceManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1000, 597);
			this.Controls.Add(this.flpnlDevices);
			this.Controls.Add(this.btnAddDevice);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "DeviceManage";
			this.Text = "Manager";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceManager_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAddDevice;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FlowLayoutPanel flpnlDevices;


	}
}