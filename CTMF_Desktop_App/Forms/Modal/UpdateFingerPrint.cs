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
using SourceAFIS.Simple;
using System.Windows.Media.Imaging;

namespace CTMF_Desktop_App.Forms.Modal
{
	public partial class UpdateFingerPrint : Form
	{
		public string fingerImageFile;
		public int fingerPostition;

		private uint _scannerAddr;

		private Person _person1;
		private Person _person2;

		private static AfisEngine afis;

		public UpdateFingerPrint(uint scannerAddr, string username)
		{
			InitializeComponent();
			lblStatus.Text = String.Empty;
			lblTargetUsername.Text = "Tên người dùng: " + username;

			fingerImageFile = null;

			_person1 = null;
			_person2 = null;

			afis = new AfisEngine();

			cbxFingerPosition.DataSource = FingerPositionDictionary.getList();
			cbxFingerPosition.DisplayMember = "Value";
			cbxFingerPosition.ValueMember = "Key";

			cbxFingerPosition.SelectedIndex = 5;

			_scannerAddr = scannerAddr;

			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				Thread.Sleep(500);

				string path = Application.StartupPath;
				BackgroundWorker bw = sender as BackgroundWorker;

				while (true)
				{
					if (_person1 == null && _person2 == null)
					{
						changeLabel("Hãy đặt ngón tay vào máy quét.");

						_person1 = new Person();

						string IMGPath = DeviceControl.getImage(_scannerAddr, bw);

						if (IMGPath == null)
						{
							return;
						}

						Fingerprint fp = new Fingerprint();
						fp.AsBitmapSource = new BitmapImage(new Uri(IMGPath, UriKind.RelativeOrAbsolute));

						_person1.Fingerprints.Add(fp);
						afis.Extract(_person1);

						panel1.BackgroundImage = Image.FromFile(IMGPath);
					}
					else if (_person1 != null && _person2 == null)
					{
						changeLabel("Hãy đặt ngón tay vào máy quét.");

						_person2 = new Person();

						string IMGPath = DeviceControl.getImage(_scannerAddr, bw);

						if (IMGPath == null)
						{
							return;
						}

						changeLabel("Đang so sánh.");

						Fingerprint fp = new Fingerprint();
						fp.AsBitmapSource = new BitmapImage(new Uri(IMGPath, UriKind.RelativeOrAbsolute));

						_person2.Fingerprints.Add(fp);
						afis.Extract(_person2);

						float score = afis.Verify(_person1, _person2);
						if (score < 25)
						{
							_person1 = _person2;
							_person2 = null;

							panel1.BackgroundImage = Image.FromFile(IMGPath);

							changeLabel("Không khớp");
							Thread.Sleep(2000);
						}
						else
						{
							fingerImageFile = IMGPath;

							string selectedValue = "5";
							cbxFingerPosition.Invoke((MethodInvoker)delegate
							{
								selectedValue = cbxFingerPosition.SelectedValue.ToString();
							});

							fingerPostition = int.Parse(selectedValue);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi lấy và so sánh ảnh.");
				Log.ErrorLog(ex.Message);
				return;
			}
		}

		private void changeLabel(string text)
		{
			lblStatus.Invoke((MethodInvoker)delegate
			{
				lblStatus.Text = text;
			});
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.Close();
		}

		private void UpdateFingerPrint_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (backgroundWorker.IsBusy)
			{
				backgroundWorker.CancelAsync();
				e.Cancel = true;
			}
		}
	}
}
