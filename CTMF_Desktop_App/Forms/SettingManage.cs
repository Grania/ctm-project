using CTMF_Desktop_App.DataAccessTableAdapters;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms
{
	public partial class SettingManage : Form
	{
		BackgroundWorker bwSync;

		public SettingManage()
		{
			InitializeComponent();

			bwSync = new BackgroundWorker();

			lblLastSync.Text = XmlSync.GetLastSync().ToString("dd/MM/yyyy HH:mm:ss tt");
		}

		private void btnResync_Click(object sender, EventArgs e)
		{
			if (bwSync.IsBusy)
			{
				MessageBox.Show("Đang đồng bộ hóa, vui long chờ.");
				return;
			}

			bwSync = new BackgroundWorker();
			bwSync.DoWork += new DoWorkEventHandler(bwSync_NewSync);

			bwSync.RunWorkerAsync();
			
		}

		private void btnSync_Click(object sender, EventArgs e)
		{
			if (bwSync.IsBusy)
			{
				MessageBox.Show("Đang đồng bộ hóa, vui long chờ.");
				return;
			}

			bwSync = new BackgroundWorker();
			bwSync.DoWork += new DoWorkEventHandler(bwSync_StartSync);

			bwSync.RunWorkerAsync();
		}

		private void bwSync_StartSync(object sender, DoWorkEventArgs e)
		{
			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "Đang đồng bộ.";
			});

			try
			{
				DataAccess.StartSync();
				Home.LoadCustomerData();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra.");
				Log.ErrorLog(ex.Message);
			}

			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "";
			});
			lblLastSync.Invoke((MethodInvoker)delegate
			{
				lblLastSync.Text = XmlSync.GetLastSync().ToString("dd/MM/yyyy HH:mm:ss tt");
			});
		}

		private void bwSync_NewSync(object sender, DoWorkEventArgs e)
		{
			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "Đang đồng bộ.";
			});

			try
			{
				DataAccess.NewSync();
				Home.LoadCustomerData();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra.");
				Log.ErrorLog(ex.Message);
			}

			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "";
			});
			lblLastSync.Invoke((MethodInvoker)delegate
			{
				lblLastSync.Text = XmlSync.GetLastSync().ToString("dd/MM/yyyy HH:mm:ss tt");
			});
		}
	}
}
