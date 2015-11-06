using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CTMF_Website.Util
{
	public class AccountInfo
	{
		public static string GetUserName(HttpRequestBase Request)
		{
			HttpCookie authCookie =
					  Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				FormsAuthenticationTicket authTicket =
					FormsAuthentication.Decrypt(authCookie.Value);

				return authTicket.Name;
				//string[] roles = authTicket.UserData.Split(new Char[] { ',' });
				//GenericPrincipal userPrincipal =
				//				 new GenericPrincipal(new GenericIdentity(authTicket.Name),
				//									  roles);
				//Context.User = userPrincipal;
			}

			return null;
		}

		public static string GetUserRole(HttpRequestBase Request)
		{
			HttpCookie authCookie =
					  Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				FormsAuthenticationTicket authTicket =
					FormsAuthentication.Decrypt(authCookie.Value);

				return authTicket.UserData;
				//string[] roles = authTicket.UserData.Split(new Char[] { ',' });
				//GenericPrincipal userPrincipal =
				//				 new GenericPrincipal(new GenericIdentity(authTicket.Name),
				//									  roles);
				//Context.User = userPrincipal;
			}

			return null;
		}
	}
}