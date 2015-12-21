namespace CTMF_Desktop_App.Forms
{
	partial class AccountManage
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtUsernameSearch = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.cbxActive = new System.Windows.Forms.ComboBox();
			this.cbxUserTypeSearch = new System.Windows.Forms.ComboBox();
			this.txtNameSearch = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnUpdateFingerPrint = new System.Windows.Forms.Button();
			this.cbSendToServer = new System.Windows.Forms.CheckBox();
			this.bwSend = new System.ComponentModel.BackgroundWorker();
			this.lblSendReport = new System.Windows.Forms.Label();
			this.btnStopDevice = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtUsernameSearch);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.btnSearch);
			this.groupBox1.Controls.Add(this.cbxActive);
			this.groupBox1.Controls.Add(this.cbxUserTypeSearch);
			this.groupBox1.Controls.Add(this.txtNameSearch);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(976, 95);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tìm kiếm";
			// 
			// txtUsernameSearch
			// 
			this.txtUsernameSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsernameSearch.Location = new System.Drawing.Point(99, 27);
			this.txtUsernameSearch.Name = "txtUsernameSearch";
			this.txtUsernameSearch.Size = new System.Drawing.Size(491, 20);
			this.txtUsernameSearch.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(87, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Tên đăng nhập :";
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(847, 24);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(113, 60);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "Tìm kiếm";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// cbxActive
			// 
			this.cbxActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxActive.FormattingEnabled = true;
			this.cbxActive.Location = new System.Drawing.Point(720, 63);
			this.cbxActive.Name = "cbxActive";
			this.cbxActive.Size = new System.Drawing.Size(121, 21);
			this.cbxActive.TabIndex = 5;
			// 
			// cbxUserTypeSearch
			// 
			this.cbxUserTypeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxUserTypeSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxUserTypeSearch.FormattingEnabled = true;
			this.cbxUserTypeSearch.Location = new System.Drawing.Point(688, 27);
			this.cbxUserTypeSearch.Name = "cbxUserTypeSearch";
			this.cbxUserTypeSearch.Size = new System.Drawing.Size(153, 21);
			this.cbxUserTypeSearch.TabIndex = 4;
			// 
			// txtNameSearch
			// 
			this.txtNameSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNameSearch.Location = new System.Drawing.Point(99, 63);
			this.txtNameSearch.Name = "txtNameSearch";
			this.txtNameSearch.Size = new System.Drawing.Size(491, 20);
			this.txtNameSearch.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(650, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Hoạt động :";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(623, 30);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Đối tượng :";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tên :";
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToOrderColumns = true;
			this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Location = new System.Drawing.Point(12, 113);
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.ReadOnly = true;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView.Size = new System.Drawing.Size(976, 443);
			this.dataGridView.TabIndex = 1;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Location = new System.Drawing.Point(859, 562);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(113, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "Làm mới";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnUpdateFingerPrint
			// 
			this.btnUpdateFingerPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnUpdateFingerPrint.Location = new System.Drawing.Point(145, 564);
			this.btnUpdateFingerPrint.Name = "btnUpdateFingerPrint";
			this.btnUpdateFingerPrint.Size = new System.Drawing.Size(108, 23);
			this.btnUpdateFingerPrint.TabIndex = 3;
			this.btnUpdateFingerPrint.Text = "Cập nhật dấu vân tay";
			this.btnUpdateFingerPrint.UseVisualStyleBackColor = true;
			this.btnUpdateFingerPrint.Click += new System.EventHandler(this.btnUpdateFingerPrint_Click);
			// 
			// cbSendToServer
			// 
			this.cbSendToServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSendToServer.AutoSize = true;
			this.cbSendToServer.Location = new System.Drawing.Point(259, 568);
			this.cbSendToServer.Name = "cbSendToServer";
			this.cbSendToServer.Size = new System.Drawing.Size(91, 17);
			this.cbSendToServer.TabIndex = 4;
			this.cbSendToServer.Text = "Gửi lên server";
			this.cbSendToServer.UseVisualStyleBackColor = true;
			// 
			// bwSend
			// 
			this.bwSend.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwSendToServer_DoWork);
			this.bwSend.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			// 
			// lblSendReport
			// 
			this.lblSendReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblSendReport.AutoSize = true;
			this.lblSendReport.Location = new System.Drawing.Point(356, 569);
			this.lblSendReport.Name = "lblSendReport";
			this.lblSendReport.Size = new System.Drawing.Size(74, 13);
			this.lblSendReport.TabIndex = 5;
			this.lblSendReport.Text = "lblSendReport";
			// 
			// btnStopDevice
			// 
			this.btnStopDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStopDevice.Enabled = false;
			this.btnStopDevice.Location = new System.Drawing.Point(12, 564);
			this.btnStopDevice.Name = "btnStopDevice";
			this.btnStopDevice.Size = new System.Drawing.Size(128, 23);
			this.btnStopDevice.TabIndex = 6;
			this.btnStopDevice.Text = "Dừng sử dụng máy quét";
			this.btnStopDevice.UseVisualStyleBackColor = true;
			this.btnStopDevice.Click += new System.EventHandler(this.btnStopDevice_Click);
			// 
			// AccountManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1000, 597);
			this.Controls.Add(this.btnStopDevice);
			this.Controls.Add(this.lblSendReport);
			this.Controls.Add(this.cbSendToServer);
			this.Controls.Add(this.btnUpdateFingerPrint);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.dataGridView);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AccountManage";
			this.Text = "AccountManager";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtNameSearch;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtUsernameSearch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ComboBox cbxActive;
		private System.Windows.Forms.ComboBox cbxUserTypeSearch;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnUpdateFingerPrint;
		private System.Windows.Forms.CheckBox cbSendToServer;
		private System.ComponentModel.BackgroundWorker bwSend;
		private System.Windows.Forms.Label lblSendReport;
		private System.Windows.Forms.Button btnStopDevice;


	}
}