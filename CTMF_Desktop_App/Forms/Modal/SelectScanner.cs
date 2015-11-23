using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms.Modal
{
	public partial class SelectScanner : Form
	{
		private const int NewScannerAddr = 1000;

		public int scannerPort { get; set; }
		public uint scannerAddr { get; set; }

		public SelectScanner()
		{
			InitializeComponent();

			//get list ports
			string[] ports = SerialPort.GetPortNames();
			lbPort.BeginUpdate();
			for (int i = 0; i < ports.Length; i++)
			{
				lbPort.Items.Add(ports[i]);
			}
			lbPort.EndUpdate();

			scannerPort = -1;
			scannerAddr = 0;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string portName = lbPort.GetItemText(lbPort.SelectedItem);
			if (portName == null || portName == string.Empty)
			{
				MessageBox.Show(string.Format(StringResource.A00004, "cổng kết nối"));
				return;
			}

			//get port number
			int portNum;
			if (!int.TryParse(portName.Substring(3), out portNum))
			{
				MessageBox.Show(string.Format(StringResource.E00002, "Port name"));
				return;
			}

			//open scanner
			uint scannerAddress;
			try{
				scannerAddress = DeviceControl.connectScanner(portNum, null);
			}
			catch
			{
				MessageBox.Show(string.Format(StringResource.A00006, "máy quét"));
				return;
			}
			MessageBox.Show(string.Format(StringResource.A00005, "máy quét"));
			scannerAddr = scannerAddress;
			scannerPort = portNum;

			this.Close();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			lbPort.Items.Clear();

			string[] ports = SerialPort.GetPortNames();
			lbPort.BeginUpdate();
			for (int i = 0; i < ports.Length; i++)
			{
				lbPort.Items.Add(ports[i]);
			}
			lbPort.EndUpdate();

			scannerPort = -1;
			scannerAddr = 0;
		}
	}
}
