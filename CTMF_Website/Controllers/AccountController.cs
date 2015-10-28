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
		public ActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(BigViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string username = model.LoginViewModel.Username;
			string password = model.LoginViewModel.Password;

			AccountTableAdapter adapter = new AccountTableAdapter();
			DataTable dt = adapter.GetDataByUsrename(username);

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

			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				1,
				username,
				DateTime.Now,
				DateTime.Now.AddMilliseconds(20),
				false,
				"admin"
				);
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
			string cookieName = FormsAuthentication.FormsCookieName;
			string cookieValue = FormsAuthentication.Encrypt(ticket);
			HttpContext.Response.Cookies.Set(new HttpCookie(cookieName, cookieValue));

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
					UserInfoAdapter.InsertUserInfo(username, name, null, 0, date, null, null, false, false, date, username, date);
					AccountTableAdapter AccountAdapter = new AccountTableAdapter();
					AccountAdapter.InsertAccount(username, password, email, 1, false);
				}
				catch(Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}

			return RedirectToAction("HomePage", "Home");
		}
	}
}
