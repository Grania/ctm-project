using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class ReportController : Controller
	{
		//
		// GET: /Report/

		[AllowAnonymous]
		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Test()
		{
			DateTime monthBegin = DateTime.Parse("2015-12-1");
			DataTable dt = new CTMF_Website.DataAccessTableAdapters.TransactionHistoryTableAdapter().GetDataByDate(monthBegin, DateTime.Now);

			CTMF_Website.Util.ExcelReport.ExportToExcel(dt, "abc");
			return RedirectToAction("Index", "Report");
		}
	}
}
