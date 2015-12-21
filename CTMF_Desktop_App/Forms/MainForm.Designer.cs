namespace CTMF_Desktop_App.Forms
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.tabControl = new System.Windows.Forms.TabControl();
			this.homeTab = new System.Windows.Forms.TabPage();
			this.pnlHome = new System.Windows.Forms.Panel();
			this.accountManageTab = new System.Windows.Forms.TabPage();
			this.pnlAccountManager = new System.Windows.Forms.Panel();
			this.settingManageTab = new System.Windows.Forms.TabPage();
			this.pnlSettingManage = new System.Windows.Forms.Panel();
			this.transactionViewTab = new System.Windows.Forms.TabPage();
			this.pnlTransactionView = new System.Windows.Forms.Panel();
			this.statusStrip.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.homeTab.SuspendLayout();
			this.accountManageTab.SuspendLayout();
			this.settingManageTab.SuspendLayout();
			this.transactionViewTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Lime;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip.Location = new System.Drawing.Point(0, 708);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1008, 22);
			this.statusStrip.TabIndex = 2;
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "home.png");
			this.imageList.Images.SetKeyName(1, "account.png");
			this.imageList.Images.SetKeyName(2, "FEZ-04-128.png");
			this.imageList.Images.SetKeyName(3, "transaction-icon-16215.png");
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.homeTab);
			this.tabControl.Controls.Add(this.accountManageTab);
			this.tabControl.Controls.Add(this.settingManageTab);
			this.tabControl.Controls.Add(this.transactionViewTab);
			this.tabControl.ImageList = this.imageList;
			this.tabControl.ItemSize = new System.Drawing.Size(100, 100);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1008, 705);
			this.tabControl.TabIndex = 1;
			this.tabControl.TabStop = false;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// homeTab
			// 
			this.homeTab.Controls.Add(this.pnlHome);
			this.homeTab.ImageIndex = 0;
			this.homeTab.Location = new System.Drawing.Point(4, 104);
			this.homeTab.Name = "homeTab";
			this.homeTab.Padding = new System.Windows.Forms.Padding(3);
			this.homeTab.Size = new System.Drawing.Size(1000, 597);
			this.homeTab.TabIndex = 0;
			this.homeTab.UseVisualStyleBackColor = true;
			// 
			// pnlHome
			// 
			this.pnlHome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHome.Location = new System.Drawing.Point(0, 0);
			this.pnlHome.Name = "pnlHome";
			this.pnlHome.Size = new System.Drawing.Size(1000, 597);
			this.pnlHome.TabIndex = 0;
			this.pnlHome.SizeChanged += new System.EventHandler(this.pnlHome_SizeChanged);
			// 
			// accountManageTab
			// 
			this.accountManageTab.Controls.Add(this.pnlAccountManager);
			this.accountManageTab.ImageIndex = 1;
			this.accountManageTab.Location = new System.Drawing.Point(4, 104);
			this.accountManageTab.Name = "accountManageTab";
			this.accountManageTab.Size = new System.Drawing.Size(1000, 597);
			this.accountManageTab.TabIndex = 1;
			this.accountManageTab.UseVisualStyleBackColor = true;
			// 
			// pnlAccountManager
			// 
			this.pnlAccountManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAccountManager.Location = new System.Drawing.Point(0, 0);
			this.pnlAccountManager.Name = "pnlAccountManager";
			this.pnlAccountManager.Size = new System.Drawing.Size(1000, 597);
			this.pnlAccountManager.TabIndex = 0;
			this.pnlAccountManager.SizeChanged += new System.EventHandler(this.pnlAccountManage_SizeChanged);
			// 
			// settingManageTab
			// 
			this.settingManageTab.Controls.Add(this.pnlSettingManage);
			this.settingManageTab.ImageIndex = 2;
			this.settingManageTab.Location = new System.Drawing.Point(4, 104);
			this.settingManageTab.Name = "settingManageTab";
			this.settingManageTab.Size = new System.Drawing.Size(1000, 597);
			this.settingManageTab.TabIndex = 2;
			this.settingManageTab.UseVisualStyleBackColor = true;
			// 
			// pnlSettingManage
			// 
			this.pnlSettingManage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSettingManage.AutoScroll = true;
			this.pnlSettingManage.Location = new System.Drawing.Point(0, 0);
			this.pnlSettingManage.Name = "pnlSettingManage";
			this.pnlSettingManage.Size = new System.Drawing.Size(1000, 597);
			this.pnlSettingManage.TabIndex = 0;
			// 
			// transactionViewTab
			// 
			this.transactionViewTab.Controls.Add(this.pnlTransactionView);
			this.transactionViewTab.ImageIndex = 3;
			this.transactionViewTab.Location = new System.Drawing.Point(4, 104);
			this.transactionViewTab.Name = "transactionViewTab";
			this.transactionViewTab.Size = new System.Drawing.Size(1000, 597);
			this.transactionViewTab.TabIndex = 3;
			this.transactionViewTab.UseVisualStyleBackColor = true;
			// 
			// pnlTransactionView
			// 
			this.pnlTransactionView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTransactionView.Location = new System.Drawing.Point(0, 0);
			this.pnlTransactionView.Name = "pnlTransactionView";
			this.pnlTransactionView.Size = new System.Drawing.Size(1000, 597);
			this.pnlTransactionView.TabIndex = 0;
			this.pnlTransactionView.SizeChanged += new System.EventHandler(this.pnlTransactionView_SizeChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 730);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.statusStrip);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CTM";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.homeTab.ResumeLayout(false);
			this.accountManageTab.ResumeLayout(false);
			this.settingManageTab.ResumeLayout(false);
			this.transactionViewTab.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage accountManageTab;
		private System.Windows.Forms.TabPage homeTab;
		private System.Windows.Forms.Panel pnlHome;
		private System.Windows.Forms.Panel pnlAccountManager;
		private System.Windows.Forms.TabPage settingManageTab;
		private System.Windows.Forms.Panel pnlSettingManage;
		private System.Windows.Forms.TabPage transactionViewTab;
		private System.Windows.Forms.Panel pnlTransactionView;
	}
}