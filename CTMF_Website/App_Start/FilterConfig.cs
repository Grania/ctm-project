using System.Web.Mvc;
using System.Web.Routing;

namespace CTMF_Website
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new AuthorizeAttribute());
		}
	}

	//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
		{
			if (filterContext.HttpContext.Request.IsAuthenticated)
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
					{"controller", "Error"},
					{"action", "Unauthorization"}
				});
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
				|| filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
			if (!skipAuthorization)
			{
				base.OnAuthorization(filterContext);
			}
		}
	}
}