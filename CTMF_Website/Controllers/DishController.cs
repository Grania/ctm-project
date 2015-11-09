using CTMF_Website.DataAccessTableAdapters;
using CTMF_Website.Util;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CTMF_Website.Controllers
{
	public class DishController : Controller
	{
		private string _dishImagePath = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\";
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
			return View();
		}

		[AllowAnonymous]
		public ActionResult EditDish(string dishID)
		{
			return View();
		}

		[AllowAnonymous]
		public async Task<JsonResult> UploadImage(string id)
		{
			try
			{
				foreach (string file in Request.Files)
				{
					var fileContent = Request.Files[file];
					if (fileContent != null && fileContent.ContentLength > 0)
					{
						// get a stream
						var stream = fileContent.InputStream;
						// and optionally write the file to disk
						var fileName = Path.GetFileName(file);
						var path = Path.Combine(_dishImagePath, fileName);

						using (var fileStream = System.IO.File.Create(path))
						{
							stream.CopyTo(fileStream);
						}
					}
				}
			}
			catch (Exception)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json("Upload failed");
			}

			return Json("File uploaded successfully");
		}
	}
}
