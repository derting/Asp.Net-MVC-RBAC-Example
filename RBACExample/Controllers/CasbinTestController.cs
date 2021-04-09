using NetCasbin;
using NetCasbin.Model;
using RBACExample.Attribute;
using RBACExample.RBAC;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace RBACExample.Controllers
{
    [AuthBreadcrumb("測試Casbin", IsController = true)]
    public class CasbinTestController : Controller
    {
        [AuthBreadcrumb("Debug Assert")]
        // GET: CasbinTest
        public ActionResult Index()
        {
            var model = CasbinHelper.GetRBACModel();
            var e = new Enforcer(model);
            e.BuildRoleLinks();

            //e.EnableAutoSave(true);

            var tmp1 = e.GetUsersForRole("role3");
            var tmp2 = e.GetRolesForUser("general", "home");
            var tmp3 = e.GetPermissionsForUser("admin");

            //owner在manage的權限中，有Role3這個角色
            var tmp6 = e.HasRoleForUser("role3", "owner", "manage");        //true
            var tmp7 = e.HasRoleForUser("role3", "general", "manage");        //true
            var tmp8 = e.HasRoleForUser("role3", "admin", "manage");        //false

            //admin在Home有那些限制
            var tmp5 = e.GetPermissionsForUserInDomain("admin", "home");    //3 row -> {home, account} {home, general} {home, admin}

            //角色Role3在Home下有甚麼權限
            var tmp4 = e.GetRolesForUserInDomain("role3", "home");          //2 row -> [general, owner]

            //admin權限可以讀取Home/Account
            bool result = e.Enforce("admin", "home", "account", "read");

            //bool resultA = e.Enforce("general", "manage", "index", "read");
            //e.AddPolicy("general", "manage", "index", "read", "allow");
            //bool resultB = e.Enforce("general", "manage", "index", "read");

            return View();
        }

        /// <summary>
        /// 移除指定的Policy
        /// URL: localhost:53832/CasbinTest/RemovePolicy?policy=p, admin, home, account, read, allow
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        [AuthBreadcrumb("API 移除 Policy")]
        public ActionResult RemovePolicy(string policy)
        {
            string config = System.IO.File.ReadAllText(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "rbac_model.conf"));
            var m = Model.CreateDefaultFromText(config);

            var a = new CasbinFileAdapter(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "rbac_policy.csv"));
            var e = new Enforcer(m, a);

            var t = new List<string>();
            t.Add(policy);

            a.RemoveAndSavePolicy(m, t);
            CasbinHelper.ForceLoadFile();
            return Content("Save complete...");
        }

        /// <summary>
        /// 增添指定Policy
        /// URL: http://localhost:53832/CasbinTest/AddPolicy?role=admin&ctrl=home&act=account&isAllow=true
        /// </summary>
        /// <param name="role"></param>
        /// <param name="ctrl"></param>
        /// <param name="act"></param>
        /// <param name="isAllow"></param>
        /// <returns></returns>
        [AuthBreadcrumb("API 增加 Policy")]
        public ActionResult AddPolicy(string role, string ctrl, string act, bool isAllow)
        {
            string config = System.IO.File.ReadAllText(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "rbac_model.conf"));
            var m = Model.CreateDefaultFromText(config);

            var a = new CasbinFileAdapter(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "rbac_policy.csv"));
            var e = new Enforcer(m, a);

            if (isAllow)
            {
                e.AddPolicy(role, ctrl, act, "read", "allow");
            }
            else
            {
                e.AddPolicy(role, ctrl, act, "read", "deny");
            }

            e.SavePolicy();
            CasbinHelper.ForceLoadFile();
            return Content("Save complete...");
        }

    }
}