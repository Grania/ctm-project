using CTMF_Website.DataAccessTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class DishController : Controller
	{
		//
		// GET: /Dish/

		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ViewDish()
		{
			DishTableAdapter adapter = new DishTableAdapter();
			DataTable dt = adapter.GetData();

			return View();
		}

	}
}
