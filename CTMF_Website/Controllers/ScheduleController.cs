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
using System.Data.SqlClient;

namespace CTMF_Website.Controllers
{
	public class ScheduleController : Controller
	{
		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public ActionResult Schedule()
		{
			return View();
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public ActionResult ListSchedule()
		{
			return View();
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
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

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetEatingTime()
		{
			try
			{
				ServingTimeTableAdapter servingTimeTA = new ServingTimeTableAdapter();
				DataTable servingTimeDT = servingTimeTA.GetData();

				IList<ServingTimeJsonModel> servingTime = new List<ServingTimeJsonModel>();

				foreach (DataRow row in servingTimeDT.Rows)
				{
					ServingTimeJsonModel model = new ServingTimeJsonModel();

					model.ServingTimeID = row.Field<int>("ServingTimeID");
					model.Name = row.Field<string>("Name");
					model.StartTimeStr = row.Field<TimeSpan>("StartTime").ToString(@"hh\:mm");

					TimeSpan? temp = row.Field<TimeSpan?>("EndTime");
					if (temp == null)
					{
						model.EndTimeStr = null;
					}
					else
					{
						model.EndTimeStr = temp.Value.ToString(@"hh\:mm");
					}

					servingTime.Add(model);
				}

				return Json(new { servingTime = servingTime }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.ToString());
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetMeatSetForDDL()
		{
			try
			{
				MealSetTableAdapter mealSetTA = new MealSetTableAdapter();
				DataTable mealSetDT = mealSetTA.GetData();

				IList<MeatSetForDDLJsonModel> mealSet = new List<MeatSetForDDLJsonModel>();

				foreach (DataRow row in mealSetDT.Rows)
				{
					MeatSetForDDLJsonModel model = new MeatSetForDDLJsonModel();

					model.MealSetID = row.Field<int>("MealSetID");
					model.Name = row.Field<string>("Name");

					mealSet.Add(model);
				}

				return Json(new { mealSet = mealSet }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetScheduleData(int selectedMonth, int selectedYear)
		{
			string username = AccountInfo.GetUserName(Request);
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

			// N: haven't schedule
			// X: is day off
			// 0: is day on
			// Number: number of eating time in that day
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

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetScheduleDateDetail(int selectedDay, int selectedMonth, int selectedYear)
		{
			try
			{
				DateTime selectedDate = DateTime.Parse(selectedYear + "-" + selectedMonth + "-" + selectedDay);

				MealSetInScheduleTableAdapter adapter = new MealSetInScheduleTableAdapter();
				DataTable dt = adapter.GetData(selectedDate);

				if (dt.Rows.Count == 0)
				{
					return null;
				}

				List<ScheduleJsonModel> schedule = new List<ScheduleJsonModel>();
				foreach (DataRow row in dt.Rows)
				{
					ScheduleJsonModel model = new ScheduleJsonModel();
					model.MealSetID = row.Field<int?>("MealSetID");
					model.ScheduleMealSetDetailID = row.Field<int?>("ScheduleMealSetDetailID");
					model.Name = row.Field<string>("Name");
					model.Image = row.Field<string>("Image");
					model.Description = row.Field<string>("Description");
					model.ServingTimeID = row.Field<int>("ServingTimeID");
					model.ServingTimeName = row.Field<string>("ServingTimeName");
					model.IsDayOn = row.Field<bool>("IsDayOn");

					schedule.Add(model);
				}

				return Json(new { result = schedule }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetScheduleMonthDetail(int selectedMonth, int selectedYear)
		{
			try
			{
				int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < daysInMonth; i++)
				{
					sb.Append("N");
				}

				ScheduleMonthTableAdapter scheduleMonthTA = new ScheduleMonthTableAdapter();
				DataTable scheduleMonthDT = scheduleMonthTA.GetData(new DateTime(selectedYear, selectedMonth, 1)
					, new DateTime(selectedYear, selectedMonth, daysInMonth));

				DateTime date;
				bool isDayOn;
				int? ScheduleMealSetDetailID;
				foreach (DataRow row in scheduleMonthDT.Rows)
				{
					date = row.Field<DateTime>("Date");
					isDayOn = row.Field<bool>("IsDayOn");
					ScheduleMealSetDetailID = row.Field<int?>("ScheduleMealSetDetailID");

					int day = date.Day;
					day--;
					char ch = sb[day];

					if (ch == 'N' || ch == 'X')
					{
						if (isDayOn)
						{
							//sb[day] = '0';
							sb[day] = (char)33;
							if (ScheduleMealSetDetailID != null)
							{
								//sb[day] = '1';
								sb[day] = (char)34;
							}
						}
						else
						{
							sb[day] = 'X';
						}
					}
					else
					{
						if (isDayOn && ScheduleMealSetDetailID != null)
						{
							char number = sb[day];
							number++;
							sb[day] = number;
						}
					}
				}

				return Json(new { value = sb.ToString() }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json(new { value = "error" }, JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetScheduleID(int selectedDay, int selectedMonth, int selectedYear, int ServingTimeID)
		{
			try
			{
				DateTime selectedDate = DateTime.Parse(selectedYear + "-" + selectedMonth + "-" + selectedDay);

				ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();
				int? ScheduleID = scheduleTA.GetIDFromDateAndServingTime(selectedDate, ServingTimeID);

				if (ScheduleID == null)
				{
					DateTime now = DateTime.Now;
					string username = AccountInfo.GetUserName(Request);

					string newScheduleID = scheduleTA.InsertScalar(selectedDate, ServingTimeID, true, now, username, now).ToString();

					ScheduleID = int.Parse(newScheduleID);
					XmlSync.SaveScheduleXml(ScheduleID.Value, selectedDate, ServingTimeID, true, now, username, now, null);
				}

				return Json(new { ScheduleID = ScheduleID.Value }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult GetEatList(int selectedDay, int selectedMonth, int selectedYear)
		{
			try
			{
				DateTime selectedDate = DateTime.Parse(selectedYear + "-" + selectedMonth + "-" + selectedDay);
				DateTime endDate = selectedDate.AddDays(1).AddTicks(-1);

				DataTable dt = new UserEatTableAdapter().GetData(AccountInfo.GetUserName(Request), selectedDate, selectedDate, endDate);

				List<UserEatJsonModel> result = new List<UserEatJsonModel>();
				int Unrecord = 0;
				foreach (DataRow row in dt.Rows)
				{
					result.Add(new UserEatJsonModel()
					{
						ScheduleMealSetDetailID = row.Field<int?>("ScheduleMealSetDetailID")
					});

					if (row.Field<int?>("ScheduleMealSetDetailID") == null)
					{
						Unrecord++;
					}
				}

				return Json(new { result = result, Unrecord = Unrecord }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		[HttpGet]
		public ActionResult GetMealSetModal(string MealSetID)
		{
			int mealSetID;

			if (String.IsNullOrWhiteSpace(MealSetID) || !int.TryParse(MealSetID, out mealSetID))
			{
				return RedirectToAction("Schedule", "Schedule");
			}

			MealSetDishDetailModel model = new MealSetDishDetailModel();

			DataTable dt = new MealSetDishInfoTableAdapter().GetDataByMealSetID(mealSetID);
			if (dt.Rows.Count > 0)
			{
				model.MealSetName = dt.Rows[0].Field<string>("MealSetName");
				model.MealSetImage = ".." + dt.Rows[0].Field<string>("MealSetImage");

				model.DishNameImage = new List<KeyValuePair<string, string>>();

				foreach (DataRow row in dt.Rows)
				{
					string dishImage = row.Field<string>("DishImage");
					if (dishImage == null)
					{
						dishImage = AppDomain.CurrentDomain.BaseDirectory + "..\\Images\\no-image.jpg";
					}
					else
					{
						dishImage = ".." + dishImage;
					}

					model.DishNameImage.Add(new KeyValuePair<string, string>(row.Field<string>("DishName"),
						dishImage));
				}
			}

			return PartialView(model);
		}

		// tu day tro xuong
		[Authorize(Roles = ("Manager"))]
		public ActionResult EditSchedule()
		{
			try
			{
				string ScheduleIDStr = Request.QueryString["ScheduleID"];

				if (ScheduleIDStr == null)
				{
					return RedirectToAction("ListSchedule", "Schedule");
				}

				int ScheduleID = int.Parse(ScheduleIDStr);

				MealSetInScheduleTableAdapter mealSetInScheduleTA = new MealSetInScheduleTableAdapter();

				DataTable mealSetInSchedule = mealSetInScheduleTA.GetDataByScheduleID(ScheduleID);
				if (mealSetInSchedule.Rows.Count == 0)
				{
					Log.ErrorLog("MealSetInScheduleTableAdapter: can't find row with [ScheduleID] = " + ScheduleIDStr);
					return View("Có lỗi khi lấy dữ liệu.");
				}

				@ViewBag.isDayOn = mealSetInSchedule.Rows[0].Field<bool>("IsDayOn");
				@ViewBag.scheduleID = ScheduleID;
				return View(mealSetInSchedule);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}
		}

		[Authorize(Roles = ("Manager"))]
		public JsonResult AddScheduleMealSet(int mealSetID, int scheduleID, bool isDayOn)
		{
			//if isDayOn = true, just insert into ScheduleMealSetDetail
			// else open transaction and set isDayOn of schedule = true
			if (isDayOn)
			{
				try
				{
					ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();

					DataTable dt = scheduleMealSetDetailTA.GetDataByMealSetIDScheduleID(mealSetID, scheduleID);
					if (dt.Rows.Count != 0)
					{
						return Json("duplicate", JsonRequestBehavior.AllowGet);
					}

					DateTime now = DateTime.Now;
					string newScheduleMealSetDetailIDStr = scheduleMealSetDetailTA.InsertScalar(mealSetID, scheduleID, now, now).ToString();
					int newScheduleMealSetDetailID = int.Parse(newScheduleMealSetDetailIDStr);

					XmlSync.SaveScheduleMealSetDetailXml(newScheduleMealSetDetailID, mealSetID, scheduleID, now, now, null);

					return Json(new { result = "done" }, JsonRequestBehavior.AllowGet);
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
					return Json("error", JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();
				ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();

				scheduleMealSetDetailTA.Connection.Open();
				scheduleTA.Connection = scheduleMealSetDetailTA.Connection;

				using (SqlTransaction transaction = scheduleMealSetDetailTA.Connection.BeginTransaction())
				{
					try
					{
						scheduleMealSetDetailTA.AttachTransaction(transaction);
						scheduleTA.AttachTransaction(transaction);

						DataTable scheduleDT = scheduleTA.GetDataByID(scheduleID);

						if (scheduleDT.Rows.Count != 1)
						{
							throw new Exception("Can't find schedule with [scheduleID] = " + scheduleID);
						}

						DataRow scheduleRow = scheduleDT.Rows[0];
						string username = AccountInfo.GetUserName(Request);
						DateTime now = DateTime.Now;

						scheduleTA.Update(scheduleRow.Field<DateTime>("Date"), scheduleRow.Field<int>("ServingTimeID"), true
							, scheduleRow.Field<DateTime>("InsertedDate"), username, now, scheduleID);

						XmlSync.SaveScheduleXml(scheduleID, scheduleRow.Field<DateTime>("Date"), scheduleRow.Field<int>("ServingTimeID"), true
							, scheduleRow.Field<DateTime>("InsertedDate"), username, now, null);

						string newScheduleMealSetDetailIDStr = scheduleMealSetDetailTA.InsertScalar(mealSetID, scheduleID, now, now).ToString();
						int newScheduleMealSetDetailID = int.Parse(newScheduleMealSetDetailIDStr);

						XmlSync.SaveScheduleMealSetDetailXml(newScheduleMealSetDetailID, mealSetID, scheduleID, now, now, null);

						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						Log.ErrorLog(ex.Message);
						return Json("error", JsonRequestBehavior.AllowGet);
					}
				}

				return Json(new { result = "done" }, JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Manager"))]
		public JsonResult RemoveScheduleMealSet(int scheduleMealSetDetailID)
		{
			try
			{
				ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();

				int rowCount = scheduleMealSetDetailTA.Delete(scheduleMealSetDetailID);

				if (rowCount == 0)
				{
					Log.ErrorLog("Can't find ScheduleMealSetDetail with [ScheduleMealSetDetailID] = " + scheduleMealSetDetailID);
					return Json("error", JsonRequestBehavior.AllowGet);
				}

				return Json(new { result = "done" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		[Authorize(Roles = ("Manager"))]
		public JsonResult SetIsDayOn(bool isDayOn, int scheduleID)
		{
			ScheduleMealSetDetailTableAdapter scheduleMealSetDetailTA = new ScheduleMealSetDetailTableAdapter();
			ScheduleTableAdapter scheduleTA = new ScheduleTableAdapter();

			scheduleMealSetDetailTA.Connection.Open();
			scheduleTA.Connection = scheduleMealSetDetailTA.Connection;

			using (SqlTransaction transaction = scheduleMealSetDetailTA.Connection.BeginTransaction())
			{
				scheduleMealSetDetailTA.AttachTransaction(transaction);
				scheduleTA.AttachTransaction(transaction);

				try
				{
					if (!isDayOn)
					{
						DataTable scheduleMealSetDetailDT = scheduleMealSetDetailTA.GetDataByScheduleID(scheduleID);

						foreach (DataRow row in scheduleMealSetDetailDT.Rows)
						{
							int scheduleMealSetDetailID = row.Field<int>("ScheduleMealSetDetailID");
							scheduleMealSetDetailTA.Delete(scheduleMealSetDetailID);
						}
					}

					DataTable scheduleDT = scheduleTA.GetDataByID(scheduleID);

					if (scheduleDT.Rows.Count != 1)
					{
						throw new Exception("Can't find schedule with [scheduleID] = " + scheduleID);
					}

					DataRow scheduleRow = scheduleDT.Rows[0];
					string username = AccountInfo.GetUserName(Request);
					DateTime now = DateTime.Now;

					scheduleTA.Update(scheduleRow.Field<DateTime>("Date"), scheduleRow.Field<int>("ServingTimeID"), isDayOn
						, scheduleRow.Field<DateTime>("InsertedDate"), username, now, scheduleID);

					XmlSync.SaveScheduleXml(scheduleID, scheduleRow.Field<DateTime>("Date"), scheduleRow.Field<int>("ServingTimeID"), isDayOn
						, scheduleRow.Field<DateTime>("InsertedDate"), username, now, null);

					transaction.Commit();
					return Json(new { result = "done" }, JsonRequestBehavior.AllowGet);
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
					transaction.Rollback();
					return Json("error", JsonRequestBehavior.AllowGet);
				}
			}
		}

		//Serving time controller
		[Authorize(Roles = ("Manager"))]
		public ActionResult ViewServingTime()
		{
			DataTable dataTable = new DataTable();
			ServingTimeTableAdapter servingTimeAdapter = new ServingTimeTableAdapter();

			try
			{
				dataTable = servingTimeAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return View(dataTable);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult EditServingTime(string servingTimeID)
		{
			ServingTimeModel servingTimeModel = new ServingTimeModel();
			DataTable servingTimeDataTable = new DataTable();
			ServingTimeTableAdapter servingTimeAdapter = new ServingTimeTableAdapter();
			try
			{
				int servingTimeId = Convert.ToInt32(servingTimeID);
				servingTimeDataTable = servingTimeAdapter.GetDataByID(servingTimeId);
				servingTimeModel.servingTimeID = servingTimeDataTable.Rows[0].Field<int>("ServingTimeID");
				servingTimeModel.Name = Convert.ToString(servingTimeDataTable.Rows[0]["Name"]);
				servingTimeModel.startTime = (TimeSpan)servingTimeDataTable.Rows[0]["StartTime"];
				servingTimeModel.endTime = (TimeSpan)servingTimeDataTable.Rows[0]["EndTime"];
				servingTimeModel.insertDate = Convert.ToDateTime(servingTimeDataTable.Rows[0]["InsertedDate"]);
				servingTimeModel.lastUpdate = Convert.ToDateTime(servingTimeDataTable.Rows[0]["LastUpdated"]);

			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "Error");
			}
			return View(servingTimeModel);
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		public ActionResult EditServingTime(ServingTimeModel servingTimeModel, string servingTimeID)
		{
			if (!ModelState.IsValid)
			{
				return View(servingTimeModel);
			}

			ServingTimeTableAdapter servingTimeTableAdapter = new ServingTimeTableAdapter();
			DataTable servingTimeDT = servingTimeTableAdapter.GetData();

			try
			{
				int servingTimeId = Convert.ToInt32(servingTimeID);
				string name = servingTimeModel.Name;
				TimeSpan startTime = servingTimeModel.startTime;
				TimeSpan endTime = servingTimeModel.endTime;

				TimeSpan startTimeDB, endTimeDB;

				if (startTime > endTime)
				{
					ModelState.AddModelError("", "Thời gian bắt đầu không được lớn hơn thời gian kết thúc.");
					return View(servingTimeModel);
				}

				foreach (System.Data.DataRow row in servingTimeDT.Rows)
				{
					if (!(row.Field<int>("ServingTimeID") == servingTimeId))
					{
						startTimeDB = row.Field<TimeSpan>("StartTime");
						endTimeDB = row.Field<TimeSpan>("EndTime");

						if ((startTime >= startTimeDB && startTime <= endTimeDB) || (endTime >= startTimeDB && endTime <= endTimeDB) || (startTime <= startTimeDB && endTime >= endTimeDB))
						{
							ModelState.AddModelError("", "Thời gian phục vụ mới trùng với thời gian phục vụ có trước đó. Vui lòng xác nhận lại!");
							return View(servingTimeModel);
						}
					}
				}

				DateTime insertDate = servingTimeModel.insertDate;
				DateTime lastUpdate = DateTime.Now;

				servingTimeTableAdapter.UpdateServingTimeByID(name, startTime, endTime, insertDate, lastUpdate, servingTimeId);
				Session["editServingTime"] = "Cập nhật thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["editServingTime"] = "Cập nhật thất bại!";
			}
			return RedirectToAction("EditServingTime", "Schedule", new { @servingTimeID = servingTimeID });
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult DeleteServingTime(string servingTimeID)
		{
			ServingTimeTableAdapter servingTimeTableAdapter = new ServingTimeTableAdapter();
			ScheduleTableAdapter scheduleTableAdapter = new ScheduleTableAdapter();
			try
			{
				int id = Convert.ToInt32(servingTimeID);
				servingTimeTableAdapter.DeleteServingTimeByID(id);
				Session["deleteServingTime"] = "Xóa thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["deleteServingTime"] = "Xóa thất bại! Thời gian phục vụ này đang được sử dụng.";
			}
			return RedirectToAction("ViewServingTime", "Schedule");
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult AddNewServingTime()
		{
			return View();
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewServingTime(ServingTimeModel servingTimeModel)
		{
			if (!ModelState.IsValid)
			{
				return View(servingTimeModel);
			}

			ServingTimeTableAdapter servingTimeDataAdapter = new ServingTimeTableAdapter();
			DataTable servingTimeDT = servingTimeDataAdapter.GetData();

			try
			{
				string name = servingTimeModel.Name;
				TimeSpan startTime = servingTimeModel.startTime;
				TimeSpan endTime = servingTimeModel.endTime;

				TimeSpan startTimeDB, endTimeDB;

				if (startTime > endTime)
				{
					ModelState.AddModelError("", "Thời gian bắt đầu không được lớn hơn thời gian kết thúc.");
					return View(servingTimeModel);
				}

				foreach (System.Data.DataRow row in servingTimeDT.Rows)
				{
					startTimeDB = row.Field<TimeSpan>("StartTime");
					endTimeDB = row.Field<TimeSpan>("EndTime");

					if ((startTime >= startTimeDB && startTime <= endTimeDB) || (endTime >= startTimeDB && endTime <= endTimeDB) || (startTime <= startTimeDB && endTime >= endTimeDB))
					{
						ModelState.AddModelError("", "Thời gian phục vụ mới trùng với thời gian phục vụ có trước đó. Vui lòng xác nhận lại!");
						return View(servingTimeModel);
					}
				}

				DateTime date = DateTime.Now;
				servingTimeDataAdapter.Insert(name, startTime, endTime, date, date);
				Session["addServingTime"] = "Thêm mới thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["addServingTime"] = "Thêm mới thất bại!";
			}
			return RedirectToAction("AddNewServingTime", "Schedule");
		}
	}
}
