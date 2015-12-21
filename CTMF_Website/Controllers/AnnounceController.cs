using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
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
		[AllowAnonymous]
		public ActionResult ViewAnnounce()
		{
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = announcementAdapter.GetData();

			return View(dt);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult ListAnnounce()
		{
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = announcementAdapter.GetData();

			return View(dt);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult AddNewAnnounce()
		{
			return View();
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddNewAnnounce(AnnounceModel announceModel)
		{
			if (!ModelState.IsValid)
			{
				return View(announceModel);
			}

			try
			{
				string updateBy = AccountInfo.GetUserName(Request);
				AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
				string title = announceModel.title;
				string subject = announceModel.subject;
				Boolean isAuto = false;
				DateTime date = DateTime.Now;
				announceTableAdapter.InsertNewAnnounce(title, subject, isAuto, date, updateBy, date);
				Session["addAnnounce"] = "Đăng tin thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["addAnnounce"] = "Đăng tin thất bại!";
			}
			return RedirectToAction("AddNewAnnounce", "Announce");
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult EditAnnounce(string announceID)
		{
			AnnounceModel announcement = new AnnounceModel();
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			try
			{
				int announceId = Convert.ToInt32(announceID);
				DataTable announcementTable = announcementAdapter.GetDataByAnnounceID(announceId);
				announcement.annoucemenID = Convert.ToInt32(announcementTable.Rows[0]["AnnouncementID"]);
				announcement.title = Convert.ToString(announcementTable.Rows[0]["Title"]);
				announcement.isAuto = Convert.ToBoolean(announcementTable.Rows[0]["IsAuto"]);
				announcement.subject = Convert.ToString(announcementTable.Rows[0]["Subject"]);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "ErrorController");
			}
			return View(announcement);
		}

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
		public ActionResult EditAnnounce(AnnounceModel announceModel, string announceID)
		{
			AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
			try
			{
				int announceId = Convert.ToInt32(announceID);
				string updateBy = AccountInfo.GetUserName(Request);
				string title = announceModel.title;
				string subject = announceModel.subject;
				Boolean isAuto = false;
				DateTime date = DateTime.Now;
				announceTableAdapter.UpdateAnnounce(title, subject, isAuto, updateBy, date, announceId);
				Session["editAnnounce"] = "Cập nhật thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["editAnnounce"] = "Cập nhật thất bại!";
			}
			return RedirectToAction("EditAnnounce", "Announce", new { @announceID = announceID });
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult DeleteAnnounce(int announceID)
		{
			AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
			try
			{
				announceTableAdapter.DeleteAnnounceByID(announceID);
				Session["deleteAnnounce"] = "Xóa thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["deleteAnnounce"] = "Xóa thất bại!";
			}
			return RedirectToAction("ListAnnounce", "Announce");
		}

	}
}


