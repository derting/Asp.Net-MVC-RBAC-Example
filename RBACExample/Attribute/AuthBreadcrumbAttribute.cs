using NetCasbin;
using RBACExample.RBAC;
using System.Linq;
using System.Web.Mvc;

namespace RBACExample.Attribute
{
    public class AuthBreadcrumbAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public bool IsController { get; set; }
        public AuthBreadcrumbAttribute(string name, bool IsController = false)
        {
            this.Name = name;
            this.IsController = IsController;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool result = false;
            string controller = filterContext.Controller.GetType().Name.ToLower();
            string action = filterContext.ActionDescriptor.ActionName.ToLower();

            var rd = filterContext.RequestContext.HttpContext.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action").ToLower();
            string currentController = rd.GetRequiredString("controller").ToLower();

            var User = filterContext.RequestContext.HttpContext.User;

            if (IsController)
            {
                result = true;
            }
            else if (User.Identity.IsAuthenticated)
            {
                var userInfo = (User as RBACPrincipal);
                if (userInfo != null)
                {
                    userInfo.Role = userInfo.Role.ToLower();

                    var model = CasbinHelper.GetRBACModel();
                    var e = new Enforcer(model);
                    e.BuildRoleLinks();

                    //查詢指定角色在指定Controller下的Permission
                    var allPermissions = e.GetRolesForUserInDomain(userInfo.Role, currentController);

                    //確認Permission是否可以讀該Controller's Action
                    if (allPermissions.Any(permission => e.Enforce(permission, currentController, currentAction, "read") == true))
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = false;
            }

            if (!result)
            {
                filterContext.Result = new RedirectResult("/Home/PermissionDeny");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}