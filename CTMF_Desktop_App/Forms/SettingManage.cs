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
	public partial class lblLastSync : Form
	{
		public lblLastSync()
		{
			InitializeComponent();
		}

		private void btnResync_Click(object sender, EventArgs e)
		{
			try
			{
				DataAccess.NewSync();
				MessageBox.Show("Done");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra.");
				Log.ErrorLog(ex.Message);
			}
		}

		private void btnSync_Click(object sender, EventArgs e)
		{
			try
			{
				DataAccess.StartSync();
				MessageBox.Show("Done");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra.");
				Log.ErrorLog(ex.Message);
			}
		}
	}
}
