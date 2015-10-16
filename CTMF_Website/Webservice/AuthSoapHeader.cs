using System.Web.Services.Protocols;

namespace CTMF_Website.Webservice
{
	public class AuthSoapHeader : SoapHeader
	{
		public string authString { get; set; }
	}
}