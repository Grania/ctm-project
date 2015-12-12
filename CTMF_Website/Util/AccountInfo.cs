using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CTMF_Website.Util
{
	public class AccountInfo
	{
		private static List<KeyValuePair<int, string>> RoleListEnglish = new List<KeyValuePair<int, string>>
		{
			new KeyValuePair<int, string>(1, "Customer"),
			new KeyValuePair<int, string>(2, "Cafeteria Staff"),
			new KeyValuePair<int, string>(3, "Manager"),
			new KeyValuePair<int, string>(4, "Administrator"),
		};

		private static List<KeyValuePair<int, string>> RoleListVnese = new List<KeyValuePair<int, string>>
		{
			new KeyValuePair<int, string>(1, "Khách hàng"),
			new KeyValuePair<int, string>(2, "Nhân viên nhà ăn"),
			new KeyValuePair<int, string>(3, "Quản lý"),
			new KeyValuePair<int, string>(4, "Quản trị viên"),
		};

		public static List<KeyValuePair<int, string>> GetRoleListEnglish()
		{
			return RoleListEnglish;
		}

		public static List<KeyValuePair<int, string>> GetRoleListVnese()
		{
			return RoleListVnese;
		}

		public static string GetRoleNameEnglish(int role)
		{
			if (role == 1)
			{
				return "Customer";
			}
			else if (role == 2)
			{
				return "Cafeteria staff";
			}
			else if (role == 3)
			{
				return "Manager";
			}
			else if (role == 4)
			{
				return "Administrator";
			}
			else
			{
				return null;
			}
		}

		public static string GetRoleNameVnese(int role)
		{
			if (role == 1)
			{
				return "Khách hàng";
			}
			else if (role == 2)
			{
				return "Nhân viên nhà ăn";
			}
			else if (role == 3)
			{
				return "Quản lý";
			}
			else if (role == 4)
			{
				return "Quản trị viên";
			}
			else
			{
				return null;
			}
		}

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
				Log.ErrorLog(cex.Message);
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
				Log.ErrorLog(cex.Message);
				return null;
			}
		}
	}
}