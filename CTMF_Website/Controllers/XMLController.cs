using CTMF_Website.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CTMF_Website.Controllers
{
	public class XMLController : ApiController
	{
		private static string _path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "xml\\";

		[System.Web.Http.AcceptVerbs("POST")]
		public async void SendInventoryXML(string filename)
		{
			try
			{
				XDocument doc = XDocument.Load(await Request.Content.ReadAsStreamAsync());
				string saveLoc = _path + filename + ".xml";
				doc.Save(saveLoc);

				var response = Request.CreateResponse(HttpStatusCode.OK);
				response.Content = new StringContent("Successful upload", Encoding.UTF8, "text/plain");
				response.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(@"text/html");
			}
			catch (Exception ex)
			{
				Log.ErrorLog(ex.Message);
				throw ex;
			}
		}
	}
}
