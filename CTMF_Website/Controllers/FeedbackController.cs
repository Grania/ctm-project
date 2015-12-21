using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class FeedbackController : Controller
	{
		[Authorize(Roles = ("Customer, Cafeteria Staff, Manager, Administrator"))]
		public JsonResult Feedback(string content)
		{
			DateTime date = DateTime.Now;
			string username = AccountInfo.GetUserName(Request);
			var message = "Gửi phản hồi thành công!";

			try
			{
				FeedbackTableAdapter feedbackAdapter = new FeedbackTableAdapter();
				feedbackAdapter.Insert(content, date, username);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				message = "Gửi phản hồi thất bại!";
			}

			return Json(message, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListFeedback()
		{
			DataTable feedbackDT = new DataTable();

			try
			{
				FeedbackTableAdapter feedbackAdapter = new FeedbackTableAdapter();
				feedbackDT = feedbackAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "Error");
			}

			return View(feedbackDT);
		}
	}
}
