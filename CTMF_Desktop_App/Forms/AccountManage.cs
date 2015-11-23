using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTMF_Desktop_App.DataAccessTableAdapters;
using CTMF_Desktop_App.Forms.Modal;
using CTMF_Desktop_App.Util;
using SourceAFIS;

namespace CTMF_Desktop_App.Forms
{
	public partial class AccountManage : Form
	{
		private static bool _isLoaded = false;
		private static int _usingPortNumber = -1;
		private static uint _scannerAddr = 0;
		public static DataTable customerDT;
		public static DataTable userTypeDT;

		public AccountManage()
		{
			InitializeComponent();
		}

		public void LoadForm()
		{
			if (_isLoaded)
			{
				return;
			}

			try
			{
				lblSendReport.Text = String.Empty;

				UserTypeTableAdapter userTypeTA = new UserTypeTableAdapter();

				userTypeDT = userTypeTA.GetData();
				if (userTypeDT.Rows.Count > 0)
				{
					List<KeyValuePair<string, string>> userTypeDS = new List<KeyValuePair<string, string>>();

					userTypeDS.Add(new KeyValuePair<string, string>("", "<<Tất cả>>"));

					string typeName;
					string typeShortName;
					foreach (DataRow row in userTypeDT.Rows)
					{
						typeName = row.Field<string>("TypeName");
						typeShortName = row.Field<string>("TypeShortName");
						userTypeDS.Add(new KeyValuePair<string, string>(typeShortName, typeName));
					}

					cbxUserTypeSearch.DataSource = userTypeDS;
					cbxUserTypeSearch.DisplayMember = "Value";
					cbxUserTypeSearch.ValueMember = "Key";
				}

				CustomerTableAdapter customerTA = new CustomerTableAdapter();

				if (customerDT == null)
				{
					customerDT = customerTA.GetData();
				}
				dataGridView.DataSource = customerDT;
				CustomGrid(dataGridView);

				List<KeyValuePair<int, string>> activeList = new List<KeyValuePair<int, string>>();
				activeList.Add(new KeyValuePair<int, string>(-1, "<<Tất cả>>"));
				activeList.Add(new KeyValuePair<int, string>(1, "Có"));
				activeList.Add(new KeyValuePair<int, string>(0, "Không"));

				cbxActive.DataSource = activeList;
				cbxActive.DisplayMember = "Value";
				cbxActive.ValueMember = "Key";

				_isLoaded = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra, không thể tải dữ liệu.");
				Log.ErrorLog(ex.Message);
			}
		}

		private void btnUpdateFingerPrint_Click(object sender, EventArgs e)
		{
			if (dataGridView.SelectedRows.Count != 1)
			{
				MessageBox.Show("Xin hãy chọn người dùng trước.");
				return;
			}

			if (bwSend.IsBusy)
			{
				MessageBox.Show("Xin hãy đợi hệ thống hoàn thành việc gửi ảnh.");
				return;
			}

			if (_usingPortNumber < 0)
			{
				using (var form = new SelectScanner())
				{
					var result = form.ShowDialog();
					_usingPortNumber = form.scannerPort;
					_scannerAddr = form.scannerAddr;
				}

				if (_usingPortNumber < 0 || _scannerAddr == 0)
				{
					MessageBox.Show("Không thể kết nối tới máy quét.");
					return;
				}

				DeviceControl.deviceList.Add(new DeviceModel
				{
					serial = null,
					scannerAddress = _scannerAddr,
					isForEating = false
				});
			}

			string username = dataGridView.SelectedRows[0].Cells["Username"].Value.ToString();
			string imageFile;
			int? fingerPosition;
			DateTime? lastUpdatedFingerPrint;

			using (var form = new UpdateFingerPrint(_scannerAddr, username))
			{
				var result = form.ShowDialog();
				imageFile = form.fingerImageFile;
				fingerPosition = form.fingerPostition;
			}

			if (imageFile != null)
			{
				Image savedImage = Image.FromFile(imageFile);
				ImageConverter converter = new ImageConverter();
				byte[] fingerImage = (byte[])converter.ConvertTo(savedImage, typeof(byte[]));

				lastUpdatedFingerPrint = DateTime.Now;

				DataRow userInfoRow = null;
				foreach (DataRow row in customerDT.Rows)
				{
					if (row["Username"].ToString() == username)
					{
						row["FingerPrintIMG"] = fingerImage;
						row["FingerPosition"] = fingerPosition;
						row["LastUpdatedFingerPrint"] = lastUpdatedFingerPrint;

						userInfoRow = row;
						break;
					}
				}

				dataGridView.DataSource = customerDT;

				UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
				userInfoTA.UpdateFingerPrint(fingerImage, lastUpdatedFingerPrint, fingerPosition, username);

				if (cbSendToServer.Checked)
				{
					lblSendReport.Text = "Đang gửi hình ảnh lên server.";

					string paramUsername = username;
					byte[] paramFingerImage = fingerImage;
					int? paramFingerPosition = fingerPosition;
					DateTime paramLastUpdatedFingerPrint = lastUpdatedFingerPrint.Value;
					DataRow paramUserInfoRow = userInfoRow;

					object[] parameters = new object[] { paramUsername, paramFingerImage, paramFingerPosition
						, paramLastUpdatedFingerPrint, paramUserInfoRow};

					bwSend.RunWorkerAsync(parameters);
				}
				else
				{
					XmlSync.SaveUserInfoXml(userInfoRow.Field<string>("Username"), userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
					, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerImage, lastUpdatedFingerPrint
					, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff"), userInfoRow.Field<bool>("IsActive"), userInfoRow.Field<DateTime>("InsertedDate")
					, userInfoRow.Field<string>("UpdatedBy"), userInfoRow.Field<DateTime>("LastUpdated"));
				}
			}
		}

		private void bwSendToServer_DoWork(object sender, DoWorkEventArgs e)
		{
			object[] parameters = e.Argument as object[];

			string username = (string)parameters[0];
			byte[] fingerImage = (byte[])parameters[1];
			int? fingerPosition = (int?)parameters[2];
			DateTime lastUpdatedFingerPrint = (DateTime)parameters[3];
			DataRow userInfoRow = (DataRow)parameters[4];

			ServiceReference.WebServiceSoapClient soap = new ServiceReference.WebServiceSoapClient();
			bool sendComplete = soap.getUpdatedFingerPrint(WebServiceAuth.AuthSoapHeader(), username, fingerImage, fingerPosition
				, lastUpdatedFingerPrint, XmlSync.GetSyncID());

			if (!sendComplete)
			{
				XmlSync.SaveUserInfoXml(userInfoRow.Field<string>("Username"), userInfoRow.Field<string>("Name"), userInfoRow.Field<string>("TypeShortName")
					, userInfoRow.Field<int>("AmountOfMoney"), userInfoRow.Field<DateTime>("LastUpdatedMoney"), fingerImage, lastUpdatedFingerPrint
					, fingerPosition, userInfoRow.Field<bool>("IsCafeteriaStaff"), userInfoRow.Field<bool>("IsActive"), userInfoRow.Field<DateTime>("InsertedDate")
					, userInfoRow.Field<string>("UpdatedBy"), userInfoRow.Field<DateTime>("LastUpdated"));
				e.Result = false;
			}
			else
			{
				e.Result = true;
			}
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if ((bool)e.Result)
			{
				lblSendReport.Invoke((MethodInvoker)delegate
				{
					lblSendReport.Text = "Đã gửi lên server thành công.";
				});
			}
			else
			{
				lblSendReport.Invoke((MethodInvoker)delegate
				{
					lblSendReport.Text = "Gửi lên server không thành công!";
				});
			}
		}

		private void CustomGrid(DataGridView grid)
		{
			grid.Columns.RemoveAt(4);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);
			grid.Columns.RemoveAt(9);

			grid.Columns[0].HeaderText = "Tên đăng nhập";
			grid.Columns[1].HeaderText = "Tên";
			grid.Columns[2].HeaderText = "Đối tượng";
			grid.Columns[3].HeaderText = "Tên Đối tượng";
			grid.Columns[4].HeaderText = "Vân tay";
			grid.Columns[5].HeaderText = "Vị trí vân tay";
			grid.Columns[6].HeaderText = "Nhân viên nhà bếp";
			grid.Columns[7].HeaderText = "Hoạt động";
			grid.Columns[8].HeaderText = "Ngày tham gia";
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			string username = txtUsernameSearch.Text;
			string name = txtNameSearch.Text;
			string typeShortName = (string)cbxUserTypeSearch.SelectedValue;
			int active = (int)cbxActive.SelectedValue;

			try
			{
				dataGridView.DataSource = null;
				dataGridView.Rows.Clear();
				dataGridView.Columns.Clear();
				dataGridView.DataSource = customerDT.AsEnumerable()
					.Where(r => (String.IsNullOrWhiteSpace(username) ? true
						: StringExtensions.ContainsInsensitive(r.Field<string>("Username"), username))
						&& (String.IsNullOrWhiteSpace(name) ? true
						: StringExtensions.ContainsInsensitive(r.Field<string>("Name"), name))
						&& (String.IsNullOrWhiteSpace(typeShortName) ? true
						: r.Field<string>("TypeShortName") == typeShortName)
						&& (active < 0 ? true : r.Field<int>("IsActive") == active)).CopyToDataTable();

				CustomGrid(dataGridView);
			}
			catch
			{
				dataGridView.Rows.Clear();
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			txtNameSearch.Text = String.Empty;
			txtUsernameSearch.Text = String.Empty;
			cbxActive.SelectedIndex = 0;
			cbxUserTypeSearch.SelectedIndex = 0;

			dataGridView.DataSource = null;
			dataGridView.Rows.Clear();
			dataGridView.Columns.Clear();
			_isLoaded = false;
			LoadForm();
		}
	}
}
