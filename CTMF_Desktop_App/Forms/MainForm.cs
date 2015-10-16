using CTMF_Desktop_App.Forms.Modal;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms
{
	public partial class MainForm : Form
	{
		IList<Device> devices;

		public MainForm()
		{
			InitializeComponent();
			devices = new List<Device>();
			
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void btnAddDevice_Click(object sender, EventArgs e)
		{
			int deviceCount = devices.Count;
			new AddDevice(devices).ShowDialog();
			if (devices.Count > deviceCount)
			{

			}
		}
	}
}
