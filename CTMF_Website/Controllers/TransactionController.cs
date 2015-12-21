using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class TransactionController : Controller
	{
		DataTable transactionDT = new DataTable();

		[Authorize(Roles = ("Manager"))]
		public ActionResult RechargeMoney()
		{
			return View();
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RechargeMoney(RechargeMoney model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string updateBy = AccountInfo.GetUserName(Request);
			string username = model.Username;
			int amountOfMoney = 0;
			if (!string.IsNullOrEmpty(model.AmountOfMoney.ToString()))
			{
				amountOfMoney = model.AmountOfMoney;
			}
			string transactionContent = "Nạp tiền";
			DateTime date = DateTime.Now;
			int transactionType = 2;
			UserInfoDetailTableAdapter userInfoAdapter = new UserInfoDetailTableAdapter();
			DataTable userInfoDataTable = userInfoAdapter.GetDataByUsername(username);

			if (userInfoDataTable.Rows.Count != 1)
			{
				ModelState.AddModelError("", "Tên đăng nhập không tồn tại.");
				return View(model);
			}
			TransactionHistoryTableAdapter transactionAdapter = new TransactionHistoryTableAdapter();
			try
			{
				string transactionID = transactionAdapter.RechargeMoneyScalar(username, transactionType, amountOfMoney, transactionContent, null, false, date, updateBy, date).ToString();
				int id = int.Parse(transactionID);
				XmlSync.SaveTransactionHistoryXml(id, username, transactionType, amountOfMoney, transactionContent, null, false, date, updateBy, date, null);
				Session["rechargeMoney"] = "Giao dịch thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["rechargeMoney"] = "Giao dịch thất bại!";
			}

			return RedirectToAction("RechargeMoney", "Transaction");
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult WithdrawMoney()
		{

			return View();
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult WithdrawMoney(WithdrawMoney model)
		{
			ViewBag.message = "";

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string updateBy = AccountInfo.GetUserName(Request);
			string username = model.Username;
			int withDrawMoney = model.AmountOfMoney;
			int transactionType = 3;
			string transactionContent = "Rút tiền";

			UserinfoModel userinfo = new UserinfoModel();
			DataTable userInfoDataTable = new DataTable();
			UserInfoDetailTableAdapter userInfoDetailAdapter = new UserInfoDetailTableAdapter();
			UserInfoTableAdapter userInfoAdapter = new UserInfoTableAdapter();
			userInfoDataTable = userInfoAdapter.GetDataByUsername(username);
			//string userTypeName = null;
			//string email = null;
			//if (!string.IsNullOrEmpty(userInfoDataTable.Rows[0]["TypeShortName"].ToString()))
			//{
			//	userInfoDataTable = userInfoDetailAdapter.GetDataByUsername(username);
			//	userTypeName = (string)userInfoDataTable.Rows[0]["TypeName"];
			//	email = userInfoDataTable.Rows[0]["Email"].ToString();
			//}
			//else
			//{
			//	AccountTableAdapter accountAdapter = new AccountTableAdapter();
			//	DataTable accountDataTable = new DataTable();
			//	accountDataTable = accountAdapter.GetDataByUsername(username);
			//	email = accountDataTable.Rows[0]["Email"].ToString();
			//}
			DateTime date = DateTime.Parse(userInfoDataTable.Rows[0]["LastUpdatedMoney"].ToString());
			int amountOfMoney = (int)userInfoDataTable.Rows[0]["AmountOfMoney"];

			TransactionHistoryTableAdapter transactionAdapter = new TransactionHistoryTableAdapter();
			int? money = transactionAdapter.GetCurrentMoney(username, date);
			if (money == null)
			{
				money = 0;
			}

			amountOfMoney += money.Value;

			if (withDrawMoney > amountOfMoney)
			{
				Session["withDrawMoney"] = "Giao dịch thất bại! Số tiền trong tài khoản không đủ để thực hiện giao dịch";
				return View(model);
			}
			else
			{
				try
				{
					string transactionID = transactionAdapter.RechargeMoneyScalar(username, transactionType, amountOfMoney, transactionContent, null, false, date, updateBy, date).ToString();
					int id = int.Parse(transactionID);
					XmlSync.SaveTransactionHistoryXml(id, username, transactionType, amountOfMoney, transactionContent, null, false, date, updateBy, date, null);
					Session["withDrawMoney"] = "Giao dịch thành công!";
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
					Session["withDrawMoney"] = "Giao dịch thất bại!";
				}

				return RedirectToAction("WithdrawMoney","Transaction");
			}
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult ListTransaction()
		{
			TransactionHistoryListTableAdapter transactionAdapter
				= new TransactionHistoryListTableAdapter();
			DataTable transactionDT = new DataTable();
			ViewBag.error = "";

			string username = Request.QueryString["username"];
			string transactionType = Request.QueryString["transactionType"];
			string date = Request.QueryString["date"];

			string page = Request.QueryString["page"];
			string amountPerPage = Request.QueryString["amountPerPage"];

			int transactionType_;
			if (!int.TryParse(transactionType, out transactionType_))
			{
				transactionType = null;
			}
			else
			{
				if (transactionType_ == 0)
				{
					transactionType = null;
				}
			}

			DateTime date_;
			if (String.IsNullOrWhiteSpace(date))
			{
				date = null;
			}
			if (!DateTime.TryParseExact(date, "dd/MM/yyyy", null
				, System.Globalization.DateTimeStyles.None
				, out date_))
			{
				ViewBag.error = "Sai định dạng ngày, tháng";
				date = null;
			}
			else
			{
				date = date_.ToString("yyyy-MM-dd");
			}

			int page_;
			if (!int.TryParse(page, out page_))
			{
				page = null;
				page_ = 1;
			}

			int amountPerPage_;
			if (!int.TryParse(amountPerPage, out amountPerPage_))
			{
				amountPerPage = "10";
				amountPerPage_ = 10;
			}

			try
			{
				string query = "SELECT * FROM ( SELECT TH.TransactionHistoryID, TH.Username, TH.TransactionTypeID, "
					+ "TH.Value, TH.TransactionContent, TH.IsAuto, TH.InsertedDate, TT.Name, "
					+ "ROW_NUMBER() OVER (ORDER BY transactionhistoryid DESC) AS RowNum "
					+ "FROM TransactionHistory AS TH INNER JOIN "
					+ "TransactionType AS TT ON TH.TransactionTypeID = TT.TransactionTypeID ";
				string countQuery = "SELECT COUNT(TH.TransactionHistoryID) FROM TransactionHistory AS TH ";
				string conditionQuery = "";

				if (username != null || transactionType != null || date != null)
				{
					conditionQuery += "WHERE ";
					bool isFirst = false;

					if (username != null)
					{
						conditionQuery += "TH.Username LIKE '%" + username + "%' ";
						isFirst = true;
					}

					if (transactionType != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "TH.TransactionTypeID = " + transactionType + " ";
						isFirst = true;
					}

					if (date != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "TH.InsertedDate BETWEEN '" + date + " 00:00:000' AND '" + date + " 23:59:59:999' ";
					}
				}

				int minRowNum = ((page_ - 1) * amountPerPage_) + 1;
				int maxRowNum = page_ * amountPerPage_;
				query += conditionQuery;
				query += ") AS SOD WHERE SOD.RowNum BETWEEN (" + minRowNum + ") AND (" + maxRowNum + ") ";

				SqlCommand countCmd = new SqlCommand(countQuery + conditionQuery, transactionAdapter.Connection);
				SqlCommand getDataCmd = new SqlCommand(query, transactionAdapter.Connection);
				SqlDataAdapter getDataAdapter = new SqlDataAdapter(getDataCmd);

				transactionAdapter.Connection.Open();
				int count = (int)countCmd.ExecuteScalar();

				getDataAdapter.Fill(transactionDT);

				int maxPage = (count / amountPerPage_);
				if (count % amountPerPage_ != 0)
				{
					maxPage++;
				}

				ViewBag.maxPage = maxPage;
				if (page_ > maxPage)
				{
					page_ = maxPage;
				}

				ViewBag.curPage = page_;
				ViewBag.amountPerPage = amountPerPage_;

				DataTable transactionTypeDT = new TransactionTypeTableAdapter().GetData();
				List<KeyValuePair<int, string>> transactionTypes = new List<KeyValuePair<int, string>>();

				foreach (DataRow row in transactionTypeDT.Rows)
				{
					transactionTypes.Add(new KeyValuePair<int, string>(
						row.Field<int>("TransactionTypeID"),
						row.Field<string>("Name")));
				}

				ViewBag.transactionTypes = transactionTypes;
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}

			return View(transactionDT);
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public ActionResult TransactionHistory(string transactionType, string date, string page, string amountPerPage)
		{
			ViewBag.error = "";
			string username = AccountInfo.GetUserName(Request);

			TransactionHistoryListTableAdapter transactionAdapter
				= new TransactionHistoryListTableAdapter();

			int transactionTypeID;
			if (!int.TryParse(transactionType, out transactionTypeID))
			{
				transactionType = null;
			}

			DateTime date_;
			if (string.IsNullOrWhiteSpace(date))
			{
				date = null;
			}
			else if (!DateTime.TryParseExact(date, "dd/MM/yyyy", null
				, System.Globalization.DateTimeStyles.None
				, out date_))
			{
				ViewBag.error = "Sai định dạng ngày, tháng";
				date = null;
			}
			else
			{
				date = date_.ToString("yyyy-MM-dd");
			}

			int page_;
			if (!int.TryParse(page, out page_))
			{
				page = null;
				page_ = 1;
			}

			int amountPerPage_;
			if (!int.TryParse(amountPerPage, out amountPerPage_))
			{
				amountPerPage = "10";
				amountPerPage_ = 10;
			}

			try
			{
				string query = "SELECT * FROM ( SELECT TH.TransactionHistoryID, TH.Username, TH.TransactionTypeID, "
					+ "TH.Value, TH.TransactionContent, TH.IsAuto, TH.InsertedDate, TT.Name, ROW_NUMBER() OVER "
					+ "(ORDER BY TH.InsertedDate DESC) AS RowNum FROM TransactionHistory AS TH INNER JOIN "
					+ "TransactionType AS TT ON TH.TransactionTypeID = TT.TransactionTypeID "
					+ "WHERE (TH.Username = '" + username + "') ";
				string conditionQuery = "";
				string countQuery = "select COUNT(th.TransactionHistoryID) from TransactionHistory TH WHERE  (TH.Username = '" + username + "') ";

				if (transactionType != null || date != null)
				{
					if (transactionType != null)
					{
						conditionQuery += "AND TH.TransactionTypeID = " + transactionType + " ";
					}

					if (date != null)
					{
						conditionQuery += "AND TH.InsertedDate BETWEEN '" + date + " 00:00:000' AND '" + date + " 23:59:59:999' ";
					}
				}

				transactionAdapter.Connection.Open();

				SqlCommand countCmd = new SqlCommand(countQuery + conditionQuery, transactionAdapter.Connection);
				int count = (int)countCmd.ExecuteScalar();

				int maxPage = (count / amountPerPage_);
				if (count % amountPerPage_ != 0)
				{
					maxPage++;
				}

				ViewBag.maxPage = maxPage;
				if (page_ > maxPage)
				{
					page_ = maxPage;
				}
				
				ViewBag.curPage = page_;
				ViewBag.amountPerPage = amountPerPage_;

				int minRowNum = ((page_ - 1) * amountPerPage_) + 1;
				int maxRowNum = page_ * amountPerPage_;

				query += conditionQuery;
				query += ") AS SOD WHERE SOD.RowNum BETWEEN (" + minRowNum + ") AND (" + maxRowNum + ") ";

				SqlCommand getDataCmd = new SqlCommand(query, transactionAdapter.Connection);
				SqlDataAdapter getDataAdapter = new SqlDataAdapter(getDataCmd);

				getDataAdapter.Fill(transactionDT);

				DataTable transactionTypeDT = new TransactionTypeTableAdapter().GetData();
				List<KeyValuePair<int, string>> transactionTypes = new List<KeyValuePair<int, string>>();

				foreach (DataRow row in transactionTypeDT.Rows)
				{
					transactionTypes.Add(new KeyValuePair<int, string>(
						row.Field<int>("TransactionTypeID"),
						row.Field<string>("Name")));
				}

				ViewBag.transactionTypes = transactionTypes;
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}

			return View(transactionDT);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult EditTransaction(string TransID)
		{
			TransactionHistoryTableAdapter adapter = new TransactionHistoryTableAdapter();
			EditTransaction model = new EditTransaction();

			TransactionTypeTableAdapter tranTypeAdapter = new TransactionTypeTableAdapter();
			DataTable tranTypeDT = tranTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in tranTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["Name"].ToString(), Value = row["TransactionTypeID"].ToString() });
			}
			ViewData["TransType"] = items;

			try
			{
				int transactionID = int.Parse(TransID);
				DataTable result = adapter.GetDataByKey(transactionID);

				if (result.Rows.Count != 0)
				{
					model.Username = result.Rows[0].Field<string>("Username");
					model.TransactionTypeID = result.Rows[0].Field<int>("TransactionTypeID");
					model.Value = result.Rows[0].Field<int>("Value");
					model.TransactionContent = result.Rows[0].Field<string>("TransactionContent");
					model.ScheduleMealSetDetailID = result.Rows[0].Field<int?>("ScheduleMealSetDetailID");
					model.IsAuto = result.Rows[0].Field<Boolean>("IsAuto");
					model.InsertedDate = result.Rows[0].Field<DateTime>("InsertedDate");
					model.UpdatedBy = result.Rows[0].Field<string>("UpdatedBy");
					model.LastUpdated = result.Rows[0].Field<DateTime>("LastUpdated");
					model.TransactionHistoryID = result.Rows[0].Field<int>("TransactionHistoryID");
				}
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return View(model);
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditTransaction(EditTransaction model)
		{
			TransactionTypeTableAdapter tranTypeAdapter = new TransactionTypeTableAdapter();
			DataTable tranTypeDT = tranTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in tranTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["Name"].ToString(), Value = row["TransactionTypeID"].ToString() });
			}
			ViewData["TransType"] = items;

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			int transactionHistoryID = model.TransactionHistoryID;
			string username = model.Username;
			int transactionTypeID = model.TransactionTypeID;
			int value = model.Value;
			string transactionContent = model.TransactionContent;
			int? scheduleMealSetDetailID = model.ScheduleMealSetDetailID;
			Boolean isAuto = model.IsAuto;
			DateTime insertedDate = model.InsertedDate;
			string updatedBy = model.UpdatedBy;
			DateTime lastUpdated = DateTime.Now;
			TransactionHistoryTableAdapter transAdapter = new TransactionHistoryTableAdapter();
			try
			{
				transAdapter.UpdateTransactionHistory(username, transactionTypeID, value, transactionContent
					, scheduleMealSetDetailID, isAuto, insertedDate, AccountInfo.GetUserName(Request), lastUpdated
					, transactionHistoryID);
				XmlSync.SaveTransactionHistoryXml(transactionHistoryID, username, transactionTypeID, value, transactionContent
					, scheduleMealSetDetailID, isAuto, insertedDate, AccountInfo.GetUserName(Request), lastUpdated, null);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListTransaction", "Transaction");
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult DetailTransaction(string TransID)
		{
			TransactionHistoryDetailTableAdapter transactionDetail = new TransactionHistoryDetailTableAdapter();
			DetailTransaction detail = new DetailTransaction();

			try
			{
				int tranID = int.Parse(TransID);

				transactionDT = transactionDetail.GetData(tranID);
				DataRow data = transactionDT.Rows[0];

				detail.Username = (string)data["Username"];
				detail.TransactionTypeName = (string)data["TransactionTypeName"];
				detail.Name = (string)data["Name"];
				detail.TransactionContent = (string)data["TransactionContent"];
				detail.Value = (int)data["Value"];
				detail.InsertedDate = (DateTime)data["InsertedDate"];
				detail.UpdatedBy = (string)data["UpdatedBy"];
				detail.LastUpdated = (DateTime)data["LastUpdated"];
				//detail.MealSetName = data.Field<string>("MealSetName");
				detail.MealSetName = data["DishName"].ToString();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("ListTransaction", "Transaction");
			}
			return View(detail);
		}
	}
}
