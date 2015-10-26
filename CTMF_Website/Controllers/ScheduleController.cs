using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class ScheduleController : Controller
	{

		[AllowAnonymous]
		public ActionResult Schedule()
		{
			return View();
		}

		[AllowAnonymous]
		public JsonResult GetDateRange()
		{
			ScheduleTableAdapter scheduleAdapter = new ScheduleTableAdapter();

			DateTime? maxDate = null;
			DateTime? minDate = null;
			try
			{
				maxDate = scheduleAdapter.GetMaxDate();
				minDate = scheduleAdapter.GetMinDate();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}

			IList<string> dateMinMax = new List<string>();
			dateMinMax.Add(minDate.Value.ToString("yyyy-MM"));
			dateMinMax.Add(maxDate.Value.ToString("yyyy-MM"));
			dateMinMax.Add(DateTime.Now.ToString("yyyy-MM"));

			return Json(dateMinMax, JsonRequestBehavior.AllowGet);
		}

		[AllowAnonymous]
		public JsonResult GetScheduleData(int selectedMonth, int selectedYear)
		{
			ScheduleMonthForUserTableAdapter adapter = new ScheduleMonthForUserTableAdapter();
			DataTable dt = null;
			try
			{
				dt = adapter.GetData("dungnmse02767", selectedMonth, selectedYear);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}

			int numberOfDay = DateTime.DaysInMonth(selectedYear, selectedMonth);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < numberOfDay; i++)
			{
				sb.Append("N");
			}

			DateTime date;
			bool isDayOn;
			int transactionHistoryID;
			DataRow row = null;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				row = dt.Rows[i];

				date = (DateTime)row["Date"];
				isDayOn = (bool)row["IsDayOn"];
				if (row["TransactionHistoryID"].ToString() == String.Empty)
				{
					transactionHistoryID = -1;
				}
				else
				{
					transactionHistoryID = Convert.ToInt32(row["TransactionHistoryID"]);
				}

				int day = date.Day;
				day--;
				char ch = sb[day];
				if (ch == 'N')
				{
					if (isDayOn)
					{
						sb[day] = '0';
						if (transactionHistoryID > 0)
						{
							sb[day] = '1';
						}
					}
					else
					{
						sb[day] = 'X';
					}
				}
				else if (ch == 'X')
				{
					if (isDayOn)
					{
						sb[day] = '0';
						if (transactionHistoryID > 0)
						{
							sb[day] = '1';
						}
					}
				}
				else
				{
					if (isDayOn)
					{
						if (transactionHistoryID > 0)
						{
							int eatTime = int.Parse(sb[day].ToString());
							sb[day] = eatTime.ToString()[0];
						}
					}
				}
			}

			return Json(new { value = sb.ToString() }, JsonRequestBehavior.AllowGet);
		}
	}
}
