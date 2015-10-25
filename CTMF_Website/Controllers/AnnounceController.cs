using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class AnnounceController : Controller
	{
		//
		// GET: /Announce/

		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ViewAnnounce()
		{
			return View();
		}
	}
}
