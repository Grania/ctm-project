using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Models;
using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class MealSetController : Controller
	{
		DataTable mealSetDT = new DataTable();

		[AllowAnonymous]
		public ActionResult ListMealSet(string search, string filter)
		{
			ViewBag.successMessage = "";

			ViewBag.notExistMealSet = "";

			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();
			try
			{
				mealSetDT = mealSetAdapter.GetData();
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			if (!(string.IsNullOrEmpty(search) && string.IsNullOrEmpty(filter)))
			{
				int type = -1;
				if (!string.IsNullOrEmpty(filter))
				{
					int.TryParse(filter, out type);
				}

				string name = search;
				if (name == null)
				{
					name = String.Empty;
				}

				bool type_ = false;
				if (type == 1)
				{
					type_ = true;
				}
				var result = from row in mealSetDT.AsEnumerable()
							 where (name == String.Empty ? true : StringExtensions.ContainsInsensitive(row.Field<string>("Name"), name))
							 && (type < 0 ? true : row.Field<bool>("CanEatMore") == type_)
							 select row;

				try
				{
					return View(result.CopyToDataTable());
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(search))
					{
						ViewBag.notExistMealSet = "Không tìm thấy kết quả nào";
					}
					else
					{
						ViewBag.notExistMealSet = "Không tìm thấy kết quả nào với từ khóa: " + search;
					}
					Log.ErrorLog(ex.Message);
				}
			}

			return View(mealSetDT);
		}

		[AllowAnonymous]
		public ActionResult DetailMealSet(string mealSetID)
		{
			int id = int.Parse(mealSetID);
			MealSetDishInfoTableAdapter mealSetDishInfoAdapter = new MealSetDishInfoTableAdapter();
			ViewData["listMealSetDish"] = mealSetDishInfoAdapter.GetDataByMealSetID(id);

			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();
			EditMealSetModel model = new EditMealSetModel();
			DataTable mealSetDT = mealSetAdapter.GetDataByMealSetID(id);

			model.MealSetID = id;
			model.MealSetName = mealSetDT.Rows[0]["Name"].ToString();
			model.Description = mealSetDT.Rows[0]["Description"].ToString();
			model.Image = mealSetDT.Rows[0]["Image"].ToString();
			model.CanEatMore = (bool)mealSetDT.Rows[0]["CanEatMore"];

			return View(model);
		}

		[AllowAnonymous]
		public ActionResult AddMealSet()
		{
			ViewBag.CheckRunTimes = "1";
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddMealSet(MealSetViewModel model)
		{
			ViewBag.CheckRunTimes = "2";

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string updateBy = AccountInfo.GetUserName(Request);
			DateTime date = DateTime.Now;
			string mealSetName = model.MealSetName;
			string description = model.Description;
			bool canEatMore = model.CanEatMore;
			string imgPath = null;
			string savePath = null;

			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();
			DataTable mealSetDT = mealSetAdapter.GetDataByName(mealSetName);

			foreach (DataRow row in mealSetDT.Rows)
			{
				if (StringExtensions.EqualsInsensitive(row["Name"].ToString(), mealSetName))
				{
					ModelState.AddModelError("", "Tên suất ăn đã tồn tại.");
					return View(model);
				}
			}

			if (!string.IsNullOrEmpty(model.Image))
			{
				savePath = _sourcePath + mealSetName.Replace(" ", "_") + ".jpg";
				var sourcePath = AppDomain.CurrentDomain.BaseDirectory + model.Image;

				//Log.ActivityLog("savePath : " + savePath + " sourcePath :" + sourcePath);

				System.IO.File.Move(sourcePath, savePath);
				imgPath = "\\Images\\MealSetImages\\" + mealSetName.Replace(" ", "_") + ".jpg";
			}

			try
			{
				
				string mealSetID = mealSetAdapter.InsertMealSetScalar(mealSetName, imgPath, description, canEatMore, date, updateBy, date).ToString();
				int id = int.Parse(mealSetID);
				XmlSync.SaveMealSetXml(id, mealSetName, canEatMore, date, updateBy, date, null);
				Log.ActivityLog("Insert into MealSet Table: MealSetName = " + mealSetName);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}
			return RedirectToAction("ListMealSet", "MealSet");
		}

		private const int AvatarScreenWidth = 500;

		//private const string TempFolder = "/Temp2";
		//private const string MapTempFolder = "~" + TempFolder;
		//private const string DishImagesPath = "\\Images\\MealSetImages";

		private static readonly string _tempPath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
		private static readonly string _sourcePath = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\MealSetImages\\";

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
			fileName = SaveTemporaryAvatarFileImage(file, fileName);

			CleanUpTempFolder(1);
			return _tempPath + fileName;
		}

		private static string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string fileName)
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

		[AllowAnonymous]
		public ActionResult EditMealSet(string mealSetID)
		{
			int id = int.Parse(mealSetID);
			EditMealSetModel model = new EditMealSetModel();
			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();
			DataTable mealSetDT = mealSetAdapter.GetDataByMealSetID(id);
			model.MealSetID = id;
			model.MealSetName = mealSetDT.Rows[0]["Name"].ToString();
			model.Description = mealSetDT.Rows[0]["Description"].ToString();
			model.Image = mealSetDT.Rows[0]["Image"].ToString();
			model.CanEatMore = (bool)mealSetDT.Rows[0]["CanEatMore"];
			return View(model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditMealSet(EditMealSetModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();

			string updateBy = AccountInfo.GetUserName(Request);
			DateTime date = DateTime.Now;
			int mealSetID = model.MealSetID;
			string mealSetName = model.MealSetName;
			string description = model.Description;
			bool canEatMore = model.CanEatMore;

			DataTable dt = mealSetAdapter.GetDataByMealSetID(mealSetID);

			string savePath = "\\Images\\MealSetImages\\" + mealSetName.Replace(" ", "_") + ".jpg";

			if (!StringExtensions.EqualsInsensitive(dt.Rows[0]["Name"].ToString(), mealSetName))
			{
				DataTable mealSetDT = mealSetAdapter.GetDataByName(mealSetName);

				foreach (DataRow row in mealSetDT.Rows)
				{
					if (StringExtensions.EqualsInsensitive(row["Name"].ToString(), mealSetName))
					{
						ModelState.AddModelError("", "Tên món ăn đã tồn tại.");
						return View(model);
					}
				}

				savePath = "\\Images\\MealSetImages\\" + mealSetName.Replace(" ", "_") + ".jpg";

			}
			if (model.Image != null)
			{
				if (model.Image.Contains("Temp2"))
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
				mealSetAdapter.UpdateMealSet(mealSetName, savePath, description, canEatMore, updateBy, date, mealSetID);
				XmlSync.SaveMealSetXml(mealSetID, mealSetName, canEatMore, date, updateBy, date, null);
				Log.ActivityLog("Update to MealSet Table: MealSetID = " + mealSetID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListMealSet", "MealSet");
		}

		[AllowAnonymous]
		public ActionResult AddMealSetDish(string mealSetID, string search, string filter)
		{
			Session["maxMealsetDish"] = "";
			Session["existMealsetDish"] = "";

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

			DataTable dishDT = new DataTable();
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
					dishDT = result.CopyToDataTable();
				}
				catch (Exception ex)
				{
					if (string.IsNullOrEmpty(search))
					{
						ViewBag.notExistMealSet = "Không tìm thấy kết quả nào";
					}
					else
					{
						ViewBag.notExistMealSet = "Không tìm thấy kết quả nào với từ khóa: " + search;
					}
					Log.ErrorLog(ex.Message);
				}
			}

			ViewData["listDish"] = dishDT;

			int id = int.Parse(mealSetID);
			MealSetDishInfoTableAdapter mealSetDishInfoAdapter = new MealSetDishInfoTableAdapter();
			ViewData["listMealSetDish"] = mealSetDishInfoAdapter.GetDataByMealSetID(id);

			MealSetTableAdapter mealSetAdapter = new MealSetTableAdapter();
			EditMealSetModel model = new EditMealSetModel();
			DataTable mealSetDT = mealSetAdapter.GetDataByMealSetID(id);

			model.MealSetID = id;
			model.MealSetName = mealSetDT.Rows[0]["Name"].ToString();
			model.Description = mealSetDT.Rows[0]["Description"].ToString();
			model.Image = mealSetDT.Rows[0]["Image"].ToString();
			model.CanEatMore = (bool)mealSetDT.Rows[0]["CanEatMore"];

			return View(model);
		}

		public PartialViewResult Add(string mealSetID, string dishID)
		{
			int dish = int.Parse(dishID);
			int mealset = int.Parse(mealSetID);
			MealSetDishModel model = new MealSetDishModel();

			Session["maxMealsetDish"] = "";
			Session["existMealsetDish"] = "";

			try
			{
				MealSetDishDetailTableAdapter mealSetDishAdapter = new MealSetDishDetailTableAdapter();
				DataTable mealSetDishDT = new DataTable();
				mealSetDishDT = mealSetDishAdapter.GetDataByMealSetID(mealset);
				int test = mealSetDishDT.Rows.Count;
				if (mealSetDishDT.Rows.Count >= 6)
				{
					Session["maxMealsetDish"] = "Số lượng món ăn trong suất ăn đã đầy. Bỏ món ăn bạn ko cần để có thể thêm món mới!";
					return PartialView("_MealSetDish", model);
				}
				mealSetDishDT = mealSetDishAdapter.GetDataByMealsetDish(mealset, dish);
				if (mealSetDishDT.Rows.Count != 0)
				{
					Session["existMealsetDish"] = "Món ăn đã được thêm trước đó. Bạn vui lòng chọn món ăn khác!";
					return PartialView("_MealSetDish", model);
				}
				mealSetDishAdapter.InsertMealSetDish(mealset,dish);
				Log.ActivityLog("Insert into MealSetDishDetail table: MealsetID = " + mealSetID + ", DishID = " + dishID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			
			MealSetDishInfoTableAdapter mealSetDishInfoAdapter = new MealSetDishInfoTableAdapter();
			DataTable mealSetDishInfoDT = mealSetDishInfoAdapter.GetDataByMealSetIDDishID(mealset, dish);
			model.DishID = dish;
			model.MealSetID = mealset;
			model.Dishname = mealSetDishInfoDT.Rows[0]["DishName"].ToString();
			model.DishTypeID = (int)mealSetDishInfoDT.Rows[0]["DishTypeID"];
			model.DishDescription = mealSetDishInfoDT.Rows[0]["DishDescription"].ToString();
			model.DishImage = mealSetDishInfoDT.Rows[0]["DishImage"].ToString();

			return PartialView("_MealSetDish", model);
		}

		public PartialViewResult Remove(string mealSetID, string dishID)
		{
			int dish = int.Parse(dishID);
			int mealset = int.Parse(mealSetID);

			try
			{
				MealSetDishDetailTableAdapter mealSetDishAdapter = new MealSetDishDetailTableAdapter();
				mealSetDishAdapter.DeleteMealSetDish(mealset, dish);
				Log.ActivityLog("Delete into MealSetDishDetail table: MealsetID = " + mealSetID + ", DishID = " + dishID);
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
			}

			MealSetDishModel model = new MealSetDishModel();
			return PartialView("_MealSetDish", model);
		}

		[HttpPost]
		public ActionResult DeletemealSet(int mealSetID)
		{
			MealSetTableAdapter MealSetAdapter = new MealSetTableAdapter();
			DataTable MealSetData = MealSetAdapter.GetDataByMealSetID(mealSetID);

			try
			{
				int test = MealSetAdapter.Delete(mealSetID);

				if (!string.IsNullOrEmpty(MealSetData.Rows[0]["Image"].ToString()))
				{
					var deleteFilePath = AppDomain.CurrentDomain.BaseDirectory + MealSetData.Rows[0]["Image"].ToString();
					System.IO.File.Delete(deleteFilePath);
				}

				ViewBag.successMessage = "Xóa thành công suất ăn: " + MealSetData.Rows[0]["Image"].ToString();

			}
			catch (Exception ex)
			{
				ViewBag.successMessage = "Suất ăn: " + MealSetData.Rows[0]["Image"].ToString() + "đang được sử dụng.Xin kiểm tra lại trước khi xóa!";
				Log.ErrorLog(ex.Message);
			}

			return RedirectToAction("ListMealSet","MealSet");
		}
	}
}
