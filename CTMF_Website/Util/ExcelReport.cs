using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CTMF_Website.Util
{
	public class ExcelReport
	{
		public static void ExportToExcel(System.Data.DataSet data, string sheetName)
		{
			XLWorkbook wb = new XLWorkbook();

			wb.Worksheets.Add(data);

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			HttpContext.Current.Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", sheetName.Replace(" ", "_")));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				wb.SaveAs(memoryStream);
				memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
				memoryStream.Close();
			}

			HttpContext.Current.Response.End();
		}

		public static void ExportToExcel(System.Data.DataTable data, string sheetName)
		{
			XLWorkbook wb = new XLWorkbook();

			wb.Worksheets.Add(data);

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			HttpContext.Current.Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", sheetName.Replace(" ", "_")));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				wb.SaveAs(memoryStream);
				memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
				memoryStream.Close();
			}

			HttpContext.Current.Response.End();
		}
	}
}