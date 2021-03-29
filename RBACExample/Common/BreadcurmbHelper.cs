using RBACExample.Attribute;
using RBACExample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RBACExample.Common
{
    public static class BreadcurmbHelper
    {
        public static List<ControllerAction> GetAll()
        {
            List<ControllerAction> actions = new List<ControllerAction>();

            var asm = Assembly.GetExecutingAssembly();

            var controllers = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type));
            foreach (var ctrl in controllers)
            {
                var ctrlBreadcrumb = ctrl.GetCustomAttribute(typeof(AuthBreadcrumbAttribute)) as AuthBreadcrumbAttribute;
                if (ctrlBreadcrumb != null)
                {
                    //Controller have breadcrumb

                    var methods = ctrl.GetMethods().Where(method =>
                        method.IsPublic
                        && !method.IsDefined(typeof(NonActionAttribute))
                        && (
                            method.ReturnType == typeof(ActionResult) ||
                            method.ReturnType == typeof(Task<ActionResult>)));

                    foreach (var method in methods)
                    {
                        var actionBreadcrumb = method.GetCustomAttribute(typeof(AuthBreadcrumbAttribute)) as AuthBreadcrumbAttribute;
                        if (actionBreadcrumb != null)
                        {
                            //Action have breadcrumb

                            actions.Add(new ControllerAction()
                            {
                                Controller = ctrl.Name,
                                ControllerLabel = ctrlBreadcrumb.Name,
                                Action = method.Name,
                                ActionLabel = actionBreadcrumb.Name
                            });
                        }
                    }
                }
            }

            return actions;
        }
    }
}