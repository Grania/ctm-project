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
			try
			{
				HttpCookie authCookie =
						  Request.Cookies[FormsAuthentication.FormsCookieName];
				if (authCookie != null)
				{
					FormsAuthenticationTicket authTicket =
						FormsAuthentication.Decrypt(authCookie.Value);

					return authTicket.Name;
				}

				return null;
			}
			catch (System.Security.Cryptography.CryptographicException cex)
			{
				FormsAuthentication.SignOut();
				return null;
			}
		}

		public static string GetUserRole(HttpRequestBase Request)
		{
			try
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
			catch (System.Security.Cryptography.CryptographicException cex)
			{
				FormsAuthentication.SignOut();
				return null;
			}
		}
	}
}