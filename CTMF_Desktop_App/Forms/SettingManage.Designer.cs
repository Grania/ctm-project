namespace CTMF_Desktop_App.Forms
{
	partial class lblLastSync
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
			this.btnResync = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSync = new System.Windows.Forms.Button();
			this.gbSync.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSync
			// 
			this.gbSync.Controls.Add(this.btnSync);
			this.gbSync.Controls.Add(this.label2);
			this.gbSync.Controls.Add(this.label1);
			this.gbSync.Controls.Add(this.btnResync);
			this.gbSync.Location = new System.Drawing.Point(12, 485);
			this.gbSync.Name = "gbSync";
			this.gbSync.Size = new System.Drawing.Size(976, 100);
			this.gbSync.TabIndex = 0;
			this.gbSync.TabStop = false;
			this.gbSync.Text = "Đồng bộ hóa";
			// 
			// btnResync
			// 
			this.btnResync.Location = new System.Drawing.Point(173, 58);
			this.btnResync.Name = "btnResync";
			this.btnResync.Size = new System.Drawing.Size(120, 23);
			this.btnResync.TabIndex = 0;
			this.btnResync.Text = "Đồng bộ hóa từ đầu";
			this.btnResync.UseVisualStyleBackColor = true;
			this.btnResync.Click += new System.EventHandler(this.btnResync_Click);
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(150, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "lblLastSync";
			// 
			// btnSync
			// 
			this.btnSync.Location = new System.Drawing.Point(10, 58);
			this.btnSync.Name = "btnSync";
			this.btnSync.Size = new System.Drawing.Size(134, 23);
			this.btnSync.TabIndex = 3;
			this.btnSync.Text = "Đồng bộ hóa";
			this.btnSync.UseVisualStyleBackColor = true;
			this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
			// 
			// lblLastSync
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1000, 597);
			this.Controls.Add(this.gbSync);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "lblLastSync";
			this.Text = "SettingManage";
			this.gbSync.ResumeLayout(false);
			this.gbSync.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbSync;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnResync;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSync;
	}
}