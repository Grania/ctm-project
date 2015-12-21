namespace CTMF_Desktop_App.Forms
{
	partial class SettingManage
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
			this.gbSync = new System.Windows.Forms.GroupBox();
			this.lblSyncStatus = new System.Windows.Forms.Label();
			this.btnSync = new System.Windows.Forms.Button();
			this.lblLastSync = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnResync = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtAutoSync = new System.Windows.Forms.TextBox();
			this.btnAutoSync = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lblAutoSyncTimeCount = new System.Windows.Forms.Label();
			this.gbSync.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSync
			// 
			this.gbSync.Controls.Add(this.lblAutoSyncTimeCount);
			this.gbSync.Controls.Add(this.label3);
			this.gbSync.Controls.Add(this.btnAutoSync);
			this.gbSync.Controls.Add(this.txtAutoSync);
			this.gbSync.Controls.Add(this.label2);
			this.gbSync.Controls.Add(this.lblSyncStatus);
			this.gbSync.Controls.Add(this.btnSync);
			this.gbSync.Controls.Add(this.lblLastSync);
			this.gbSync.Controls.Add(this.label1);
			this.gbSync.Controls.Add(this.btnResync);
			this.gbSync.Location = new System.Drawing.Point(12, 12);
			this.gbSync.Name = "gbSync";
			this.gbSync.Size = new System.Drawing.Size(976, 100);
			this.gbSync.TabIndex = 0;
			this.gbSync.TabStop = false;
			this.gbSync.Text = "Đồng bộ hóa";
			// 
			// lblSyncStatus
			// 
			this.lblSyncStatus.AutoSize = true;
			this.lblSyncStatus.Location = new System.Drawing.Point(791, 20);
			this.lblSyncStatus.Name = "lblSyncStatus";
			this.lblSyncStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblSyncStatus.Size = new System.Drawing.Size(71, 13);
			this.lblSyncStatus.TabIndex = 4;
			this.lblSyncStatus.Text = "lblSyncStatus";
			// 
			// btnSync
			// 
			this.btnSync.Location = new System.Drawing.Point(707, 58);
			this.btnSync.Name = "btnSync";
			this.btnSync.Size = new System.Drawing.Size(134, 23);
			this.btnSync.TabIndex = 3;
			this.btnSync.Text = "Đồng bộ hóa";
			this.btnSync.UseVisualStyleBackColor = true;
			this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
			// 
			// lblLastSync
			// 
			this.lblLastSync.AutoSize = true;
			this.lblLastSync.Location = new System.Drawing.Point(150, 20);
			this.lblLastSync.Name = "lblLastSync";
			this.lblLastSync.Size = new System.Drawing.Size(61, 13);
			this.lblLastSync.TabIndex = 2;
			this.lblLastSync.Text = "lblLastSync";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(137, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Lần đồng bộ hóa gần nhất:";
			// 
			// btnResync
			// 
			this.btnResync.Location = new System.Drawing.Point(847, 58);
			this.btnResync.Name = "btnResync";
			this.btnResync.Size = new System.Drawing.Size(120, 23);
			this.btnResync.TabIndex = 0;
			this.btnResync.Text = "Đồng bộ hóa từ đầu";
			this.btnResync.UseVisualStyleBackColor = true;
			this.btnResync.Click += new System.EventHandler(this.btnResync_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Tự đồng bộ hóa sau:";
			// 
			// txtAutoSync
			// 
			this.txtAutoSync.Location = new System.Drawing.Point(120, 60);
			this.txtAutoSync.Name = "txtAutoSync";
			this.txtAutoSync.Size = new System.Drawing.Size(113, 20);
			this.txtAutoSync.TabIndex = 1;
			// 
			// btnAutoSync
			// 
			this.btnAutoSync.Location = new System.Drawing.Point(273, 58);
			this.btnAutoSync.Name = "btnAutoSync";
			this.btnAutoSync.Size = new System.Drawing.Size(75, 23);
			this.btnAutoSync.TabIndex = 6;
			this.btnAutoSync.Text = "Đồng ý";
			this.btnAutoSync.UseVisualStyleBackColor = true;
			this.btnAutoSync.Click += new System.EventHandler(this.btnAutoSync_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(239, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(28, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "phút";
			// 
			// lblAutoSyncTimeCount
			// 
			this.lblAutoSyncTimeCount.AutoSize = true;
			this.lblAutoSyncTimeCount.Location = new System.Drawing.Point(354, 63);
			this.lblAutoSyncTimeCount.Name = "lblAutoSyncTimeCount";
			this.lblAutoSyncTimeCount.Size = new System.Drawing.Size(114, 13);
			this.lblAutoSyncTimeCount.TabIndex = 8;
			this.lblAutoSyncTimeCount.Text = "lblAutoSyncTimeCount";
			// 
			// SettingManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1000, 597);
			this.Controls.Add(this.gbSync);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SettingManage";
			this.Text = "SettingManage";
			this.gbSync.ResumeLayout(false);
			this.gbSync.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbSync;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnResync;
		private System.Windows.Forms.Label lblLastSync;
		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.Label lblSyncStatus;
		private System.Windows.Forms.Label lblAutoSyncTimeCount;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnAutoSync;
		private System.Windows.Forms.TextBox txtAutoSync;
		private System.Windows.Forms.Label label2;
	}
}