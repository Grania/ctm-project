using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms.Modal
{
	public partial class AddDevice : Form
	{
		IList<Device> devices;

		bool isScannerConnected = false;
		bool isDisplayConnected = false;
		bool isDone = false;

		SerialPort mySerial;

		public AddDevice()
		{
			InitializeComponent();

			btnDone.Enabled = false;

			mySerial = new SerialPort();
			mySerial.BaudRate = 9600;
			mySerial.ReadTimeout = 3000;

			//get list ports
			string[] ports = SerialPort.GetPortNames();
			lbPort.BeginUpdate();
			for (int i = 0; i < ports.Length; i++)
			{
				lbPort.Items.Add(ports[i]);
			}
			lbPort.EndUpdate();
		}

		public AddDevice(IList<Device> devices)
			: this()
		{
			this.devices = devices;
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			lbPort.Items.Clear();

			//get list ports
			string[] ports = SerialPort.GetPortNames();
			lbPort.SelectionMode = SelectionMode.One;
			lbPort.BeginUpdate();
			for (int i = 0; i < ports.Length; i++)
			{
				lbPort.Items.Add(ports[i]);
			}
			lbPort.EndUpdate();

			if (mySerial.IsOpen)
			{
				mySerial.Close();
			}

			isScannerConnected = false;
			isDisplayConnected = false;

			btnConScanner.Enabled = true;
			btnConDisplay.Enabled = true;
			btnDone.Enabled = false;
		}

		private void btnConScanner_Click(object sender, EventArgs e)
		{
			string portName = lbPort.GetItemText(lbPort.SelectedItem);
			if (portName == null || portName == string.Empty)
			{
				MessageBox.Show(string.Format(StringResources.A00004, "cổng kết nối"));
				return;
			}

			//get port number
			int portNum;
			if (!int.TryParse(portName.Substring(3), out portNum))
			{
				MessageBox.Show(string.Format(StringResources.E00002, "Port name"));
				return;
			}

			//open scanner
			if (!DeviceControl.connectScanner(portNum))
			{
				MessageBox.Show(string.Format(StringResources.A00006, "máy quét"));
				return;
			}
			MessageBox.Show(string.Format(StringResources.A00005, "máy quét"));
			isScannerConnected = true;
			btnConScanner.Enabled = false;

			//enable done btn
			if (isDisplayConnected)
			{
				btnDone.Enabled = true;
			}
		}

		private void btnConDisplay_Click(object sender, EventArgs e)
		{
			string portName = lbPort.GetItemText(lbPort.SelectedItem);
			if (portName == null || portName == string.Empty)
			{
				MessageBox.Show(string.Format(StringResources.A00004, "cổng kết nối"));
				return;
			}

			mySerial.PortName = portName;
			mySerial.Open();

			isDisplayConnected = DeviceControl.connectDisplay(mySerial);
			if (!isDisplayConnected)
			{
				MessageBox.Show(string.Format(StringResources.A00006, "màn hình"));
				mySerial.Close();
				return;
				
			}
			MessageBox.Show(string.Format(StringResources.A00005, "màn hình"));
			btnConDisplay.Enabled = false;

			//enable done btn
			if (isScannerConnected)
			{
				btnDone.Enabled = true;
			}
		}

		private void AddDevice_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!isDone)
			{
				DeviceControl.close(mySerial);
			}
		}

		private void btnDone_Click(object sender, EventArgs e)
		{
			devices.Add(new Device(mySerial, 0xffffffff));

			isDone = true;
			this.Close();
		}
	}
}
