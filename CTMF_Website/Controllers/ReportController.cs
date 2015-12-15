using ClosedXML.Excel;
using CTMF_Website.DataAccessTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class ReportController : Controller
	{
		[Authorize(Roles = ("Manager"))]
		public ActionResult Test()
		{
			DateTime monthBegin = DateTime.Parse("2015-12-1");
			DataTable dt = new CTMF_Website.DataAccessTableAdapters.TransactionHistoryTableAdapter().GetDataByDate(monthBegin, DateTime.Now);

			CTMF_Website.Util.ExcelReport.ExportToExcel(dt, "abc");
			return RedirectToAction("Index", "Report");
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult Report(string id, string frmDate, string tDate)
		{
			if (id == null)
			{
				return View();
			}

			int ID;
			DateTime fromDate = new DateTime();
			DateTime toDate = new DateTime();
			if (!int.TryParse(id, out ID))
			{
				ViewBag.error = "Không thể lấy thống kê";
				return View();
			}

			if (ID != 6)
			{
				if (String.IsNullOrWhiteSpace(frmDate))
				{
					//fromDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
					fromDate = DateTime.Now.Date;
				}
				else
				{
					if (!DateTime.TryParseExact(frmDate, "dd/MM/yyyy", null
					, System.Globalization.DateTimeStyles.None
					, out fromDate))
					{
						ViewBag.error = "Ngày bắt đầu bị sai";
						return View();
					}
				}

				if (String.IsNullOrWhiteSpace(tDate))
				{
					toDate = DateTime.Now;
				}
				else
				{
					if (!DateTime.TryParseExact(tDate, "dd/MM/yyyy", null
						, System.Globalization.DateTimeStyles.None
						, out toDate))
					{
						ViewBag.error = "Ngày kết thúc bị sai";
						return View();
					}
					else
					{
						toDate = toDate.AddDays(1).AddTicks(-1);
					}
				}
			}

			if (fromDate > toDate)
			{
				ViewBag.error = "Không thể lấy thống kê";
				return View();
			}

			//Lịch sử giao dịch
			if (ID == 1)
			{
				DataTable transactionDT = new TransactionHistoryListTableAdapter().GetDataByDate(fromDate, toDate);

				transactionDT.PrimaryKey = null;

				transactionDT.Columns.Remove("TransactionHistoryID");
				transactionDT.Columns.Remove("TransactionTypeID");
				transactionDT.Columns.Remove("IsAuto");

				transactionDT.Columns["Username"].SetOrdinal(0);
				transactionDT.Columns["Value"].SetOrdinal(1);
				transactionDT.Columns["TransactionContent"].SetOrdinal(2);
				transactionDT.Columns["Name"].SetOrdinal(3);
				transactionDT.Columns["InsertedDate"].SetOrdinal(4);

				transactionDT.Columns["Username"].ColumnName = "Tên đăng nhập";
				transactionDT.Columns["Value"].ColumnName = "Giá trị giao dịch";
				transactionDT.Columns["TransactionContent"].ColumnName = "Nội dung giao dịch";
				transactionDT.Columns["Name"].ColumnName = "Loại giao dịch";
				transactionDT.Columns["InsertedDate"].ColumnName = "Thời điểm giao dịch";

				CTMF_Website.Util.ExcelReport.ExportToExcel(transactionDT, "Lich_Su_Giao_Dich");
			}
			//Lịch sử ăn
			else if (ID == 2)
			{
				DataTable transactionDT = new TransactionHistoryListTableAdapter().GetEatingDataByDate(fromDate, toDate);

				transactionDT.PrimaryKey = null;

				transactionDT.Columns.Remove("TransactionHistoryID");
				transactionDT.Columns.Remove("TransactionTypeID");
				transactionDT.Columns.Remove("IsAuto");
				transactionDT.Columns.Remove("Name");

				transactionDT.Columns["Username"].SetOrdinal(0);
				transactionDT.Columns["Value"].SetOrdinal(1);
				transactionDT.Columns["TransactionContent"].SetOrdinal(2);
				transactionDT.Columns["InsertedDate"].SetOrdinal(3);

				transactionDT.Columns["Username"].ColumnName = "Tên đăng nhập";
				transactionDT.Columns["Value"].ColumnName = "Giá trị giao dịch";
				transactionDT.Columns["TransactionContent"].ColumnName = "Nội dung giao dịch";
				transactionDT.Columns["InsertedDate"].ColumnName = "Thời điểm giao dịch";

				CTMF_Website.Util.ExcelReport.ExportToExcel(transactionDT, "Lich_Su_An");
			}
			//Thống kế lượt ăn theo bữa
			else if (ID == 3)
			{
				DataTable eatTimeDT = new EatTimeTableAdapter().GetDataByDate(fromDate.Date, toDate.Date);

				eatTimeDT.Columns["Date"].SetOrdinal(0);
				eatTimeDT.Columns["Name"].SetOrdinal(1);
				eatTimeDT.Columns["EatTime"].SetOrdinal(2);

				eatTimeDT.Columns["Date"].ColumnName = "Ngày";
				eatTimeDT.Columns["Name"].ColumnName = "Tên bữa ăn";
				eatTimeDT.Columns["EatTime"].ColumnName = "Số lượt ăn";

				CTMF_Website.Util.ExcelReport.ExportToExcel(eatTimeDT, "Luot_An_Theo_Bua");
			}
			//Thông kê suất ăn được sử dụng
			else if (ID == 4)
			{
				DataTable mealSetEatTime = new MealSetEatTimeTableAdapter().GetDataByDate(fromDate, toDate);

				mealSetEatTime.Columns["MealSetID"].SetOrdinal(0);
				mealSetEatTime.Columns["Name"].SetOrdinal(1);
				mealSetEatTime.Columns["EatTime"].SetOrdinal(2);

				mealSetEatTime.Columns["MealSetID"].ColumnName = "Mã suất ăn";
				mealSetEatTime.Columns["Name"].ColumnName = "Tên suất ăn";
				mealSetEatTime.Columns["EatTime"].ColumnName = "Số lượt ăn";

				CTMF_Website.Util.ExcelReport.ExportToExcel(mealSetEatTime, "Luot_An_Theo_Suat");
			}
			//Thống kê món ăn được sử dụng
			else if (ID == 5)
			{
				DataTable dishEatTime = new DishEatTimeTableAdapter().GetDataByDate(fromDate, toDate);

				dishEatTime.Columns["DishID"].SetOrdinal(0);
				dishEatTime.Columns["Name"].SetOrdinal(1);
				dishEatTime.Columns["EatTime"].SetOrdinal(2);

				dishEatTime.Columns["DishID"].ColumnName = "Mã món ăn";
				dishEatTime.Columns["Name"].ColumnName = "Tên suất ăn";
				dishEatTime.Columns["EatTime"].ColumnName = "Số lượt ăn";

				CTMF_Website.Util.ExcelReport.ExportToExcel(dishEatTime, "Luot_An_Theo_Mon_An");
			}
			//Thống kê người dùng đang nợ tiền
			else if (ID == 6)
			{
				DataTable debtCustomer = new DebtCustomerTableAdapter().GetData();

				debtCustomer.Columns["Username"].SetOrdinal(0);
				debtCustomer.Columns["Name"].SetOrdinal(1);
				debtCustomer.Columns["Email"].SetOrdinal(2);
				debtCustomer.Columns["TypeName"].SetOrdinal(3);
				debtCustomer.Columns["Money"].SetOrdinal(4);

				debtCustomer.Columns["Username"].ColumnName = "Tên đăng nhập";
				debtCustomer.Columns["Name"].ColumnName = "Tên";
				debtCustomer.Columns["TypeName"].ColumnName = "Loại người dùng";
				debtCustomer.Columns["Money"].ColumnName = "Số tiền đang nợ";

				CTMF_Website.Util.ExcelReport.ExportToExcel(debtCustomer, "Nguoi_Dung_Dang_No");
			}

			return View();
		}
	}
}
