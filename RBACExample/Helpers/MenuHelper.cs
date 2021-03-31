using NetCasbin;
using RBACExample.Common;
using RBACExample.Models;
using RBACExample.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RBACExample.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlString DynamicMenu(this HtmlHelper helper, string role = "Guest")
        {
            role = role.ToLower();
            List<ControllerAction> permissionList = GetRolePermissions(role);

            string ctrlContent = string.Empty;
            foreach (var permissGroup in permissionList.GroupBy(y => y.Controller))
            {
                var ctrl = permissGroup.Key;
                ctrl = ctrl.ToLower().Replace("controller", "");
                var lblCtrl = permissGroup.First().ControllerLabel;

                string actContent = string.Empty;
                foreach (var permiss in permissGroup)
                {
                    var act = permiss.Action;
                    var lblact = permiss.ActionLabel;
                    actContent += $"<a href=\"/{ctrl}/{act}\">{lblact}</a>";
                }

                ctrlContent += $"<li>" +
                                    $"<label>{lblCtrl}</label>" +
                                    $"<div>{actContent}</div>" +
                                $"</li>";
            }


            string result = $"<ul>{ctrlContent}</ul>";

            return new MvcHtmlString(result);
        }

        private static List<ControllerAction> GetRolePermissions(string role)
        {
            List<ControllerAction> permissionList = new List<ControllerAction>();

            var model = CasbinHelper.GetRBACModel();
            var e = new Enforcer(model);
            e.BuildRoleLinks();

            var ctrlActions = BreadcurmbHelper.GetAll();
            foreach (var ctrl in ctrlActions.GroupBy(y => y.Controller))
            {
                var controllerName = ctrl.Key;
                controllerName = controllerName.ToLower().Replace("controller", "");

                foreach (var permission in e.GetRolesForUserInDomain(role, controllerName))
                {
                    var policys = e.GetPermissionsForUserInDomain(permission, controllerName);

                    List<ControllerAction> ctrlActPermisses = ctrlActions
                        .Where(y => y.Controller == ctrl.Key && policys.Any(p => p[2].ToString() == y.Action.ToLower() && p[4] == "allow")).ToList();

                    permissionList.AddRange(ctrlActPermisses);
                }
            }

            return permissionList;
        }
    }
}