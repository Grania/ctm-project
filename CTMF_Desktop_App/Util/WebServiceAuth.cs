using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using CTMF_Desktop_App.ServiceReference;

namespace CTMF_Desktop_App.Util
{
	public static class WebServiceAuth
	{
		private const string authString = "cc66ea17f303543d8c46207a7eaac530";

		public static AuthSoapHeader AuthSoapHeader(){
			AuthSoapHeader authSoapHd = new AuthSoapHeader();
			authSoapHd.authString = authString;
			return authSoapHd;
		}
	}
}