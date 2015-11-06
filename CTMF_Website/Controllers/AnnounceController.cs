using CTMF_Website.DataAccessTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = announcementAdapter.GetData();

			return View(dt);
		}

		[AllowAnonymous]
		public ActionResult Details(int id)
		{
			return View();
		}
	}
}
