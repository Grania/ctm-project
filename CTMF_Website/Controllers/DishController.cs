﻿using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class DishController : Controller
	{
		DataTable dishDT = new DataTable();

		[AllowAnonymous]
		public ActionResult ViewDish(string search, string filter)
		{
			DishTableAdapter adapter = new DishTableAdapter();

			try
			{
				dishDT = adapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			if (!(string.IsNullOrEmpty(search) && string.IsNullOrEmpty(filter)))
			{
				int type = -1;
				int.TryParse(filter, out type);

				string name = search;
				if (name == null)
				{
					name = String.Empty;
				}

				var result = from row in dishDT.AsEnumerable()
							 where (name == String.Empty ? true : StringExtensions.ContainsInsensitive(row.Field<string>("Name"), name))
							 && (type < 1 ? true : row.Field<int>("DishTypeID") == type)
							 select row;

				return View(result.CopyToDataTable());
			}

			return View(dishDT);
		}

		[AllowAnonymous]
		public ActionResult ListDish(string search, string filter)
		{
			DishInfoDetailTableAdapter adapter = new DishInfoDetailTableAdapter();

			try
			{
				dishDT = adapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			if (!(string.IsNullOrEmpty(search) && string.IsNullOrEmpty(filter)))
			{
				int type = -1;
				int.TryParse(filter, out type);

				string name = search;
				if (name == null)
				{
					name = String.Empty;
				}

				var result = from row in dishDT.AsEnumerable()
							 where (name == String.Empty ? true : StringExtensions.ContainsInsensitive(row.Field<string>("Name"), name))
							 && (type < 1 ? true : row.Field<int>("DishTypeID") == type)
							 select row;

				return View(result.CopyToDataTable());
			}

			return View(dishDT);
		}

		[AllowAnonymous]
		public ActionResult AddDish()
		{
			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in dishTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["DishTypeID"].ToString() });
			}
			ViewData["DishType"] = items;
			ViewBag.CheckRunTimes = "1";

			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddDish(DishViewModel model)
		{
			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in dishTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["DishTypeID"].ToString() });
			}
			ViewData["DishType"] = items;
			ViewBag.CheckRunTimes = "2";

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string updateBy = AccountInfo.GetUserName(Request);
			DateTime date = DateTime.Now;
			string dishName = model.Dishname;
			int dishTypeID = model.DishTypeID;
			string description = model.Description;

			string savePath = null;

			DishTableAdapter dishAdapter = new DishTableAdapter();
			DataTable dishDT = dishAdapter.GetByName(dishName);

			foreach (DataRow row in dishDT.Rows)
			{
				if (StringExtensions.EqualsInsensitive(row["Name"].ToString(), dishName))
				{
					ModelState.AddModelError("", "Tên món ăn đã tồn tại.");
					return View(model);
				}
			}

			if (!string.IsNullOrEmpty(model.Image))
			{
				savePath = DishImagesPath + "\\" + dishName.Replace(" ", "_") + ".jpg";
				var sourcePath = HttpContext.Server.MapPath(model.Image);
				var destinationPath = HttpContext.Server.MapPath(savePath);

				System.IO.File.Move(sourcePath, destinationPath);
			}

			try
			{
				dishAdapter.InsertDish(dishName, dishTypeID, description, savePath, date, updateBy, date);
				Log.ActivityLog("Insert into Dish Table: DishName = " + dishName);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return RedirectToAction("ListDish", "Dish");
		}

		private const int AvatarScreenWidth = 500;

		private const string TempFolder = "/Temp";
		private const string MapTempFolder = "~" + TempFolder;
		private const string DishImagesPath = "\\Images\\DishImages";

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult UploadImage()
		{
			HttpPostedFileBase files = Request.Files[0] as HttpPostedFileBase;
			if (files == null)
			{
				var errorData = new
				{
					success = false,
					errorMessage = "No file uploaded."
				};
				return Json(errorData);
			}
			if (!files.ContentType.Contains("image"))
			{
				var errorData = new
				{
					success = false,
					errorMessage = "File is of wrong format."
				};
				return Json(errorData);
			}
			if (files.ContentLength <= 0)
			{
				var errorData = new
				{
					success = false,
					errorMessage = "File cannot be zero length."
				};
				return Json(errorData);
			}
			var webPath = GetTempSavedFilePath(files);
			var successData = new
			{
				success = true,
				fileName = webPath.Replace("/", "\\")
			};
			return Json(successData);
		}

		private string GetTempSavedFilePath(HttpPostedFileBase file)
		{
			var serverPath = HttpContext.Server.MapPath(TempFolder);
			if (Directory.Exists(serverPath) == false)
			{
				Directory.CreateDirectory(serverPath);
			}

			var fileName = Path.GetFileName(file.FileName);
			fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

			CleanUpTempFolder(1);
			return Path.Combine(TempFolder, fileName);
		}

		private static string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
		{
			var img = new WebImage(file.InputStream);
			var ratio = img.Height / (double)img.Width;
			img.Resize(AvatarScreenWidth, (int)(AvatarScreenWidth * ratio));

			var fullFileName = Path.Combine(serverPath, fileName);
			if (System.IO.File.Exists(fullFileName))
			{
				System.IO.File.Delete(fullFileName);
			}

			img.Save(fullFileName);
			return Path.GetFileName(img.FileName);
		}

		private void CleanUpTempFolder(int hoursOld)
		{
			try
			{
				var currentUtcNow = DateTime.UtcNow;
				var serverPath = HttpContext.Server.MapPath(TempFolder);
				if (!Directory.Exists(serverPath)) return;
				var fileEntries = Directory.GetFiles(serverPath);
				foreach (var fileEntry in fileEntries)
				{
					var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
					var res = currentUtcNow - fileCreationTime;
					if (res.TotalHours > hoursOld)
					{
						System.IO.File.Delete(fileEntry);
					}
				}
			}
			catch
			{

			}
		}

		[AllowAnonymous]
		public ActionResult EditDish(string dishID)
		{
			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in dishTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["DishTypeID"].ToString() });
			}
			ViewData["DishType"] = items;

			int id = int.Parse(dishID);
			EditDishModel model = new EditDishModel();
			DishTableAdapter dishAdapter = new DishTableAdapter();
			DataTable dishDT = dishAdapter.GetDataByDishID(id);
			model.DishID = id;
			model.Dishname = dishDT.Rows[0]["Name"].ToString();
			model.DishTypeID = (int)dishDT.Rows[0]["DishTypeID"];
			model.Description = dishDT.Rows[0]["Description"].ToString();
			model.Image = dishDT.Rows[0]["Image"].ToString();
			return View(model);

		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditDish(EditDishModel model)
		{
			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();
			foreach (DataRow row in dishTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["DishTypeID"].ToString() });
			}
			ViewData["DishType"] = items;

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			DishTableAdapter dishAdapter = new DishTableAdapter();

			string updateBy = AccountInfo.GetUserName(Request);
			DateTime date = DateTime.Now;
			int dishID = model.DishID;
			string dishName = model.Dishname;
			int dishTypeID = model.DishTypeID;
			string description = model.Description;

			DataTable dt = dishAdapter.GetDataByDishID(dishID);
			string savePath = dt.Rows[0]["Image"].ToString();

			if (!StringExtensions.EqualsInsensitive(dt.Rows[0]["Name"].ToString(), dishName))
			{
				DataTable dishDT = dishAdapter.GetByName(dishName);

				foreach (DataRow row in dishDT.Rows)
				{
					if (StringExtensions.EqualsInsensitive(row["Name"].ToString(), dishName))
					{
						ModelState.AddModelError("", "Tên món ăn đã tồn tại.");
						return View(model);
					}
				}
				savePath = DishImagesPath + "\\" + dishName.Replace(" ", "_") + ".jpg";
			}

			if (model.Image != null)
			{
				if (!StringExtensions.EqualsInsensitive(savePath, model.Image))
				{
					savePath = DishImagesPath + "\\" + dishName.Replace(" ", "_") + ".jpg";
					var sourcePath = HttpContext.Server.MapPath(model.Image);
					var destinationPath = HttpContext.Server.MapPath(savePath);

					System.IO.File.Move(sourcePath, destinationPath);

					string oldImage = dt.Rows[0]["Image"].ToString();
					if (!string.IsNullOrEmpty(oldImage))
					{
						var oldImagePath = HttpContext.Server.MapPath(oldImage);
						if (System.IO.File.Exists(oldImagePath))
						System.IO.File.Delete(oldImagePath);
					}

				}
			}
			else
			{
				savePath = null;
			}

			try
			{
				dishAdapter.UpdateDish(dishName, dishTypeID, description, savePath, updateBy, date, dishID);
				Log.ActivityLog("Update to Dish Table: DishID = " + dishID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListDish", "Dish");
		}
	}
}
