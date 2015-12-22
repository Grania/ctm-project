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
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public ActionResult UserInfo()
		{
			string username = AccountInfo.GetUserName(Request);

			UserinfoModel userinfo = new UserinfoModel();
			DataTable userInfoDataTable = new DataTable();
			UserInfoDetailTableAdapter userInfoDetailAdapter = new UserInfoDetailTableAdapter();
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
		public ActionResult Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string username = model.Username;
			string password = model.Password;
			bool remember = model.Remember;

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
				string role = AccountInfo.GetRoleNameEnglish(dt.Rows[0].Field<int>("Role"));
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				1,
				username,
				DateTime.Now,
				DateTime.Now.AddDays(20),
				remember,
				role
				);
				HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
				string cookieName = FormsAuthentication.FormsCookieName;
				string cookieValue = FormsAuthentication.Encrypt(ticket);
				cookie.Path = FormsAuthentication.FormsCookiePath;

				if (remember)
				{
					cookie.Expires = ticket.Expiration;
				}

				HttpContext.Response.Cookies.Set(cookie);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("HomePage", "Home");
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

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public ActionResult ChangePassword()
		{

			return View();
		}

		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ChangePassword model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string username = AccountInfo.GetUserName(Request);

			string oldPassword = model.OldPassword;
			string newPassword = model.Password;

			AccountTableAdapter adapter = new AccountTableAdapter();
			DataTable dt = adapter.GetDataByUsername(username);

			if (!dt.Rows[0]["PASSWORD"].ToString().Equals(oldPassword))
			{
				ModelState.AddModelError("", "Nhập sai mật khẩu cũ.");
				return View();
			}

			AccountTableAdapter AccountAdapter = new AccountTableAdapter();

			try
			{
				AccountAdapter.ChangePassword(newPassword, username);
				Log.ActivityLog("Account: " + username + "change password!");
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

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

						AccountAdapter.Insert(username, password, email, 1);
						Log.ActivityLog("Insert into Account: Username = " + username);

						XmlSync.SaveUserInfoXml(username, name, userType, 0, date, null, null, null, false, false, date, username, date, null);
						transaction.Commit();

						Session["register"] = "Đăng ký thành công!";
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						Log.ErrorLog(ex.Message);
						Session["register"] = "Đăng ký thất bại!";
					}
				}
			}

			return RedirectToAction("HomePage", "Home");
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult ListUser()
		{
			UserInfoDetailTableAdapter userInfoAdapter = new UserInfoDetailTableAdapter();
			DataTable userDT = new DataTable();

			string username = Request.QueryString["username"];
			string name = Request.QueryString["name"];
			string userType = Request.QueryString["userType"];
			string role = Request.QueryString["role"];
			string active = Request.QueryString["active"];

			string page = Request.QueryString["page"];
			string amountPerPage = Request.QueryString["amountPerPage"];

			if (String.IsNullOrWhiteSpace(userType))
			{
				userType = null;
			}

			int role_;
			if (!int.TryParse(role, out role_))
			{
				role = null;
			}

			int active_;
			if (!int.TryParse(active, out active_))
			{
				active = null;
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
				string query = "SELECT * FROM ( SELECT UserInfo.Username, UserInfo.Name, UserInfo.TypeShortName, "
					+ "UserInfo.AmountOfMoney, UserInfo.LastUpdatedMoney, UserInfo.FingerPosition, UserInfo.IsCafeteriaStaff, "
					+ "UserInfo.InsertedDate, UserInfo.UpdatedBy, UserInfo.LastUpdated, Account.Email, Account.Role, "
					+ "UserType.TypeName, UserType.MealValue, UserType.MoreMealValue, UserType.CanDebt, "
					+ "UserType.CanEatMore, UserInfo.IsActive, ROW_NUMBER() OVER (order by UserInfo.InsertedDate DESC) AS RowNum "
					+ "FROM UserInfo INNER JOIN Account ON UserInfo.Username = Account.Username INNER JOIN "
					+ "UserType ON UserInfo.TypeShortName = UserType.TypeShortName ";
				string countQuery = "SELECT COUNT(UserInfo.Username) FROM UserInfo INNER JOIN Account "
					+ "ON UserInfo.Username = Account.Username ";
				string conditionQuery = "";

				if (username != null || name != null || userType != null || role != null || active != null)
				{
					conditionQuery += "WHERE ";
					bool isFirst = false;

					if (username != null)
					{
						conditionQuery += "UserInfo.Username like '%" + username + "%' ";
						isFirst = true;
					}

					if (name != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "UserInfo.Name like N'%" + name + "%' ";
						isFirst = true;
					}

					if (userType != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "UserInfo.TypeShortName = '" + userType + "' ";
						isFirst = true;
					}

					if (role != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "Account.Role = " + role + " ";
						isFirst = true;
					}

					if (active != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "UserInfo.IsActive = " + active + " ";
					}
				}

				int minRowNum = ((page_ - 1) * amountPerPage_) + 1;
				int maxRowNum = page_ * amountPerPage_;
				query += conditionQuery;
				query += ") AS UI WHERE UI.RowNum BETWEEN " + minRowNum + " AND " + maxRowNum + " ";

				SqlCommand countCmd = new SqlCommand(countQuery + conditionQuery, userInfoAdapter.Connection);
				SqlCommand getDataCmd = new SqlCommand(query, userInfoAdapter.Connection);
				SqlDataAdapter getDataAdapter = new SqlDataAdapter(getDataCmd);

				userInfoAdapter.Connection.Open();
				int count = (int)countCmd.ExecuteScalar();

				getDataAdapter.Fill(userDT);

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

				ViewBag.roleList = AccountInfo.GetRoleListVnese();

				DataTable userTypeDT = new UserTypeTableAdapter().GetData();
				List<KeyValuePair<string, string>> userTypes = new List<KeyValuePair<string, string>>();
				foreach (DataRow row in userTypeDT.Rows)
				{
					userTypes.Add(new KeyValuePair<string, string>(row.Field<string>("TypeShortName"), row.Field<string>("TypeName")));
				}
				ViewBag.userTypes = userTypes;
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}

			return View(userDT);
		}

		[Authorize(Roles = "Administrator")]
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

		[Authorize(Roles = "Administrator")]
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

			try
			{
				userInfo.Username = username;
				userInfo.Name = userinfoDataTable.Rows[0]["Name"].ToString();
				userInfo.UserTypeID = (string)userinfoDataTable.Rows[0]["TypeShortName"];
				userInfo.Email = userinfoDataTable.Rows[0]["Email"].ToString();
				userInfo.Role = (int)userinfoDataTable.Rows[0]["Role"];
				userInfo.isActive = (bool)userinfoDataTable.Rows[0]["IsActive"];

				return View(userInfo);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "Error");
			}
		}

		[Authorize(Roles = "Administrator")]
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
				DataTable dt = AccountAdapter.GetDataByUsername(model.Username);

				if (!dt.Rows[0]["Email"].ToString().Equals(email))
				{
					DataTable AccountDT = AccountAdapter.GetDataByEmail(email);

					if (AccountDT.Rows.Count == 1)
					{
						if (!AccountDT.Rows[0]["Username"].ToString().Equals(model.Username))
						{
							Log.ActivityLog(AccountDT.Rows[0]["Username"].ToString().Equals(model.Username).ToString());
							ModelState.AddModelError("", "Email đã tồn tại");
							return View(model);
						}
					}
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
					accountAdapter.UpdateAccount(email, role, username);
					Log.ActivityLog("Update to Account: username = " + username);

					transaction.Commit();
					XmlSync.SaveUserInfoXml(username, name, userTypeID, amountOfMoney, lastUpdatedMoney, fingerPrintIMG
						, lastUpdatedFingerPrint, fingerPosition, isCafeteriaStaff, isActive, insertedDate, updateBy, date, null);

					Session["editUser"] = "Cập nhật thành công!";
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					Log.ErrorLog(ex.Message);
					Session["editUser"] = "Cập nhật thất bại!";
				}
			}

			return RedirectToAction("EditUser", "Account", new { @username = model.Username });
		}

		[Authorize(Roles = "Administrator")]
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

		[Authorize(Roles = "Administrator")]
		public ActionResult ViewUserType()
		{
			DataTable dataTable = new DataTable();
			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			try
			{
				dataTable = userTypeTableAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return View(dataTable);
		}

		[Authorize(Roles = "Administrator")]
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

		[Authorize(Roles = "Administrator")]
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
				return RedirectToAction("Error", "Error");
			}
			return View(userTypeModel);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditUserType(UserTypeModel userTypeModel, string typeShortName)
		{
			if (!ModelState.IsValid)
			{
				return View(userTypeModel);
			}

			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();
			try
			{
				string typeName = userTypeModel.typeName;
				int mealValue = userTypeModel.mealValue;
				int? moreMealValue = 0;

				if (!string.IsNullOrEmpty(userTypeModel.moreMealValue.ToString()))
				{
					moreMealValue = userTypeModel.moreMealValue;
				}

				string description = userTypeModel.description;
				Boolean canDebt = userTypeModel.canDebt;
				Boolean canEatMore = userTypeModel.canEatMore;
				DateTime insertDate = userTypeModel.insertedDate;
				DateTime lastUpdate = DateTime.Now;
				string updateBy = AccountInfo.GetUserName(Request);
				userTypeTableAdapter.UpdateUserTypeByTypeShortName(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertDate, updateBy, lastUpdate, typeShortName);
				XmlSync.SaveUserTypeXml(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, insertDate, updateBy, lastUpdate, null);
				Session["editUserType"] = "Cập nhật thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["editUserType"] = "Cập nhật thất bại!";
			}
			return RedirectToAction("EditUserType", "Account", new { @typeShortName = typeShortName });
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult AddNewUserType()
		{
			return View();
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewUserType(UserTypeModel userTypeModel)
		{
			if (!ModelState.IsValid)
			{
				return View(userTypeModel);
			}

			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();

			try
			{
				string typeShortName = userTypeModel.typeShortName;
				string typeName = userTypeModel.typeName;
				int mealValue = userTypeModel.mealValue;
				int? moreMealValue = 0;

				if (!string.IsNullOrEmpty(userTypeModel.moreMealValue.ToString()))
				{
					moreMealValue = userTypeModel.moreMealValue;
				}

				string description = userTypeModel.description;
				Boolean canDebt = userTypeModel.canDebt;
				Boolean canEatMore = userTypeModel.canEatMore;
				DateTime date = DateTime.Now;
				string updateBy = AccountInfo.GetUserName(Request);
				userTypeTableAdapter.InsertNewUserType(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, date, updateBy, date);
				XmlSync.SaveUserTypeXml(typeShortName, typeName, mealValue, moreMealValue, description, canDebt, canEatMore, date, updateBy, date, null);
				Session["addUserType"] = "Thêm mới thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["addUserType"] = "Thêm mới thất bại!";
			}
			return RedirectToAction("AddNewUserType", "Account");
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult DeleteUserType(string typeShortName)
		{
			UserTypeTableAdapter userTypeTableAdapter = new UserTypeTableAdapter();

			try
			{
				userTypeTableAdapter.Delete(typeShortName);
				Session["deleteUserType"] = "Xóa thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["deleteUserType"] = "Xóa thất bại! Loại người dùng này đang được sử dụng.";
			}

			return RedirectToAction("ViewUserType", "Account");
		}

		[Authorize(Roles = "Administrator, Manager")]
		public JsonResult GetUsernameList()
		{
			string query = "SELECT Username FROM UserInfo";

			UserInfoTableAdapter userInfoTA = new UserInfoTableAdapter();
			userInfoTA.Connection.Open();

			SqlCommand getDataCmd = new SqlCommand(query, userInfoTA.Connection);
			SqlDataAdapter adapter = new SqlDataAdapter(getDataCmd);

			DataTable dt = new DataTable();

			try
			{
				adapter.Fill(dt);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return Json("error", JsonRequestBehavior.AllowGet);
			}

			List<string> usernameList = new List<string>();
			foreach (DataRow row in dt.Rows)
			{
				usernameList.Add(row.Field<string>("Username"));
			}

			return Json(new { result = usernameList }, JsonRequestBehavior.AllowGet);
		}
	}
}
