using QLHD.Utinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLHD.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (SessionStore.users == null)
            //{
            //    RedirectToAction("Login", "Account");
            //}

            //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (SessionStore.users == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login"}));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}