﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult Unauthorization()
		{
			return View();
		}

		public ActionResult Error()
		{
			return View();
		}
	}
}
