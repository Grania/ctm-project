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
		//
		// GET: /Announce/

		public ActionResult Index()
		{
			return View();
		}

		//GET:/List Announce
		[AllowAnonymous]
		public ActionResult ViewAnnounce()
		{
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = announcementAdapter.GetData();

			return View(dt);
		}

		[AllowAnonymous]
		public ActionResult DetailsAnnounce(string announceID)
		{
			int announceId = Convert.ToInt32(announceID);
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = announcementAdapter.GetDataByAnnounceID(announceId);
			AnnounceModel announce = new AnnounceModel();
			try
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					announce.annoucemenID = Convert.ToInt32(dt.Rows[i]["AnnouncementID"]);
					announce.title = Convert.ToString(dt.Rows[i]["Title"]);
					announce.isAuto = Convert.ToBoolean(dt.Rows[i]["IsAuto"]);
					announce.insertDate = Convert.ToDateTime(dt.Rows[i]["InsertedDate"]);
					announce.lastUpdate = Convert.ToDateTime(dt.Rows[i]["LastUpdated"]);
					announce.subject = Convert.ToString(dt.Rows[i]["Subject"]);
					announce.updateBy = Convert.ToString(dt.Rows[i]["UpdatedBy"]);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			if (announce == null)
			{
				return HttpNotFound();
			}
			return View(announce);
		}

		[AllowAnonymous]
		public ActionResult AddNewAnnounce(AnnounceModel announceModel)
		{
			if (announceModel != null)
			{
				try
				{
					string updateBy = AccountInfo.GetUserName(Request);
					AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
					string title = announceModel.title;
					string subject = announceModel.subject;
					Boolean isAuto = false;
					DateTime date = DateTime.Now;
					announceTableAdapter.InsertNewAnnounce(title, subject, isAuto, date, updateBy, date);
					return RedirectToAction("ViewAnnounce", "Announce");
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}
				return View(); 
				
		}

		[AllowAnonymous]
		public ActionResult EditAnnounce(string announceID)
		{
			AnnounceModel announcement = new AnnounceModel();
			int announceId = Convert.ToInt32(announceID);
			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable announcementTable = announcementAdapter.GetDataByAnnounceID(announceId);
			try
			{
				announcement.annoucemenID = Convert.ToInt32(announcementTable.Rows[0]["AnnouncementID"]);
				announcement.title = Convert.ToString(announcementTable.Rows[0]["Title"]);
				announcement.isAuto = Convert.ToBoolean(announcementTable.Rows[0]["IsAuto"]);
				announcement.insertDate = Convert.ToDateTime(announcementTable.Rows[0]["InsertedDate"]);
				announcement.lastUpdate = Convert.ToDateTime(announcementTable.Rows[0]["LastUpdated"]);
				announcement.subject = Convert.ToString(announcementTable.Rows[0]["Subject"]);
				announcement.updateBy = Convert.ToString(announcementTable.Rows[0]["UpdatedBy"]);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			if (announcement == null)
			{
				return RedirectToAction("Error", "ErrorController");
			}
			return View(announcement);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult EditAnnounce(AnnounceModel announceModel,string announceID)
		{
			int announceId = Convert.ToInt32(announceID);
			AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
			if (announceModel != null)
			{
				try
				{
					string updateBy = AccountInfo.GetUserName(Request);
					string title = announceModel.title;
					string subject = announceModel.subject;
					Boolean isAuto = false;
					DateTime date = DateTime.Now;
					announceTableAdapter.UpdateAnnounce(title, subject, isAuto, date, updateBy, date, announceId);
					return RedirectToAction("ViewAnnounce", "Announce");
				}
				catch (Exception ex)
				{
					Log.ErrorLog(ex.Message);
				}
			}
					return RedirectToAction("ViewAnnounce", "Announce");
			}

		public ActionResult DeleteAnnounce(int announceID)
		{
			AnnouncementTableAdapter announceTableAdapter = new AnnouncementTableAdapter();
			try {
				announceTableAdapter.DeleteAnnounceByID(announceID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return RedirectToAction("ViewAnnounce", "Announce");
		}

		}

	}


