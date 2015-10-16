using CTMF_Website.DataAccessTableAdapters;
using System.Data;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace CTMF_Website.Webservice
{
	/// <summary>
	/// Summary description for WebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class WebService : System.Web.Services.WebService
	{
		public AuthSoapHeader soapHeader;

		public WebService()
		{
		}

		public WebService(string authString)
		{
			soapHeader.authString = authString;
		}

		public WebService(AuthSoapHeader soapHeader)
		{
			this.soapHeader = soapHeader;
		}

		private bool authHeader()
		{
			if (soapHeader == null)
			{
				return false;
			}
			if (soapHeader.authString == "cc66ea17f303543d8c46207a7eaac530")
			{
				return true;
			}
			return false;
		}

		[WebMethod, SoapHeader("soapHeader")]
		public DataTable GetAccount(string username)
		{
			if (!authHeader())
			{
				return null;
			}

			ACCOUNTTableAdapter adapter = new ACCOUNTTableAdapter();
			return adapter.GetDataBy(username);
		}
	}
}
