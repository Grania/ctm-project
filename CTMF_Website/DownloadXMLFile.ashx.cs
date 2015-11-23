using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTMF_Website
{
	/// <summary>
	/// Summary description for DownloadXMLFile
	/// </summary>
	public class DownloadXMLFile : IHttpHandler
	{
		private const string _authString = "4d740f80a99f32e2eead4727282f22ba";

		public void ProcessRequest(HttpContext context)
		{
			//context.Response.ContentType = "text/plain";
			//context.Response.Write("Hello World");

			System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
			System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;

			string authString = request.QueryString["Auth"];
			if (authString == null || authString != _authString)
			{
				response.Write("Auth string error !");
				return;
			}

			string fileName = request.QueryString["FileName"];
			if (fileName == null || fileName == string.Empty)
			{
				response.Write("File name is require.");
				return;
			}

			response.ClearContent();
			response.Clear();
			response.ContentType = "text/plain";
			response.AddHeader("Content-Disposition"
				,"attachment; filename=" + fileName + ";");
			response.TransmitFile(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\xml\\" + fileName);
			response.Flush();
			response.End();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}