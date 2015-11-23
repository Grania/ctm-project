using CTMF_Desktop_App.Forms.Modal;
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
using System.Configuration;
using SourceAFIS.Simple;
using CTMF_Desktop_App.Forms.ExtentionClass;
using CTMF_Desktop_App.DataAccessTableAdapters;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;

namespace CTMF_Desktop_App.Forms
{
	public partial class DeviceManage : Form
	{
		IList<DeviceModel> devices;
		IList<Panel> pnlDeviceList;
		public static IList<Customer> fingerPrintDB;
		BackgroundWorker bwLoadFingerPrints;

		static AfisEngine afis;

		public DeviceManage()
		{
			InitializeComponent();

			afis = new AfisEngine();
			afis.Threshold = 25;

			fingerPrintDB = null;

			bwLoadFingerPrints = new BackgroundWorker();
			bwLoadFingerPrints.DoWork += new DoWorkEventHandler(bwLoadFingerPrints_DoWork);
			bwLoadFingerPrints.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadFingerPrints_RunWorkerCompleted);

			//invisible the HorizontalScroll
			this.flpnlDevices.HorizontalScroll.Maximum = 0;
			this.flpnlDevices.AutoScroll = false;
			this.flpnlDevices.VerticalScroll.Visible = false;
			this.flpnlDevices.AutoScroll = true;

			devices = new List<DeviceModel>();
			pnlDeviceList = new List<Panel>();


			Panel panel1 = new Panel();
			Panel panel2 = new Panel();

			panel2.Size = new Size(flpnlDevices.Width - 25, 100);
			panel1.Size = new Size(flpnlDevices.Width - 25, 100);

			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel2.BorderStyle = BorderStyle.FixedSingle;

			Label lblDeviceNumber = new Label();

			lblDeviceNumber.Text = "Thiết bị số 1";
			lblDeviceNumber.Location = new Point(10, 10);
			lblDeviceNumber.Size = new Size(80, 15);
			//lblDeviceNumber.BorderStyle = BorderStyle.FixedSingle;

			Label lblDeviceCOMNumber = new Label();

			lblDeviceCOMNumber.Text = "Cổng màn hình: COM8";
			lblDeviceCOMNumber.Location = new Point(10, 30);
			lblDeviceCOMNumber.Size = new Size(140, 15);
			//lblDeviceCOMNumber.BorderStyle = BorderStyle.FixedSingle;

			Label lblDeviceAddress = new Label();

			lblDeviceAddress.Text = "Địa chỉ máy quét: 4000000000";
			lblDeviceAddress.Location = new Point(10, 50);
			lblDeviceAddress.Size = new Size(200, 15);
			lblDeviceAddress.BorderStyle = BorderStyle.FixedSingle;

			Label lblDevicePanelStatusLabel = new Label();

			lblDevicePanelStatusLabel.Text = "Trạng thái: ";
			lblDevicePanelStatusLabel.Location = new Point(10, 70);
			lblDevicePanelStatusLabel.Size = new Size(60, 15);
			//lblDevicePanelStatusLabel.BorderStyle = BorderStyle.FixedSingle;

			Label lblDevicePanelStatus = new Label();

			lblDevicePanelStatus.ForeColor = Color.Green;
			lblDevicePanelStatus.Text = "Nghỉ";
			lblDevicePanelStatus.Location = new Point(70, 70);
			lblDevicePanelStatus.Size = new Size(200, 15);
			//lblDevicePanelStatus.BorderStyle = BorderStyle.FixedSingle;

			Button btnStart = new Button();

			btnStart.Anchor = (AnchorStyles.Right);
			btnStart.Text = "Bắt đầu";
			btnStart.Location = new Point(panel1.Size.Width - 120, 10);
			btnStart.Size = new Size(100, 25);
			btnStart.UseVisualStyleBackColor = true;
			btnStart.Click += new EventHandler(btnStart_Click);

			Button btnRemove = new Button();

			btnRemove.Anchor = AnchorStyles.Right;
			btnRemove.Text = "Loại bỏ";
			btnRemove.Location = new Point(panel1.Size.Width - 120, 50);
			btnRemove.Size = new Size(100, 25);
			btnRemove.UseVisualStyleBackColor = true;

			panel1.Controls.Add(lblDevicePanelStatusLabel);
			panel1.Controls.Add(lblDevicePanelStatus);
			panel1.Controls.Add(lblDeviceNumber);
			panel1.Controls.Add(lblDeviceCOMNumber);
			panel1.Controls.Add(lblDeviceAddress);

			panel1.Controls.Add(btnStart);
			panel1.Controls.Add(btnRemove);

			flpnlDevices.Controls.Add(panel1);
			flpnlDevices.Controls.Add(panel2);

			pnlDeviceList.Add(panel1);
			pnlDeviceList.Add(panel2);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (bwLoadFingerPrints.IsBusy)
			{
				MessageBox.Show("Xin hãy đợi hệ thống tại xong ảnh.");
				return;
			}

			Panel devicePanel = ((Button)sender).Parent as Panel;
			DeviceModel device = null;
			Label lblStatus = null;

			foreach (Control con in devicePanel.Controls)
			{
				if (con is Label)
				{
					if (con.Name == "DeviceName")
					{
						string name = con.Text;

						foreach (DeviceModel d in devices)
						{
							if (d.name == name)
							{
								device = d;
								break;
							}
						}
					}
					else if(con.Name == "DeviceStatus")
					{
						lblStatus = con as Label;
					}
					else if (device != null && lblStatus != null)
					{
						break;
					}
				}
			}

			if (device == null || lblStatus == null)
			{
				MessageBox.Show("Không kết nối được với thiết bị.");
				Log.ErrorLog("Device name not match in list of device.");
				return;
			}

			if (fingerPrintDB == null)
			{
				bwLoadFingerPrints.RunWorkerAsync(sender);
				return;
			}

			if (!device.isValid())
			{
				MessageBox.Show("Thiết bị này không đủ điều kiện.");
				return;
			}

			if (device.backgroundWokder == null)
			{
				BackgroundWorker bw = new BackgroundWorker();
				bw.WorkerSupportsCancellation = true;

				bw.DoWork += new DoWorkEventHandler(bwStart_DoWork);
				bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwComplete_RunWorkerCompleted);

				device.backgroundWokder = bw;
				bw.RunWorkerAsync(device);

				((Button)sender).Text = "Dừng lại";
				lblStatus.ForeColor = Color.Red;
				lblStatus.Text = "Đang hoạt động";
			}
			else if (device.backgroundWokder.IsBusy)
			{
				device.backgroundWokder.CancelAsync();

				((Button)sender).Text = "Tiếp tục";
				lblStatus.ForeColor = Color.Green;
				lblStatus.Text = "Nghỉ";
			}
			else
			{
				device.backgroundWokder.RunWorkerAsync(device);

				((Button)sender).Text = "Dừng lại";
				lblStatus.ForeColor = Color.Red;
				lblStatus.Text = "Đang hoạt động";
			}
		}

		private void bwStart_DoWork(object sender, DoWorkEventArgs e)
		{
			DeviceModel device = e.Argument as DeviceModel;

			try
			{
				string path = Application.StartupPath;
				BackgroundWorker bw = device.backgroundWokder;

				DeviceControl.setLCDText(device.serial, "Hay dat ngon tay\nvao may quet");
				while (true)
				{
					Customer cus = new Customer();

					string IMGPath = DeviceControl.getImage(device.scannerAddress, bw);

					if (bw.CancellationPending)
					{
						DeviceControl.setLCDText(device.serial, "Da dung lai.");
					}

					if (IMGPath == null)
					{
						return;
					}

					Fingerprint fp = new Fingerprint();
					fp.AsBitmapSource = new BitmapImage(new Uri(IMGPath, UriKind.RelativeOrAbsolute));

					cus.Fingerprints.Add(fp);
					afis.Extract(cus);

					Customer matchCus = afis.Identify(cus, fingerPrintDB).FirstOrDefault() as Customer;

					if (matchCus == null)
					{
						DeviceControl.setLCDText(device.serial, "Khong tim thay\nvan tay phu hop");
						Thread.Sleep(1500);
					}
					else
					{
						string alert = DataAccess.PayForFood(matchCus);

						DeviceControl.setLCDText(device.serial, matchCus.TypetShortName + ":" + matchCus.Username
							+ "\n" + alert);

						Thread.Sleep(1500);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi lấy vân tay.");
				Log.ErrorLog(ex.Message);
				return;
			}
		}

		private void bwComplete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{

		}

		private void btnAddDevice_Click(object sender, EventArgs e)
		{
			int deviceCount = devices.Count;
			new AddDevice(devices).ShowDialog();
			if (devices.Count > deviceCount)
			{
				DeviceModel device = devices[devices.Count - 1];

				Panel devicePanel = new Panel();

				devicePanel.Size = new Size(flpnlDevices.Width - 25, 100);
				devicePanel.BorderStyle = BorderStyle.FixedSingle;

				Label lblDeviceNumber = new Label();

				lblDeviceNumber.Text = device.name;
				lblDeviceNumber.Name = "DeviceName";
				lblDeviceNumber.Location = new Point(10, 10);
				lblDeviceNumber.Size = new Size(80, 15);
				//lblDeviceNumber.BorderStyle = BorderStyle.FixedSingle;

				Label lblDeviceCOMNumber = new Label();

				lblDeviceCOMNumber.Text = "Cổng màn hình: " + device.serial.PortName;
				lblDeviceCOMNumber.Location = new Point(10, 30);
				lblDeviceCOMNumber.Size = new Size(140, 15);
				//lblDeviceCOMNumber.BorderStyle = BorderStyle.FixedSingle;

				Label lblDeviceAddress = new Label();

				lblDeviceAddress.Text = "Địa chỉ máy quét: " + device.scannerAddress;
				lblDeviceAddress.Location = new Point(10, 50);
				lblDeviceAddress.Size = new Size(200, 15);
				lblDeviceAddress.BorderStyle = BorderStyle.FixedSingle;

				Label lblDevicePanelStatusLabel = new Label();

				lblDevicePanelStatusLabel.Text = "Trạng thái: ";
				lblDevicePanelStatusLabel.Location = new Point(10, 70);
				lblDevicePanelStatusLabel.Size = new Size(60, 15);
				//lblDevicePanelStatusLabel.BorderStyle = BorderStyle.FixedSingle;

				Label lblDevicePanelStatus = new Label();

				lblDevicePanelStatus.ForeColor = Color.Green;
				lblDevicePanelStatus.Text = "Nghỉ";
				lblDevicePanelStatus.Name = "DeviceStatus";
				lblDevicePanelStatus.Location = new Point(70, 70);
				lblDevicePanelStatus.Size = new Size(200, 15);
				//lblDevicePanelStatus.BorderStyle = BorderStyle.FixedSingle;

				Button btnStart = new Button();

				btnStart.Anchor = (AnchorStyles.Right);
				btnStart.Text = "Bắt đầu";
				btnStart.Location = new Point(devicePanel.Size.Width - 120, 10);
				btnStart.Size = new Size(100, 25);
				btnStart.UseVisualStyleBackColor = true;
				btnStart.Click += new EventHandler(btnStart_Click);

				Button btnRemove = new Button();

				btnRemove.Anchor = AnchorStyles.Right;
				btnRemove.Text = "Loại bỏ";
				btnRemove.Location = new Point(devicePanel.Size.Width - 120, 50);
				btnRemove.Size = new Size(100, 25);
				btnRemove.UseVisualStyleBackColor = true;

				devicePanel.Controls.Add(lblDevicePanelStatusLabel);
				devicePanel.Controls.Add(lblDevicePanelStatus);
				devicePanel.Controls.Add(lblDeviceNumber);
				devicePanel.Controls.Add(lblDeviceCOMNumber);
				devicePanel.Controls.Add(lblDeviceAddress);

				devicePanel.Controls.Add(btnStart);
				devicePanel.Controls.Add(btnRemove);

				pnlDeviceList.Add(devicePanel);
				flpnlDevices.Controls.Add(devicePanel);
			}
		}

		private void bwLoadFingerPrints_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done");
			btnStart_Click(e.Result, new EventArgs());
		}

		private void bwLoadFingerPrints_DoWork(object sender, DoWorkEventArgs e)
		{
			if (fingerPrintDB != null)
			{
				e.Cancel = true;
				return;
			}
			fingerPrintDB = new List<Customer>();

			if (AccountManage.customerDT == null)
			{
				AccountManage.customerDT = new CustomerTableAdapter().GetData();
			}
			foreach (DataRow row in AccountManage.customerDT.Rows)
			{
				if (row.Field<bool>("IsActive"))
				{
					Customer customer = new Customer()
					{
						Username = row.Field<string>("Username"),
						TypetShortName = row.Field<string>("TypeShortName"),
						MealValue = row.Field<int>("MealValue"),
						CanDebt = row.Field<bool>("CanDebt"),
						CanEatMore = row.Field<bool>("CanEatMore"),
						MoreMealValue = row.Field<int?>("MoreMealValue")
					};
					Fingerprint fp = new Fingerprint();
					MemoryStream ms = new MemoryStream(row.Field<byte[]>("FingerPrintIMG"));
					Image returnImage = Image.FromStream(ms);
					fp.AsBitmap = (Bitmap)returnImage;
					customer.Fingerprints.Add(fp);
					afis.Extract(customer);

					fingerPrintDB.Add(customer);
				}
			}

			e.Result = e.Argument;
		}

		private void flpnlDevices_MouseEnter(object sender, EventArgs e)
		{
			flpnlDevices.Focus();
		}

		private void flpnlDevices_SizeChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < pnlDeviceList.Count; i++)
			{
				pnlDeviceList[i].Width = flpnlDevices.Width - 25;
			}
		}

		private void DeviceManager_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//DataAccess.SyncData();
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

		private void button2_Click(object sender, EventArgs e)
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
