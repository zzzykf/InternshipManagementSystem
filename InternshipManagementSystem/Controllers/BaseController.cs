using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InternshipManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            if(authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                ViewBag.Username = ticket.Name;
            }
            if (User.IsInRole("Faculty Coordinator"))
            {
                ViewBag.Role = "Faculty Coordinator";
            }
            else if (User.IsInRole("Student"))
            {
                ViewBag.Role = "Student";
            }
            else if (User.IsInRole("Industrial Supervisor"))
            {
                ViewBag.Role = "Industrial Supervisor";
            }
            else if (User.IsInRole("Faculty Supervisor"))
            {
                ViewBag.Role = "Faculty Supervisor";
            }
            else
            {
                ViewBag.Role = "Hello";
            }
            base.OnActionExecuting(filterContext);
        }
    }

}