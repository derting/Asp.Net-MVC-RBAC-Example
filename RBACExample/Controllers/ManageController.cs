using NetCasbin;
using Newtonsoft.Json;
using RBACExample.Attribute;
using RBACExample.Common;
using RBACExample.Models;
using RBACExample.RBAC;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RBACExample.Controllers
{
    [AuthBreadcrumb("管理目錄")]
    public class ManageController : Controller
    {
        [AuthBreadcrumb("管理目錄首頁")]
        public ActionResult Index()
        { 
            var e = new Enforcer(CasbinHelper.GetRBACModel());
            var roles = e.GetAllRoles().Distinct().Select(y => y).ToList();

            var ctrlActions = BreadcurmbHelper.GetAll();
            List<ControllerActionPermission> permList = new List<ControllerActionPermission>();
            foreach (var ctrlGroup in ctrlActions.GroupBy(y=>y.Controller))
            {
                var ctrl = ctrlGroup.Key.ToLower();
                ctrl = ctrl.Replace("controller", "");
                foreach (var role in roles)
                {
                    foreach (var action in ctrlGroup)
                    {
                        bool isAllow = false;
                        if (e.Enforce(role, ctrl, action.Action.ToLower(), "read"))
                        {
                            isAllow = true;
                        }

                        var serializedParent = JsonConvert.SerializeObject(action);
                        var cap = JsonConvert.DeserializeObject<ControllerActionPermission>(serializedParent);

                        cap.IsAllow = isAllow;
                        cap.Permission = role;
                        permList.Add(cap);
                    }
                }
            }
            
            ViewBag.Roles = roles;
            ViewBag.Permisiions = permList;
            return View(ctrlActions);
        }
    }
}