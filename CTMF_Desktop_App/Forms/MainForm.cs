using CTMF_Desktop_App.Forms.Modal;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms
{
	public partial class MainForm : Form
	{
		public static string username;

		DeviceManage deviceManageForm;
		AccountManage accountManageForm;
		lblLastSync settingManageForm;

		public MainForm()
		{
			username = "dungnmse02767";
			InitializeComponent();

			deviceManageForm = new DeviceManage();
			deviceManageForm.TopLevel = false;
			this.pnlDeviceManage.Controls.Add(deviceManageForm);
			deviceManageForm.Show();

			accountManageForm = new AccountManage();
			accountManageForm.TopLevel = false;
			this.pnlAccountManager.Controls.Add(accountManageForm);
			accountManageForm.Show();

			settingManageForm = new lblLastSync();
			settingManageForm.TopLevel = false;
			this.pnlSettingManage.Controls.Add(settingManageForm);
			settingManageForm.Show();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void pnlDeviceManage_SizeChanged(object sender, EventArgs e)
		{
			deviceManageForm.Height = this.pnlDeviceManage.Height;
			deviceManageForm.Width = this.pnlDeviceManage.Width;
		}

		private void pnlAccountManager_SizeChanged(object sender, EventArgs e)
		{
			accountManageForm.Height = this.pnlAccountManager.Height;
			accountManageForm.Width = this.pnlAccountManager.Width;
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage selectedTab = tabControl.SelectedTab;

			if (selectedTab == accountManageTab)
			{
				accountManageForm.LoadForm();
			}
			if (selectedTab == settingManageTab)
			{
				pnlSettingManage.Focus();
			}
		}
	}
}
