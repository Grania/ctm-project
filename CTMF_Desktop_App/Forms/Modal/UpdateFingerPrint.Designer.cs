namespace CTMF_Desktop_App.Forms.Modal
{
	partial class UpdateFingerPrint
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbxFingerPosition = new System.Windows.Forms.ComboBox();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblTargetUsername = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point(12, 74);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(256, 288);
			this.panel1.TabIndex = 0;
			// 
			// cbxFingerPosition
			// 
			this.cbxFingerPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFingerPosition.FormattingEnabled = true;
			this.cbxFingerPosition.Location = new System.Drawing.Point(12, 368);
			this.cbxFingerPosition.Name = "cbxFingerPosition";
			this.cbxFingerPosition.Size = new System.Drawing.Size(256, 21);
			this.cbxFingerPosition.TabIndex = 1;
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(12, 37);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(71, 20);
			this.lblStatus.TabIndex = 2;
			this.lblStatus.Text = "lblStatus";
			// 
			// lblTargetUsername
			// 
			this.lblTargetUsername.AutoSize = true;
			this.lblTargetUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTargetUsername.Location = new System.Drawing.Point(12, 9);
			this.lblTargetUsername.Name = "lblTargetUsername";
			this.lblTargetUsername.Size = new System.Drawing.Size(144, 20);
			this.lblTargetUsername.TabIndex = 3;
			this.lblTargetUsername.Text = "lblTargetUsername";
			// 
			// UpdateFingerPrint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(277, 401);
			this.Controls.Add(this.lblTargetUsername);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.cbxFingerPosition);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "UpdateFingerPrint";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "UpdateFingerPrint";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateFingerPrint_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox cbxFingerPosition;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblTargetUsername;
	}
}