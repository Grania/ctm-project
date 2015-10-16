using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			string username = model.Username;
			string password = model.Password;

			ACCOUNTTableAdapter adapter = new ACCOUNTTableAdapter();
			DataTable dt = adapter.GetDataBy(username);

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

			return RedirectToAction("Index", "Home");
		}
	}
}
