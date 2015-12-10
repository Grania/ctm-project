using CTMF_Desktop_App.Forms.Modal;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms
{
	public partial class MainForm : Form
	{
		internal static string username;
		internal static DateTime loginTime;

		internal static Home homeForm;
		internal static AccountManage accountManageForm;
		internal static SettingManage settingManageForm;

		public MainForm(string username_, DateTime loginTime_)
		{
			//username = username_;
			//loginTime = loginTime_;

			username = "dungnmse02767";
			loginTime = DateTime.Now;

			InitializeComponent();

			homeForm = new Home();
			homeForm.TopLevel = false;
			this.pnlHome.Controls.Add(homeForm);
			homeForm.Show();

			accountManageForm = new AccountManage();
			accountManageForm.TopLevel = false;
			this.pnlAccountManager.Controls.Add(accountManageForm);
			accountManageForm.Show();

			settingManageForm = new SettingManage();
			settingManageForm.TopLevel = false;
			this.pnlSettingManage.Controls.Add(settingManageForm);
			settingManageForm.Show();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void pnlHome_SizeChanged(object sender, EventArgs e)
		{
			homeForm.Height = this.pnlHome.Height;
			homeForm.Width = this.pnlHome.Width;
		}

		private void pnlAccountManage_SizeChanged(object sender, EventArgs e)
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
