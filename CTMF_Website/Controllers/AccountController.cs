using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System.Data.SqlClient;

namespace CTMF_Website.Controllers
{
	public class AccountController : Controller
	{
		DataTable userDT = new DataTable();


		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}


		public ActionResult UserInfo()
		{
			string username = AccountInfo.GetUserName(Request);

			UserinfoModel userinfo = new UserinfoModel();
			DataTable userInfoDataTable = new DataTable();
			UserInfoDetailTableAdapter userInfoDetailAdapter = new UserInfoDetailTableAdapter();
			//DataTable userinfoDataTable = userInfoDetailAdapter.GetDataByUsername(username);
			UserInfoTableAdapter userInfoAdapter = new UserInfoTableAdapter();
			userInfoDataTable = userInfoAdapter.GetDataByUsername(username);
			string userTypeName = null;
			string email = null;
			if (!string.IsNullOrEmpty(userInfoDataTable.Rows[0]["TypeShortName"].ToString()))
			{
				userInfoDataTable = userInfoDetailAdapter.GetDataByUsername(username);
				userTypeName = (string)userInfoDataTable.Rows[0]["TypeName"];
				email = userInfoDataTable.Rows[0]["Email"].ToString();
			}
			else
			{
				AccountTableAdapter accountAdapter = new AccountTableAdapter();
				DataTable accountDataTable = new DataTable();
				accountDataTable = accountAdapter.GetDataByUsername(username);
				email = accountDataTable.Rows[0]["Email"].ToString();
			}
			DateTime date = DateTime.Parse(userInfoDataTable.Rows[0]["LastUpdatedMoney"].ToString());
			int amountOfMoney = (int)userInfoDataTable.Rows[0]["AmountOfMoney"];

			TransactionHistoryTableAdapter transactionAdapter = new TransactionHistoryTableAdapter();
			int? money = transactionAdapter.GetCurrentMoney(username, date);
			if (money == null)
			{
				money = 0;
			}

			amountOfMoney += money.Value;

			userinfo.Username = username;
			userinfo.Name = userInfoDataTable.Rows[0]["Name"].ToString();
			userinfo.TypeName = userTypeName;
			userinfo.Email = email;
			userinfo.AmountOfMoney = amountOfMoney;

			return View(userinfo);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string username = model.Username;
			string password = model.Password;

			AccountTableAdapter adapter = new AccountTableAdapter();
			DataTable dt = adapter.GetDataByUsername(username);

			if (dt.Rows.Count != 1)
			{
				ModelState.AddModelError("", "Tên đăng nhập không tồn tại.");
				return View(model);
			}

			if ((string)dt.Rows[0]["PASSWORD"] != password)
			{
				ModelState.AddModelError("", "Sai mật khẩu.");
				return View(model);
			}

			try
			{
				string role = dt.Rows[0]["Role"].ToString();
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				1,
				username,
				DateTime.Now,
				DateTime.Now.AddMilliseconds(20),
				false,
				role
				);
				HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
				string cookieName = FormsAuthentication.FormsCookieName;
				string cookieValue = FormsAuthentication.Encrypt(ticket);
				HttpContext.Response.Cookies.Set(new HttpCookie(cookieName, cookieValue));
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			//return RedirectToAction("HomePage", "Home");
			return RedirectToLocal(returnUrl);
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("HomePage", "Home");
			}
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Logout()
		{
			string username = AccountInfo.GetUserName(Request);

			FormsAuthentication.SignOut();
			HttpCookie c = Request.Cookies[FormsAuthentication.FormsCookieName];
			c.Expires = DateTime.Now.AddDays(-1);

			Response.Cookies.Set(c);

			Session.Clear();

			return RedirectToAction("HomePage", "Home");
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string username = model.Username;
			string password = model.Password;
			string name = model.Name;
			string email = model.Email;
			DateTime date = DateTime.Now;
			string userType = "DF";

			string errormsg = null;

			UserInfoTableAdapter UserInfoAdapter = new UserInfoTableAdapter();
			DataTable UserInfoDT = UserInfoAdapter.GetDataByUsername(username);

			if (UserInfoDT.Rows.Count == 1)
			{
				errormsg += "Tên đăng nhập ";
			}

			if (!string.IsNullOrEmpty(email))
			{
				AccountTableAdapter AccountAdapter = new AccountTableAdapter();
				DataTable AccountDT = AccountAdapter.GetDataByEmail(email);

				if (AccountDT.Rows.Count == 1)
				{
					if (errormsg != null)
					{
						errormsg += ", Email ";
					}
					else errormsg += "Email ";
				}
			}

			if (errormsg != null)
			{
				errormsg += "đã tồn tại!";
				ModelState.AddModelError("", errormsg);
				return View(model);
			}
			else
			{
				AccountTableAdapter AccountAdapter = new AccountTableAdapter();

				UserInfoAdapter.Connection.Open();
				AccountAdapter.Connection = UserInfoAdapter.Connection;

				using (SqlTransaction transaction = UserInfoAdapter.Connection.BeginTransaction())
				{
					UserInfoAdapter.AttachTransaction(transaction);
					AccountAdapter.AttachTransaction(transaction);

					try
					{
						UserInfoAdapter.InsertUserInfo(username, name, userType, 0, date, null, null, null, false, false, date, username, date);
						Log.ActivityLog("Insert into UserInfo: Username = " + username);

						AccountAdapter.InsertAccount(username, password, email, 1, false);
						Log.ActivityLog("Insert into Account: Username = " + username);

						XmlSync.SaveUserInfoXml(username, name, userType, 0, date, null, null, null, false, false, date, username, date, null);
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						Log.ErrorLog(ex.Message);
					}
				}
			}

			return RedirectToAction("HomePage", "Home");
		}

		[AllowAnonymous]
		public ActionResult ListUser(string search, string filter)
		{
			UserInfoDetailTableAdapter userInfoAdapter = new UserInfoDetailTableAdapter();

			try
			{
				userDT = userInfoAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return View(userDT);
		}

		[AllowAnonymous]
		public ActionResult AddUser()
		{
			UserTypeTableAdapter userTypeAdapter = new UserTypeTableAdapter();
			DataTable userTypeDT = userTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in userTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["TypeShortName"].ToString() });
			}
			ViewData["UserType"] = items;
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddUser(UserInfoDetailModel model)
		{
			UserTypeTableAdapter userTypeAdapter = new UserTypeTableAdapter();
			DataTable userTypeDT = userTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in userTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["TypeShortName"].ToString() });
			}
			ViewData["UserType"] = items;

			if (!ModelState.IsValid)
			{
				return View();
			}

			string updateBy = AccountInfo.GetUserName(Request);
			string username = model.Username;
			string password = model.Password;
			string name = model.Name;
			string email = model.Email;
			string userTypeID = model.UserTypeID;
			int role = model.Role;
			bool isCafeteria = false;
			if (role == 2)
			{
				isCafeteria = true;
			}
			DateTime date = DateTime.Now;

			string errormsg = null;

			UserInfoTableAdapter userInfoAdapter = new UserInfoTableAdapter();
			DataTable userInfoDT = userInfoAdapter.GetDataByUsername(username);

			if (userInfoDT.Rows.Count == 1)
			{
				errormsg += "Tên đăng nhập ";
			}

			if (!string.IsNullOrEmpty(email))
			{
				AccountTableAdapter AccountAdapter = new AccountTableAdapter();
				DataTable AccountDT = AccountAdapter.GetDataByEmail(email);

				if (AccountDT.Rows.Count == 1)
				{
					if (errormsg != null)
					{
						errormsg += ", Email ";
					}
					else errormsg += "Email ";
				}
			}

			if (errormsg != null)
			{
				errormsg += "đã tồn tại!";
				ModelState.AddModelError("", errormsg);
				return View(model);
			}
			else
			{
				try
				{
					userInfoAdapter.InsertUserInfo(username, name, userTypeID, 0, date, null, null, null, isCafeteria, false, date, updateBy, date);
					Log.ActivityLog("Insert into UserInfo: Username = " + username);
					AccountTableAdapter AccountAdapter = new AccountTableAdapter();
					AccountAdapter.InsertAccount(username, password, email, role, false);
					Log.ActivityLog("Insert into Account: Username = " + username);
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}

			return RedirectToAction("ListUser", "Account");
		}

		[AllowAnonymous]
		public ActionResult EditUser(string username)
		{
			UserTypeTableAdapter userTypeAdapter = new UserTypeTableAdapter();
			DataTable userTypeDT = userTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in userTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["TypeShortName"].ToString() });
			}
			ViewData["UserType"] = items;

			EditUserModel userInfo = new EditUserModel();
			UserInfoDetailTableAdapter userinfoAdapter = new UserInfoDetailTableAdapter();
			DataTable userinfoDataTable = userinfoAdapter.GetDataByUsername(username);

			userInfo.Username = username;
			userInfo.Name = userinfoDataTable.Rows[0]["Name"].ToString();
			userInfo.UserTypeID = (string)userinfoDataTable.Rows[0]["TypeShortName"];
			userInfo.Email = userinfoDataTable.Rows[0]["Email"].ToString();
			userInfo.Role = (int)userinfoDataTable.Rows[0]["Role"];
			userInfo.isActive = (bool)userinfoDataTable.Rows[0]["IsActive"];

			return View(userInfo);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditUser(EditUserModel model)
		{
			UserTypeTableAdapter userTypeAdapter = new UserTypeTableAdapter();
			DataTable userTypeDT = userTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in userTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["TypeShortName"].ToString() });
			}
			ViewData["UserType"] = items;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string email = model.Email;
			if (!string.IsNullOrEmpty(email))
			{
				AccountTableAdapter AccountAdapter = new AccountTableAdapter();
				DataTable AccountDT = AccountAdapter.GetDataByEmail(email);

				if (AccountDT.Rows.Count == 1)
				{
					ModelState.AddModelError("", "Email đã tồn tại");
					return View(model);
				}
			}

			string updateBy = AccountInfo.GetUserName(Request);
			DateTime date = DateTime.Now;
			string username = model.Username;
			string name = model.Name;
			string userTypeID = model.UserTypeID;
			bool isCafeteriaStaff = false;
			int role = model.Role;
			bool isActive = model.isActive;
			if (role == 2)
			{
				isCafeteriaStaff = true;
			}

			AccountTableAdapter accountAdapter = new AccountTableAdapter();
			UserInfoTableAdapter userInfoAdapter = new UserInfoTableAdapter();

			DataTable userInfoDT = userInfoAdapter.GetDataByUsername(username);
			DataRow userInfoRow = userInfoDT.Rows[0];

			int amountOfMoney = userInfoRow.Field<int>("AmountOfMoney");
			DateTime lastUpdatedMoney = userInfoRow.Field<DateTime>("LastUpdatedMoney");
			byte[] fingerPrintIMG = userInfoRow.Field<byte[]>("FingerPrintIMG");
			DateTime? lastUpdatedFingerPrint = userInfoRow.Field<DateTime?>("LastUpdatedFingerPrint");
			int? fingerPosition = userInfoRow.Field<int?>("FingerPosition");
			DateTime insertedDate = userInfoRow.Field<DateTime>("InsertedDate");

			accountAdapter.Connection.Open();
			userInfoAdapter.Connection = accountAdapter.Connection;

			using (SqlTransaction transaction = accountAdapter.Connection.BeginTransaction())
			{
				accountAdapter.AttachTransaction(transaction);
				userInfoAdapter.AttachTransaction(transaction);

				try
				{
					userInfoAdapter.UpdateUserInfo(username, name, userTypeID, amountOfMoney, lastUpdatedMoney, fingerPrintIMG
						, lastUpdatedFingerPrint, fingerPosition, isCafeteriaStaff, isActive, insertedDate, updateBy, date, username);
					Log.ActivityLog("Update to UserInfo: username = " + username);
					accountAdapter.UpdateAccount(email, role, isActive, username);
					Log.ActivityLog("Update to Account: username = " + username);

					XmlSync.SaveUserInfoXml(username, name, userTypeID, amountOfMoney, lastUpdatedMoney, fingerPrintIMG
						, lastUpdatedFingerPrint, fingerPosition, isCafeteriaStaff, isActive, insertedDate, updateBy, date, null);
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					Log.ErrorLog(ex.Message);
				}
			}

			return RedirectToAction("ListUser", "Account");
		}

		[AllowAnonymous]
		public ActionResult DetailUser(string username)
		{
			UserinfoModel userinfo = new UserinfoModel();
			UserInfoDetailTableAdapter userinfoAdapter = new UserInfoDetailTableAdapter();
			DataTable userinfoDataTable = userinfoAdapter.GetDataByUsername(username);
			DateTime date = DateTime.Parse(userinfoDataTable.Rows[0]["LastUpdatedMoney"].ToString());
			int amountOfMoney = (int)userinfoDataTable.Rows[0]["AmountOfMoney"];

			TransactionHistoryTableAdapter transactionAdapter = new TransactionHistoryTableAdapter();
			int? money = transactionAdapter.GetCurrentMoney(username, date);
			if (money == null)
			{
				money = 0;
			}

			amountOfMoney += money.Value;

			userinfo.Username = username;
			userinfo.Name = userinfoDataTable.Rows[0]["Name"].ToString();
			userinfo.TypeName = (string)userinfoDataTable.Rows[0]["TypeName"];
			userinfo.Email = userinfoDataTable.Rows[0]["Email"].ToString();
			userinfo.AmountOfMoney = amountOfMoney;
			userinfo.Role = (int)userinfoDataTable.Rows[0]["Role"];

			return View(userinfo);
		}

		[AllowAnonymous]
		public ActionResult ViewUserType()
		{
			DataTable dataTable = new DataTable();
			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			try
			{
				dataTable = userTypeTableAdapter.GetData();
				var results = from DataRow myRow in dataTable.Rows
							  where (string)myRow["TypeShortName"] != null
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
		public ActionResult DetailsUserType(string typeShortName)
		{
			UserTypeTableAdapter userTypeDataTableAdapter = new UserTypeTableAdapter();
			DataTable dt = userTypeDataTableAdapter.GetDataByTypeShortName(typeShortName);
			UserTypeModel userTypeModel = new UserTypeModel();
			try
			{
				userTypeModel.typeShortName = Convert.ToString(dt.Rows[0]["TypeShortName"]);
				userTypeModel.typeName = Convert.ToString(dt.Rows[0]["TypeName"]);
				userTypeModel.mealValue = Convert.ToInt32(dt.Rows[0]["MealValue"]);
				userTypeModel.moreMealValue = Convert.ToInt32(dt.Rows[0]["MoreMealValue"]);
				userTypeModel.description = Convert.ToString(dt.Rows[0]["Description"]);
				userTypeModel.canDebt = Convert.ToBoolean(dt.Rows[0]["CanDebt"]);
				userTypeModel.canEatMore = Convert.ToBoolean(dt.Rows[0]["CanEatMore"]);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			if (userTypeModel == null)
			{
				return HttpNotFound();
			}
			return View(userTypeModel);
		}

		[AllowAnonymous]
		public ActionResult EditUserType(string typeShortName)
		{
			UserTypeModel userTypeModel = new UserTypeModel();
			DataTable userTypeDataTable = new DataTable();
			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			try
			{
				userTypeDataTable = userTypeTableAdapter.GetDataByTypeShortName(typeShortName);
				userTypeModel.typeShortName = Convert.ToString(userTypeDataTable.Rows[0]["TypeShortName"]);
				userTypeModel.typeName = Convert.ToString(userTypeDataTable.Rows[0]["TypeName"]);
				userTypeModel.mealValue = Convert.ToInt32(userTypeDataTable.Rows[0]["MealValue"]);
				userTypeModel.moreMealValue = Convert.ToInt32(userTypeDataTable.Rows[0]["MoreMealValue"]);
				userTypeModel.description = Convert.ToString(userTypeDataTable.Rows[0]["Description"]);
				userTypeModel.canDebt = Convert.ToBoolean(userTypeDataTable.Rows[0]["CanDebt"]);
				userTypeModel.canEatMore = Convert.ToBoolean(userTypeDataTable.Rows[0]["CanEatMore"]);
				userTypeModel.insertedDate = Convert.ToDateTime(userTypeDataTable.Rows[0]["InsertedDate"]);
				userTypeModel.lastUpdated = Convert.ToDateTime(userTypeDataTable.Rows[0]["LastUpdated"]);
				userTypeModel.updatedBy = Convert.ToString(userTypeDataTable.Rows[0]["UpdatedBy"]);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			if (userTypeModel == null)
			{
				return RedirectToAction("Error", "ErrorController");
			}
			return View(userTypeModel);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult EditUserType(UserTypeModel userTypeModel, string typeShortName)
		{
			if (!ModelState.IsValid)
			{
				return View(userTypeModel);
			}

			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			if (userTypeModel != null)
			{
				try
				{
					string typeName = userTypeModel.typeName;
					int mealValue = userTypeModel.mealValue;
					int? moreMealValue = userTypeModel.moreMealValue;
					string description = userTypeModel.description;
					Boolean canDebt = userTypeModel.canDebt;
					Boolean canEatMore = userTypeModel.canEatMore;
					DateTime insertDate = userTypeModel.insertedDate;
					DateTime lastUpdate = DateTime.Now;
					string updateBy = AccountInfo.GetUserName(Request);
					int test = userTypeTableAdapter.UpdateUserTypeByTypeShortName(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertDate, updateBy, lastUpdate, typeShortName);
					XmlSync.SaveUserTypeXml(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertDate, updateBy, lastUpdate, null);
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}
			return RedirectToAction("ViewUserType", "Account");
		}

		[AllowAnonymous]
		public ActionResult AddNewUserType()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewUserType(UserTypeModel userTypeModel)
		{
			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			if (userTypeModel != null)
			{
				try
				{
					string typeShortName = userTypeModel.typeShortName;
					string typeName = userTypeModel.typeName;
					int mealValue = userTypeModel.mealValue;
					int? moreMealValue = userTypeModel.moreMealValue;
					string description = userTypeModel.description;
					Boolean canDebt = userTypeModel.canDebt;
					Boolean canEatMore = userTypeModel.canEatMore;
					DateTime date = DateTime.Now;
					string updateBy = AccountInfo.GetUserName(Request);
					int test = userTypeTableAdapter.InsertNewUserType(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, date, updateBy, date);
					XmlSync.SaveUserTypeXml(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, date, updateBy, date, null);
					return RedirectToAction("ViewUserType", "Account");
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
