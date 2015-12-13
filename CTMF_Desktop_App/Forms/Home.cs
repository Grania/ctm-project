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
using System.IO.Ports;

namespace CTMF_Desktop_App.Forms
{
	public partial class Home : Form
	{
		IList<DeviceModel> devices;
		IList<Panel> pnlDeviceList;
		public static IList<Customer> fingerPrintDB;
		public static DateTime? lastUpdatedFingerPrintDB;
		BackgroundWorker bwLoadFingerPrints;

		//Client information
		public static string username;
		public static string loginTime;
		public static string deviceCount;
		public static string userCount;
		public static string fingerPrintCount;
		public static string transactionCount;
		public static string transactionToServerCount;

		public static int? scheduleID;
		IList<KeyValuePair<int, string>> usingMealSetList;

		static AfisEngine afis;

		public Home()
		{
			//init for client info
			username = MainForm.username;
			loginTime = MainForm.loginTime.ToShortTimeString();
			deviceCount = "0";

			try
			{
				UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
				int? userCount_ = userInfoTA.GetCount();
				if (userCount_ == null)
				{
					userCount_ = 0;
				}
				userCount = userCount_.Value.ToString();

				int? fingerPrintCount_ = (int?)userInfoTA.GetUsingFingerPrintCout();
				if (fingerPrintCount_ == null)
				{
					fingerPrintCount_ = 0;
				}
				fingerPrintCount = fingerPrintCount_.Value.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi tải dữu liệu.");
				Log.ErrorLog(ex.Message);
			}

			transactionCount = "0";
			transactionToServerCount = "0";

			InitializeComponent();

			ReloadClientInfo();

			afis = new AfisEngine();
			afis.Threshold = 25;

			fingerPrintDB = null;
			lastUpdatedFingerPrintDB = null;

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
			usingMealSetList = new List<KeyValuePair<int, string>>();

			usingMealSetList.Add(new KeyValuePair<int, string>(0, "<<Không có>>"));
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
			ComboBox cbxMealSet = null;

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
					else if (con.Name == "DeviceStatus")
					{
						lblStatus = con as Label;
					}
				}

				if (con is ComboBox)
				{
					if (con.Name == "MealSetList")
					{
						cbxMealSet = (ComboBox)con;
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

			bool canEatMore = cbxMealSet.Text.Contains("(Có ăn thêm)");
			device.canEatMore = canEatMore;
			int scheduleMealSetDetailID = int.Parse(cbxMealSet.SelectedValue.ToString());
			if (scheduleMealSetDetailID > 0)
			{
				device.scheduleMealSetDetailID = scheduleMealSetDetailID;
				device.mealSetLabel = cbxMealSet.Text[0];
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

				if (device.scheduleMealSetDetailID != null)
				{
					DeviceControl.setLCDText(device.serial, "Phuc vu suat: " + device.mealSetLabel.ToString());
				}
				else
				{
					DeviceControl.setLCDText(device.serial, "Phuc vu suat\ntu do");
				}

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
						string alert = DataAccess.PayForFood(matchCus, device.eatMoreFlag, device.scheduleMealSetDetailID);

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

		private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			SerialPort sp = (SerialPort)sender;

			//lookup for device
			foreach (DeviceModel device in devices)
			{
				if (device.serial.PortName == sp.PortName)
				{
					string receive = sp.ReadLine();
					if (receive == DeviceControl.EatMoreCmd)
					{
						if (!device.canEatMore)
						{
							DeviceControl.setLCDText(sp, "Khong co suat\nan them");
							break;
						}

						lock (device)
						{
							device.eatMoreFlag = !device.eatMoreFlag;
						}

						if (device.eatMoreFlag)
						{
							DeviceControl.setLCDText(sp, "Suat an them.");
						}
						else
						{
							if (device.scheduleMealSetDetailID != null)
							{
								DeviceControl.setLCDText(device.serial, "Phuc vu suat: " + device.mealSetLabel.ToString());
							}
							else
							{
								DeviceControl.setLCDText(device.serial, "Phuc vu suat\ntu do");
							}
						}
					}

					break;
				}
			}
		}

		private void btnAddDevice_Click(object sender, EventArgs e)
		{
			int deviceCount = devices.Count;
			new AddDevice(devices).ShowDialog();

			if (devices.Count > deviceCount)
			{
				DeviceModel device = devices[devices.Count - 1];
				device.serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serial_DataReceived);

				Panel devicePanel = new Panel();

				devicePanel.Size = new Size(flpnlDevices.Width - 25, 120);
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
				//lblDeviceAddress.BorderStyle = BorderStyle.FixedSingle;

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

				Label lblMealSet = new Label();

				lblMealSet.Text = "Suất ăn :";
				lblMealSet.Location = new Point(10, 90);
				lblMealSet.Size = new Size(60, 15);
				//lblDevicePanelStatus.BorderStyle = BorderStyle.FixedSingle;

				ComboBox cbxMealSet = new ComboBox();
				cbxMealSet.DropDownStyle = ComboBoxStyle.DropDownList;
				cbxMealSet.FormattingEnabled = true;
				cbxMealSet.Location = new Point(70, 88);
				cbxMealSet.Size = new Size(200, 15);
				cbxMealSet.Name = "MealSetList";
				cbxMealSet.DataSource = usingMealSetList;
				cbxMealSet.DisplayMember = "Value";
				cbxMealSet.ValueMember = "Key";

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
				devicePanel.Controls.Add(lblMealSet);
				devicePanel.Controls.Add(cbxMealSet);

				devicePanel.Controls.Add(btnStart);
				devicePanel.Controls.Add(btnRemove);

				pnlDeviceList.Add(devicePanel);
				flpnlDevices.Controls.Add(devicePanel);
			}
		}

		private void bwLoadFingerPrints_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done");

			lastUpdatedFingerPrintDB = XmlSync.GetLastSync();

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
				if (row.Field<bool>("IsActive") && row.Field<byte[]>("FingerPrintIMG") != null
					&& row.Field<string>("TypeShortName") != "DF")
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

		public static void LoadCustomerData()
		{
			if (fingerPrintDB == null)
			{
				fingerPrintDB = new List<Customer>();
				lastUpdatedFingerPrintDB = null;
			}

			DataTable customerData;
			if (lastUpdatedFingerPrintDB == null)
			{
				lastUpdatedFingerPrintDB = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
				customerData = new CustomerTableAdapter().GetData();
			}
			else
			{
				customerData = new CustomerTableAdapter().GetDataFromDate(lastUpdatedFingerPrintDB.Value
				, lastUpdatedFingerPrintDB.Value);
			}

			lock (fingerPrintDB)
			{
				foreach (DataRow row in customerData.Rows)
				{
					DateTime lastUpdated = row.Field<DateTime>("LastUpdated");
					DateTime? lastUpdatedFingerPrint = row.Field<DateTime?>("LastUpdatedFingerPrint");

					if (lastUpdated > lastUpdatedFingerPrintDB || lastUpdatedFingerPrint > lastUpdatedFingerPrintDB)
					{
						string username = row.Field<string>("Username");
						bool isActive = row.Field<bool>("IsActive");
						string typeShortName = row.Field<string>("TypeShortName");

						int index = 0;
						bool found = false;
						for (; index < fingerPrintDB.Count; index++)
						{
							if (fingerPrintDB[index].Username == username)
							{
								found = true;
								break;
							}
						}

						if (lastUpdated > lastUpdatedFingerPrintDB)
						{
							if (isActive && typeShortName != Customer.DefaultTypeShortName)
							{
								int mealValue = row.Field<int>("MealValue");
								bool canDebt = row.Field<bool>("CanDebt");
								bool canEatMore = row.Field<bool>("CanEatMore");
								int? moreMealValue = row.Field<int?>("MoreMealValue");

								if (found)
								{
									//update customer
									fingerPrintDB[index].MealValue = mealValue;
									fingerPrintDB[index].CanDebt = canDebt;
									fingerPrintDB[index].CanEatMore = canEatMore;
									fingerPrintDB[index].MoreMealValue = moreMealValue;
								}
								else
								{
									if (lastUpdatedFingerPrint != null)
									{
										//insert customer
										fingerPrintDB.Add(new Customer()
										{
											Username = username,
											TypetShortName = typeShortName,
											MealValue = mealValue,
											CanDebt = canDebt,
											CanEatMore = canEatMore,
											MoreMealValue = moreMealValue
										});
										index = fingerPrintDB.Count - 1;
									}
								}
							}
							else
							{
								if (found)
								{
									//remove customer
									fingerPrintDB.RemoveAt(index);
								}

								continue;
							}
						}

						if (lastUpdatedFingerPrint > lastUpdatedFingerPrintDB)
						{
							Fingerprint fp = new Fingerprint();
							MemoryStream ms = new MemoryStream(row.Field<byte[]>("FingerPrintIMG"));
							Image returnImage = Image.FromStream(ms);
							fp.AsBitmap = (Bitmap)returnImage;

							fingerPrintDB[index].Fingerprints = new List<Fingerprint>();
							fingerPrintDB[index].Fingerprints.Add(fp);
							afis.Extract(fingerPrintDB[index]);

						}
					}
				}

				lastUpdatedFingerPrintDB = XmlSync.GetLastSync();
			}
		}

		internal void ReloadClientInfo()
		{
			lblUsername.Text = username;
			lblLoginTime.Text = loginTime;
			lblDeviceCount.Text = deviceCount;
			lblUserCount.Text = userCount;
			lblFingerPrintCount.Text = fingerPrintCount;
			lblTransactionCount.Text = transactionCount;
			lblTransactionToServerCout.Text = transactionToServerCount;
		}

		private void lblUpdateServingTime_Click(object sender, EventArgs e)
		{
			using (var form = new UpdateServingTime(scheduleID))
			{
				form.ShowDialog();

				if (scheduleID != form.scheduleID)
				{
					scheduleID = form.scheduleID;
				}
			};

			if (scheduleID == null)
			{
				return;
			}

			dgvScheduleMealSet.ColumnCount = 2;
			dgvScheduleMealSet.Columns[0].Name = "Mã suất";
			dgvScheduleMealSet.Columns[1].Name = "Tên suất";

			dgvScheduleMealSet.Columns[0].Width = 100;
			dgvScheduleMealSet.Columns[1].Width = 260;

			int labelChar = 65;

			try
			{
				MealSetInScheduleDetailTableAdapter mealSetInScheduleDetailTA = new MealSetInScheduleDetailTableAdapter();
				DataTable dt = mealSetInScheduleDetailTA.GetDataByScheduleID(scheduleID.Value);

				dgvScheduleMealSet.Rows.Clear();

				usingMealSetList = new List<KeyValuePair<int, string>>();

				if (dt.Rows.Count != 0)
				{
					usingMealSetList.Add(new KeyValuePair<int, string>(0, "<<Không có>>"));

					foreach (DataRow row in dt.Rows)
					{
						string name = row.Field<string>("Name");
						int scheduleMealSetDetailID = row.Field<int>("ScheduleMealSetDetailID");
						bool canEatMore = row.Field<bool>("CanEatMore");
						string displayName = ((char)labelChar).ToString() + ": " + name + " " + (canEatMore ? "(Có ăn thêm)" : "");

						string[] rowValue = new string[] {
							((char)labelChar).ToString(),
							name
						};
						dgvScheduleMealSet.Rows.Add(rowValue);

						usingMealSetList.Add(new KeyValuePair<int, string>(scheduleMealSetDetailID, displayName));

						labelChar++;
					}
				}

				RefreshDropDownList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi lấy dữ liệu.");
				Log.ErrorLog(ex.Message);
			}
		}

		private void RefreshDropDownList()
		{
			foreach (Panel panel in pnlDeviceList)
			{
				foreach (Control con in panel.Controls)
				{
					if (con is ComboBox && con.Name == "MealSetList")
					{
						((ComboBox)con).DataSource = usingMealSetList;
						((ComboBox)con).DisplayMember = "Value";
						((ComboBox)con).ValueMember = "Key";
						break;
					}
				}
			}
		}
	}
}
