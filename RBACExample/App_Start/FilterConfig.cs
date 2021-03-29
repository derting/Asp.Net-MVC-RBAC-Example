using RBACExample.Attribute;
using System.Web.Mvc;

namespace RBACExample.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            MvcAuthorizeAttribute.ActionRoleMappingInitial();
            filters.Add(new MvcAuthorizeAttribute());
        }
    }
}