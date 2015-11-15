using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class TransactionController : Controller
	{
		DataTable transactionDT = new DataTable();
		[AllowAnonymous]
		public ActionResult TransactionHistory()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult RechargeMoney()
		{
			return View();
		}

		[AllowAnonymous]
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
			int amountOfMoney = model.AmountOfMoney;
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
				transactionAdapter.RechargeMoney(username, transactionType, amountOfMoney, transactionContent, null, false, date, updateBy, date);
			}
			catch(Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListTransaction","Transaction");
		}

		[AllowAnonymous]
		public ActionResult ListTransaction()
		{
			TransactionHistoryListTableAdapter transactionAdapter
				= new TransactionHistoryListTableAdapter();

			try
			{
				transactionDT = transactionAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return View(transactionDT);
		}

		[AllowAnonymous]
		public ActionResult EditTransaction(string TransID)
		{
			TransactionHistoryTableAdapter adapter = new TransactionHistoryTableAdapter();
			EditTransaction model = new EditTransaction();

			TransactionTypeTableAdapter tranTypeAdapter = new TransactionTypeTableAdapter();
			DataTable tranTypeDT = tranTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach(DataRow row in tranTypeDT.Rows)
			{
				items.Add(new SelectListItem{Text = row["Name"].ToString(), Value = row["TransactionTypeID"].ToString()});
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

		[AllowAnonymous]
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
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListTransaction","Transaction");
		}

		[AllowAnonymous]
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
				detail.DishName = (string)data["DishName"];
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return View(detail);
		}
	}
}
