using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using CTMF_Website.Models;
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
		public ActionResult Schedule()
		{
			return View();
		}
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

		//[AllowAnonymous]
		public JsonResult GetScheduleData(int selectedMonth, int selectedYear)
		{
			string username = "";
			try
			{
				username = AccountInfo.GetUserName(Request);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			ScheduleMonthForUserTableAdapter adapter = new ScheduleMonthForUserTableAdapter();
			DataTable dt = null;
			try
			{
				dt = adapter.GetData(username, selectedMonth, selectedYear);
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

		//Serving time controller
		[AllowAnonymous]
		public ActionResult ViewServingTime()
		{
			DataTable dataTable = new DataTable();
			ServingTimeTableAdapter servingTimeAdapter = new ServingTimeTableAdapter();

			try
			{
				dataTable = servingTimeAdapter.GetData();
				var results = from DataRow myRow in dataTable.Rows
							  where (int)myRow["ServingTimeID"] != 0
							  select myRow;
				return View(results.CopyToDataTable());
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return View(dataTable);
		}

		[AllowAnonymous]
		public ActionResult EditServingTime(string servingTimeID)
		{
			int servingTimeId = Convert.ToInt32(servingTimeID);
			ServingTimeModel servingTimeModel = new ServingTimeModel();
			DataTable servingTimeDataTable = new DataTable();
			ServingTimeTableAdapter servingTimeAdapter = new ServingTimeTableAdapter();
			try
			{
				servingTimeDataTable = servingTimeAdapter.GetDataByID(servingTimeId);
				servingTimeModel.servingTimeID = servingTimeDataTable.Rows[0].Field<int>("ServingTimeID");
				servingTimeModel.name = Convert.ToString(servingTimeDataTable.Rows[0]["Name"]);
				servingTimeModel.startTime = (TimeSpan)servingTimeDataTable.Rows[0]["StartTime"];
				if (servingTimeDataTable.Rows[0]["EndTime"].ToString() == String.Empty)
				{
					servingTimeModel.endTime = null;
				}
				else
				{
					servingTimeModel.endTime = (TimeSpan?)servingTimeDataTable.Rows[0]["EndTime"];
				}
				servingTimeModel.insertDate = Convert.ToDateTime(servingTimeDataTable.Rows[0]["InsertedDate"]);
				servingTimeModel.lastUpdate = Convert.ToDateTime(servingTimeDataTable.Rows[0]["LastUpdated"]);

			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			if (servingTimeModel == null)
			{
				return RedirectToAction("Error", "ErrorController");
			}
			return View(servingTimeModel);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult EditServingTime(ServingTimeModel servingTimeModel, string servingTimeID)
		{
			int servingTimeId = Convert.ToInt32(servingTimeID);
			ServingTimeTableAdapter servingTimeTableAdapter = new ServingTimeTableAdapter();
			if (servingTimeModel != null)
			{
				try
				{
					string name = servingTimeModel.name;
					TimeSpan startTime = servingTimeModel.startTime;
					TimeSpan? endTime = servingTimeModel.endTime;
					DateTime insertDate = servingTimeModel.insertDate;
					DateTime lastUpdate = DateTime.Now;

					servingTimeTableAdapter.UpdateServingTimeByID(name, startTime, endTime, insertDate, lastUpdate, servingTimeId);
					return RedirectToAction("ViewServingTime", "Schedule");
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}
			return RedirectToAction("ViewServingTime", "Schedule");
		}


		[AllowAnonymous]
		public ActionResult DeleteServingTime(string servingTimeID)
		{
			ServingTimeTableAdapter servingTimeTableAdapter = new ServingTimeTableAdapter();
			ScheduleTableAdapter scheduleTableAdapter = new ScheduleTableAdapter();
			try { 
				int servingTimeId = Convert.ToInt32(servingTimeID);
				//scheduleTableAdapter.DeleteScheduleByServingTimeID(servingTimeId);
				servingTimeTableAdapter.DeleteServingTimeByID(servingTimeId);
			}catch(Exception ex){
				Log.ErrorLog(ex.Message);
			}
			return RedirectToAction("ViewServingTime","Schedule");
		}
		[AllowAnonymous]
		public ActionResult AddNewServingTime()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewServingTime(ServingTimeModel servingTimeModel)
		{
			ServingTimeTableAdapter servingTimeDataAdapter = new ServingTimeTableAdapter();
			if (servingTimeModel != null)
			{
				try
				{
					string name = servingTimeModel.name;
					TimeSpan startTime = servingTimeModel.startTime;
					TimeSpan? endTime = servingTimeModel.endTime;
					DateTime date = DateTime.Now;
					servingTimeDataAdapter.Insert(name, startTime, endTime, date, date);
					return RedirectToAction("ViewServingTime", "Schedule");
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}
			return View();
		}
	}
}
