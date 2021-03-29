using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RBACExample.Attribute
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        private static Dictionary<string, string> _ActionRoleMapping = new Dictionary<string, string>();

        public static void ActionRoleMappingInitial()
        {           
            _ActionRoleMapping.Clear();

            AddConfig("HomeAccount", new string[] { "admin", "general" });
            AddConfig("HomeGeneral", "general");
            AddConfig("HomeAdmin", "admin");
            AddConfig("ManageIndex", "admin");
        }

        public static void AddConfig(string controllerAction, params string[] roles)
        {
            var rolesString = string.Empty;
            roles.ToList().ForEach(r => rolesString += "," + r);
            rolesString = rolesString.TrimStart(',');
            _ActionRoleMapping.Add(controllerAction, rolesString);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var key = string.Format("{0}{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
            key = key.ToUpper();

            if (_ActionRoleMapping.Any(x => x.Key.ToUpper().Contains(key)))
            {
                var mapping = _ActionRoleMapping.FirstOrDefault(x => x.Key.ToUpper().Contains(key));
                
                this.Roles = mapping.Value;
                base.OnAuthorization(filterContext);
            }
        }
    }
}