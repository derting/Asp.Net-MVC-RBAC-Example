using RBACExample.App_Start;
using RBACExample.RBAC;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace RBACExample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DelegateRoleProvider.SetGetRolesForUser(
                userName => 
                    RBACContext.GetRBACUser(userName).Roles.SelectMany(o => o.Permissions).Select(p => p.PermissionName).ToArray());

            DelegateRoleProvider.SetIsUserInRole(
                (userName, roleName) => 
                    RBACContext.GetRBACUser(userName).Roles.SelectMany(o => o.Permissions).Any(p => p.PermissionName == roleName));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}