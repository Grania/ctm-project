namespace CTMF_Desktop_App.Forms.Modal
{
	partial class AddDevice
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
			this.lbPort = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnConScanner = new System.Windows.Forms.Button();
			this.btnConDisplay = new System.Windows.Forms.Button();
			this.btnDone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbPort
			// 
			this.lbPort.FormattingEnabled = true;
			this.lbPort.Location = new System.Drawing.Point(12, 29);
			this.lbPort.Name = "lbPort";
			this.lbPort.Size = new System.Drawing.Size(157, 303);
			this.lbPort.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Danh sách thiết bị đang kế nối:";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(175, 278);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(130, 48);
			this.btnReset.TabIndex = 2;
			this.btnReset.Text = "Làm lại";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnConScanner
			// 
			this.btnConScanner.Location = new System.Drawing.Point(175, 111);
			this.btnConScanner.Name = "btnConScanner";
			this.btnConScanner.Size = new System.Drawing.Size(130, 48);
			this.btnConScanner.TabIndex = 3;
			this.btnConScanner.Text = "Kết nối máy quét";
			this.btnConScanner.UseVisualStyleBackColor = true;
			this.btnConScanner.Click += new System.EventHandler(this.btnConScanner_Click);
			// 
			// btnConDisplay
			// 
			this.btnConDisplay.Location = new System.Drawing.Point(175, 165);
			this.btnConDisplay.Name = "btnConDisplay";
			this.btnConDisplay.Size = new System.Drawing.Size(130, 51);
			this.btnConDisplay.TabIndex = 4;
			this.btnConDisplay.Text = "Kết nối màn hình";
			this.btnConDisplay.UseVisualStyleBackColor = true;
			this.btnConDisplay.Click += new System.EventHandler(this.btnConDisplay_Click);
			// 
			// btnDone
			// 
			this.btnDone.Location = new System.Drawing.Point(175, 29);
			this.btnDone.Name = "btnDone";
			this.btnDone.Size = new System.Drawing.Size(130, 41);
			this.btnDone.TabIndex = 5;
			this.btnDone.Text = "Xong";
			this.btnDone.UseVisualStyleBackColor = true;
			this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
			// 
			// AddDevice
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 338);
			this.Controls.Add(this.btnDone);
			this.Controls.Add(this.btnConDisplay);
			this.Controls.Add(this.btnConScanner);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbPort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddDevice";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Thêm thiết bị";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddDevice_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnConScanner;
		private System.Windows.Forms.Button btnConDisplay;
		private System.Windows.Forms.Button btnDone;
	}
}