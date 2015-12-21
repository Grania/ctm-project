using CTMF_Desktop_App.DataAccessTableAdapters;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms
{
	public partial class SettingManage : Form
	{
		BackgroundWorker bwSync;
		BackgroundWorker bwSyncTimeCount;

		public SettingManage()
		{
			InitializeComponent();

			bwSync = new BackgroundWorker();
			bwSyncTimeCount = new BackgroundWorker();
			bwSyncTimeCount.WorkerSupportsCancellation = true;
			bwSyncTimeCount.DoWork += new DoWorkEventHandler(bwSyncTimeCount_DoWork);
			lblSyncStatus.Text = "";
			lblAutoSyncTimeCount.Text = "";

			DateTime? lastSync = XmlSync.GetLastSync();
			if (lastSync == null)
			{
				lblLastSync.Text = "";
			}
			else
			{
				lblLastSync.Text = XmlSync.GetLastSync().Value.ToString("dd/MM/yyyy HH:mm:ss tt");
			}
		}

		private void bwSyncTimeCount_DoWork(object sender, DoWorkEventArgs e)
		{
			int min = (int)e.Argument;

			DateTime now = DateTime.Now;
			now = now.AddMinutes(min);

			TimeSpan timeCount = TimeSpan.FromMinutes(min);

			lblAutoSyncTimeCount.Invoke((MethodInvoker)delegate
			{
				lblAutoSyncTimeCount.Text = timeCount.ToString("h'h 'm'm 's's'");
			});

			while (!bwSyncTimeCount.CancellationPending)
			{
				Thread.Sleep(1000);
				if (timeCount <= TimeSpan.Zero)
				{
					btnSync_Click(sender, e);
					timeCount = TimeSpan.FromMinutes(min);
				}

				timeCount = timeCount.Subtract(TimeSpan.FromSeconds(1));
				lblAutoSyncTimeCount.Invoke((MethodInvoker)delegate
				{
					lblAutoSyncTimeCount.Text = timeCount.ToString("h'h 'm'm 's's'");
				});
			}
		}

		private void btnResync_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("Đồng bộ hóa từ đầu sẽ xóa sạch các dữ liệu\nchưa được đồng bộ hóa lên server!\nBạn chắc chứ ?", "Xác nhận", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.No)
			{
				return;
			}

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

			MainForm.transactionViewForm.Mark();
			int count = MainForm.transactionViewForm.GetCount();

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

			MainForm.transactionViewForm.CheckAll();
			MainForm.homeForm.SetTransactionToServer(count);

			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "";
			});
			lblLastSync.Invoke((MethodInvoker)delegate
			{
				DateTime? lastSync = XmlSync.GetLastSync();
				if (lastSync != null)
				{
					lblLastSync.Text = XmlSync.GetLastSync().Value.ToString("dd/MM/yyyy HH:mm:ss tt");
				}
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

			MainForm.transactionViewForm.Remark();

			lblLastSync.Invoke((MethodInvoker)delegate
			{
				DateTime? lastSync = XmlSync.GetLastSync();
				if (lastSync != null)
				{
					lblLastSync.Text = XmlSync.GetLastSync().Value.ToString("dd/MM/yyyy HH:mm:ss tt");
				}
			});
			lblSyncStatus.Invoke((MethodInvoker)delegate
			{
				lblSyncStatus.Text = "Đã đồng bộ hóa xong.";
				Thread.Sleep(2000);
			});
		}

		private void btnAutoSync_Click(object sender, EventArgs e)
		{
			if (!bwSyncTimeCount.IsBusy)
			{
				int min;

				if (!int.TryParse(txtAutoSync.Text, out min))
				{
					MessageBox.Show("Xin hãy điền số phút là số.");
					return;
				}

				bwSyncTimeCount.RunWorkerAsync(min);
				btnAutoSync.Text = "Hủy";
				txtAutoSync.Enabled = false;
			}
			else
			{
				bwSyncTimeCount.CancelAsync();
				btnAutoSync.Text = "Đồng ý";

				lblAutoSyncTimeCount.Invoke((MethodInvoker)delegate
				{
					lblAutoSyncTimeCount.Text = "";
				});
				txtAutoSync.Enabled = true;
			}
		}
	}
}
