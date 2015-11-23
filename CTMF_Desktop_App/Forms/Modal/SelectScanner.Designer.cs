namespace CTMF_Desktop_App.Forms.Modal
{
	partial class SelectScanner
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
			this.label1 = new System.Windows.Forms.Label();
			this.lbPort = new System.Windows.Forms.ListBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Danh sách thiết bị đang kế nối:";
			// 
			// lbPort
			// 
			this.lbPort.FormattingEnabled = true;
			this.lbPort.Location = new System.Drawing.Point(11, 25);
			this.lbPort.Name = "lbPort";
			this.lbPort.Size = new System.Drawing.Size(157, 329);
			this.lbPort.TabIndex = 2;
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(175, 25);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(97, 44);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "Đồng ý";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(175, 327);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(97, 23);
			this.btnRefresh.TabIndex = 5;
			this.btnRefresh.Text = "Làm mới";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// SelectScanner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 362);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbPort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SelectScanner";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "SelectScanner";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbPort;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnRefresh;
	}
}