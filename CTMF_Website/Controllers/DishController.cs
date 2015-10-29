using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
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

		//[AllowAnonymous]
		//public ActionResult ViewDish()
		//{
		//	DishTableAdapter adapter = new DishTableAdapter();
		//	DataTable dt = adapter.GetData();
		//	BigViewModel BigViewModel = new BigViewModel();
		//	BigViewModel.DataTableModel = dt;
		//	return View(BigViewModel);
		//}

		[AllowAnonymous]
		public ActionResult ViewDish(int blogPostId)
		{
			BigViewModel BigViewModel = new BigViewModel();
			DishTableAdapter adapter = new DishTableAdapter();
			DataTable dt = new DataTable();
			if (blogPostId != 0)
			{
				dt = adapter.GetDataBy(blogPostId);
			}
			else
			{
				dt = adapter.GetData();
			}
			BigViewModel.DataTableModel = dt;
			return View(BigViewModel);
		}
	}
}
