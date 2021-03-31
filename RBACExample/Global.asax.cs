using Newtonsoft.Json;
using RBACExample.App_Start;
using RBACExample.Models;
using RBACExample.RBAC;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace RBACExample
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            // 
            // Cookie -> FormsAuthenticationTicket -> userData -> HttpContext.Current.User
            //

            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie != null)
            {
                try
                {
                    UserInfo userData = null;
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                    {
                        userData = JsonConvert.DeserializeObject<UserInfo>(ticket.UserData);
                    }

                    if (ticket != null && userData != null)
                    {
                        RBACPrincipal principal = new RBACPrincipal(ticket.Name);
                        principal.Name = userData.Name;
                        principal.Role = userData.Role;
                        principal.LoginTime = userData.LoginTime;

                        HttpContext.Current.User = principal;
                    }
                }
                catch { /* 不處理異常。 */ }
            }
        }
    }
}