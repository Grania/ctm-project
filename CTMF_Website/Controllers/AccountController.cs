using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CTMF_Website.Controllers
{
	public class AccountController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

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

		[AllowAnonymous]
		public ActionResult UserInfo()
		{
			string username = AccountInfo.GetUserName(Request);

			Userinfo userinfo = new Userinfo();
			UserInfoDetailTableAdapter userinfoAdapter = new UserInfoDetailTableAdapter();
			DataTable userinfoDataTable = userinfoAdapter.GetDataByUsername(username);
			DateTime date = DateTime.Parse(userinfoDataTable.Rows[0]["LastUpdatedMoney"].ToString());
			int amountOfMoney = (int)userinfoDataTable.Rows[0]["AmountOfMoney"];

			TransactionHistoryTableAdapter transactionAdapter = new TransactionHistoryTableAdapter();
			DataTable transactionDataTable = transactionAdapter.GetDataByDate(date, username);

			foreach (DataRow row in transactionDataTable.Rows)
			{
				try
				{
					if (row["TransactionTypeID"].Equals(2))
					{
						amountOfMoney += (int)row["Value"];
					}
					else
					{
						amountOfMoney -= (int)row["Value"];
					}
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}

			userinfo.Username = username;
			userinfo.Name = (string)userinfoDataTable.Rows[0]["Name"];
			userinfo.TypeName = (string)userinfoDataTable.Rows[0]["TypeName"];
			userinfo.Email = (string)userinfoDataTable.Rows[0]["Email"];
			userinfo.AmountOfMoney = amountOfMoney;

			return View(userinfo);
		}

		//[AllowAnonymous]
		//public ActionResult UserInfo(Userinfo model)
		//{
		//	return View();
		//}

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
			catch(Exception ex)
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
				try
				{
					UserInfoAdapter.InsertUserInfo(username, name, null, 0, date, null,null, null, false, false, date, username, date);
					AccountTableAdapter AccountAdapter = new AccountTableAdapter();
					AccountAdapter.InsertAccount(username, password, email, 1, false);
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}

			return RedirectToAction("HomePage", "Home");
		}
	}
}
