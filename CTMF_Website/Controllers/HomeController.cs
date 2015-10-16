using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class HomeController : Controller
	{
		[Authorize(Roles = "admin")]
		public ActionResult Index()
		{
			return View();
		}

	}
}
