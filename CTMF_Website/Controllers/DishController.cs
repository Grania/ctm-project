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
			ViewBag.notExistDish = "";

			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			ViewData["DishType"] = dishTypeDT;

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
				try
				{
					return View(result.CopyToDataTable());
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(search))
					{
						ViewBag.notExistDish = "Không tìm thấy kết quả nào";
					}
					else
					{
						ViewBag.notExistDish = "Không tìm thấy kết quả nào với từ khóa: " + search;
					}
					Log.ErrorLog(ex.Message);
				}
			}

			return View(dishDT);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult ListDish(string search, string filter)
		{
			ViewBag.notExistDish = "";

			DishTypeTableAdapter dishTypeAdapter = new DishTypeTableAdapter();
			DataTable dishTypeDT = dishTypeAdapter.GetData();

			List<SelectListItem> items = new List<SelectListItem>();

			items.Add(new SelectListItem { Text = "Tất Cả", Value = "" });

			foreach (DataRow row in dishTypeDT.Rows)
			{
				items.Add(new SelectListItem { Text = row["TypeName"].ToString(), Value = row["DishTypeID"].ToString() });
			}
			ViewData["DishType"] = items;

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

				try
				{
					return View(result.CopyToDataTable());
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(search))
					{
						ViewBag.notExistDish = "Không tìm thấy kết quả nào";
					}
					else
					{
						ViewBag.notExistDish = "Không tìm thấy kết quả nào với từ khóa: " + search;
					}
					Log.ErrorLog(ex.Message);
				}
			}

			return View(dishDT);
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult DetailDish(string dishID)
		{
			try
			{
				int id = int.Parse(dishID);

				DetailDishModel model = new DetailDishModel();
				DishInfoDetailTableAdapter dishAdapter = new DishInfoDetailTableAdapter();
				DataTable dishDT = dishAdapter.GetDataByDishID(id);
				model.DishID = id;
				model.Dishname = dishDT.Rows[0]["Name"].ToString();
				model.DishTypeName = dishDT.Rows[0]["TypeName"].ToString();
				model.Description = dishDT.Rows[0]["Description"].ToString();
				model.Image = dishDT.Rows[0]["Image"].ToString();

				return View(model);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "Error");
			}
		}

		[Authorize(Roles = ("Manager"))]
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

		[Authorize(Roles = ("Manager"))]
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
			string imgPath = null;
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
				savePath = _sourcePath + dishName.Replace(" ", "_") + ".jpg";
				var sourcePath = AppDomain.CurrentDomain.BaseDirectory + model.Image;

				System.IO.File.Move(sourcePath, savePath);
				imgPath = "\\Images\\DishImages\\" + dishName.Replace(" ", "_") + ".jpg";
			}

			try
			{

				dishAdapter.InsertDish(dishName, dishTypeID, description, imgPath, date, updateBy, date);
				Log.ActivityLog("Insert into Dish Table: DishName = " + dishName);
				Session["addDish"] = "Thêm món ăn thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["addDish"] = "Thêm món ăn thất bại!";
			}
			return RedirectToAction("AddDish", "Dish");
		}

		private const int AvatarScreenWidth = 500;
		private static readonly string _tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
		private static readonly string _sourcePath = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\DishImages\\";

		[Authorize(Roles = ("Manager"))]
		[HttpPost]
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
			SavedFilePath(files);
			var successData = new
			{
				success = true,
				fileName = "/Temp/" + files.FileName
			};

			return Json(successData);
		}

		private string SavedFilePath(HttpPostedFileBase file)
		{
			//var serverPath = HttpContext.Server.MapPath(TempFolder);
			if (Directory.Exists(_tempPath) == false)
			{
				Directory.CreateDirectory(_tempPath);
			}

			var fileName = Path.GetFileName(file.FileName);
			fileName = SaveTemporaryImageFileImage(file, fileName);

			CleanUpTempFolder(1);
			return _tempPath + fileName;
		}

		private static string SaveTemporaryImageFileImage(HttpPostedFileBase file, string fileName)
		{
			var img = new WebImage(file.InputStream);
			var ratio = img.Height / (double)img.Width;
			img.Resize(AvatarScreenWidth, (int)(AvatarScreenWidth * ratio));

			string fullFileName = _tempPath + fileName;
			if (System.IO.File.Exists(fullFileName))
			{
				System.IO.File.Delete(fullFileName);
			}

			img.Save(fullFileName);
			return fileName;
		}

		private void CleanUpTempFolder(int hoursOld)
		{
			try
			{
				var currentUtcNow = DateTime.UtcNow;
				var serverPath = _tempPath;
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

		[Authorize(Roles = ("Manager"))]
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

			try
			{
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
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				return RedirectToAction("Error", "Error");
			}
		}

		[Authorize(Roles = ("Manager"))]
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

			string savePath = "\\Images\\DishImages\\" + dishName.Replace(" ", "_") + ".jpg";

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

				savePath = "\\Images\\DishImages\\" + dishName.Replace(" ", "_") + ".jpg";

			}

			if (model.Image != null)
			{
				if (model.Image.Contains("Temp"))
				{
					string oldImage = dt.Rows[0]["Image"].ToString();
					var oldImagePath = AppDomain.CurrentDomain.BaseDirectory + oldImage;
					System.IO.File.Delete(oldImagePath);
				}

				System.IO.File.Move(AppDomain.CurrentDomain.BaseDirectory + model.Image, AppDomain.CurrentDomain.BaseDirectory + savePath);
			}
			else
			{
				savePath = null;
			}

			try
			{
				dishAdapter.UpdateDish(dishName, dishTypeID, description, savePath, updateBy, date, dishID);
				Log.ActivityLog("Update to Dish Table: DishID = " + dishID);
				Session["editDish"] = "Cập nhật thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["editDish"] = "Cập nhật thất bại!";
			}

			return RedirectToAction("EditDish", "Dish", new { @dishID = model.DishID});
		}

		[Authorize(Roles = ("Manager"))]
		public ActionResult DeleteDish(int dishID)
		{
			try
			{
				DishTableAdapter DishAdapter = new DishTableAdapter();
				DataTable DishData = DishAdapter.GetDataByDishID(dishID);
				int test = DishAdapter.Delete(dishID);
				if (!string.IsNullOrEmpty(DishData.Rows[0]["Image"].ToString()))
				{
					var deleteFilePath = AppDomain.CurrentDomain.BaseDirectory + DishData.Rows[0]["Image"].ToString();
					System.IO.File.Delete(deleteFilePath);
				}
				Session["deleteDish"] = "Xóa thành công!";
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				Session["deleteDish"] = "Xóa thất bại! Món ăn đang được sử dụng.";
			}

			return RedirectToAction("ListDish", "Dish");
		}
	}
}
