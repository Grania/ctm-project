using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class AnnounceController : Controller
	{
		[AllowAnonymous]
		public ActionResult ViewAnnounce(string title, string date, string page, string amountPerPage)
		{
			ViewBag.error = "";
			string username = AccountInfo.GetUserName(Request);

			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = new DataTable();

			if (String.IsNullOrWhiteSpace(title))
			{
				title = null;
			}

			DateTime date_;
			if (String.IsNullOrWhiteSpace(date))
			{
				date = null;
			}
			else if (!DateTime.TryParseExact(date, "dd/MM/yyyy", null
				, System.Globalization.DateTimeStyles.None
				, out date_))
			{
				ViewBag.error = "Sai định dạng ngày, tháng";
				date = null;
			}
			else
			{
				date = date_.ToString("yyyy-MM-dd");
			}

			int page_;
			if (!int.TryParse(page, out page_))
			{
				page = null;
				page_ = 1;
			}

			int amountPerPage_;
			if (!int.TryParse(amountPerPage, out amountPerPage_))
			{
				amountPerPage = "10";
				amountPerPage_ = 10;
			}

			try
			{
				string query = "SELECT * FROM( SELECT *, ROW_NUMBER() OVER (ORDER BY insertedDate DESC) AS RowNum "
					+ "FROM Announcement ";
				string conditionQuery = "";
				string countQuery = "SELECT COUNT(AnnouncementID) FROM Announcement ";

				if (title != null || date != null)
				{
					conditionQuery += "WHERE ";
					bool isFirst = false;

					if (title != null)
					{
						conditionQuery += "Title like '%" + title + "%' ";
						isFirst = true;
					}

					if (date != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "InsertedDate BETWEEN '" + date + " 00:00:000' AND '" + date + " 23:59:59:999' ";
					}
				}

				announcementAdapter.Connection.Open();

				SqlCommand countCmd = new SqlCommand(countQuery + conditionQuery, announcementAdapter.Connection);
				int count = (int)countCmd.ExecuteScalar();

				int maxPage = (count / amountPerPage_);
				if (count % amountPerPage_ != 0)
				{
					maxPage++;
				}

				ViewBag.maxPage = maxPage;
				if (page_ > maxPage)
				{
					page_ = maxPage;
				}

				ViewBag.curPage = page_;
				ViewBag.amountPerPage = amountPerPage_;

				int minRowNum = ((page_ - 1) * amountPerPage_) + 1;
				int maxRowNum = page_ * amountPerPage_;

				query += conditionQuery;
				query += ") AS SOD WHERE SOD.RowNum BETWEEN (" + minRowNum + ") AND (" + maxRowNum + ") ";

				SqlCommand getDataCmd = new SqlCommand(query, announcementAdapter.Connection);
				SqlDataAdapter getDataAdapter = new SqlDataAdapter(getDataCmd);

				getDataAdapter.Fill(dt);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}

			return View(dt);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult ListAnnounce(string title, string date, string page, string amountPerPage)
		{
			ViewBag.error = "";
			string username = AccountInfo.GetUserName(Request);

			AnnouncementTableAdapter announcementAdapter = new AnnouncementTableAdapter();
			DataTable dt = new DataTable();

			if (String.IsNullOrWhiteSpace(title))
			{
				title = null;
			}

			DateTime date_;
			if (String.IsNullOrWhiteSpace(date))
			{
				date = null;
			}
			else if(!DateTime.TryParseExact(date, "dd/MM/yyyy", null
				, System.Globalization.DateTimeStyles.None
				, out date_))
			{
				ViewBag.error = "Sai định dạng ngày, tháng";
				date = null;
			}
			else
			{
				date = date_.ToString("yyyy-MM-dd");
			}

			int page_;
			if (!int.TryParse(page, out page_))
			{
				page = null;
				page_ = 1;
			}

			int amountPerPage_;
			if (!int.TryParse(amountPerPage, out amountPerPage_))
			{
				amountPerPage = "10";
				amountPerPage_ = 10;
			}

			try
			{
				string query = "SELECT * FROM( SELECT *, ROW_NUMBER() OVER (ORDER BY insertedDate DESC) AS RowNum "
					+ "FROM Announcement ";
				string conditionQuery = "";
				string countQuery = "SELECT COUNT(AnnouncementID) FROM Announcement ";

				if (title != null || date != null)
				{
					conditionQuery += "WHERE ";
					bool isFirst = false;

					if (title != null)
					{
						conditionQuery += "Title like '%"+title+"%' ";
						isFirst = true;
					}

					if (date != null)
					{
						if (isFirst)
						{
							conditionQuery += "AND ";
						}
						conditionQuery += "InsertedDate BETWEEN '" + date + " 00:00:000' AND '" + date + " 23:59:59:999' ";
					}
				}

				announcementAdapter.Connection.Open();

				SqlCommand countCmd = new SqlCommand(countQuery + conditionQuery, announcementAdapter.Connection);
				int count = (int)countCmd.ExecuteScalar();

				int maxPage = (count / amountPerPage_);
				if (count % amountPerPage_ != 0)
				{
					maxPage++;
				}

				ViewBag.maxPage = maxPage;
				if (page_ > maxPage)
				{
					page_ = maxPage;
				}

				ViewBag.curPage = page_;
				ViewBag.amountPerPage = amountPerPage_;

				int minRowNum = ((page_ - 1) * amountPerPage_) + 1;
				int maxRowNum = page_ * amountPerPage_;

				query += conditionQuery;
				query += ") AS SOD WHERE SOD.RowNum BETWEEN (" + minRowNum + ") AND (" + maxRowNum + ") ";

				SqlCommand getDataCmd = new SqlCommand(query, announcementAdapter.Connection);
				SqlDataAdapter getDataAdapter = new SqlDataAdapter(getDataCmd);

				getDataAdapter.Fill(dt);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return View("Error");
			}

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


