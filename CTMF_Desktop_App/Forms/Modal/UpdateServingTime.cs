using CTMF_Desktop_App.DataAccessTableAdapters;
using CTMF_Desktop_App.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CTMF_Desktop_App.Forms.Modal
{
	public partial class UpdateServingTime : Form
	{
		private int? _scheduleID;
		internal int? scheduleID;

		DateTime now = DateTime.Now;

		public UpdateServingTime(int? scheduleID)
		{
			InitializeComponent();

			txtDate.Text = now.ToString("dd/MM/yyyy");

			this.scheduleID = scheduleID;

			try
			{
				ServingTimeTableAdapter servingTimeTA = new ServingTimeTableAdapter();
				DataTable servingTimeDT = servingTimeTA.GetData();

				List<KeyValuePair<int, string>> servingTimeDdlDS = new List<KeyValuePair<int, string>>();
				int? closestServingTimeID = null;
				TimeSpan? closestTime = null;
				TimeSpan timeOfDay = DateTime.Now.TimeOfDay;

				foreach (DataRow row in servingTimeDT.Rows)
				{
					int servingTimeID = row.Field<int>("ServingTimeID");
					string servingTimeName = row.Field<string>("Name");

					servingTimeDdlDS.Add(new KeyValuePair<int, string>(servingTimeID, servingTimeName));

					TimeSpan startTime = row.Field<TimeSpan>("StartTime");
					TimeSpan timeBetween = startTime - timeOfDay;
					if (timeBetween > TimeSpan.Zero)
					{
						if (closestTime == null || closestTime > timeBetween)
						{
							closestTime = timeBetween;
							closestServingTimeID = servingTimeID;
							continue;
						}
					}
				}

				cbxServingTime.DataSource = servingTimeDdlDS;
				cbxServingTime.DisplayMember = "Value";
				cbxServingTime.ValueMember = "Key";
				if (closestServingTimeID != null)
				{
					cbxServingTime.SelectedValue = closestServingTimeID.Value;
				}
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				MessageBox.Show("Có lỗi khi lấy dữ liệu.");
			}
		}

		private void btnCheckScheduleMealSet_Click(object sender, EventArgs e)
		{
			dataGridView.Rows.Clear();

			int servingTimeID;
			if (!int.TryParse(cbxServingTime.SelectedValue.ToString(), out servingTimeID))
			{
				MessageBox.Show("Không thể lấy dữ liệu bữa ăn.");
				return;
			}

			DataTable schedule = new ScheduleTableAdapter().GetDataByDateAndServingTime(now.Date, servingTimeID);
			if (schedule.Rows.Count != 1)
			{
				this._scheduleID = null;
				MessageBox.Show("Không tìm thấy lịch ăn cho bữa ăn này.");
				btnOk.Enabled = false;
				return;
			}
			else if (!schedule.Rows[0].Field<bool>("IsDayOn"))
			{
				this._scheduleID = null;
				MessageBox.Show("Bữa ăn này đang được đặt là nghỉ.");
				btnOk.Enabled = false;
				return;
			}

			//this._scheduleID = scheduleID;
			this._scheduleID = schedule.Rows[0].Field<int>("ScheduleID");

			dataGridView.ColumnCount = 2;
			dataGridView.Columns[0].Name = "Mã suất";
			dataGridView.Columns[1].Name = "Tên suất";

			int labelChar = 65;

			try
			{
				MealSetInScheduleDetailTableAdapter mealSetInScheduleDetailTA = new MealSetInScheduleDetailTableAdapter();
				DataTable dt = mealSetInScheduleDetailTA.GetDataByScheduleID(this._scheduleID.Value);

				if (dt.Rows.Count == 0)
				{
					MessageBox.Show("Chưa có suất ăn nào đươc đặt trong bữa này.");
				}
				else
				{
					foreach (DataRow row in dt.Rows)
					{
						string[] rowValue = new string[] {
							((char)labelChar).ToString(),
							row.Field<string>("Name")
						};
						dataGridView.Rows.Add(rowValue);

						labelChar++;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi lấy dữ liệu.");
				Log.ErrorLog(ex.Message);
				btnOk.Enabled = false;
			}

			btnOk.Enabled = true;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (_scheduleID == null)
			{
				scheduleID = _scheduleID;
			}
			else
			{
				if (_scheduleID != null)
				{
					scheduleID = _scheduleID;
				}
			}
			this.Dispose();
		}
	}
}