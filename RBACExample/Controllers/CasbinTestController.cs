using NetCasbin;
using RBACExample.RBAC;
using System.Web.Mvc;

namespace RBACExample.Controllers
{
    public class CasbinTestController : Controller
    {
        // GET: CasbinTest
        public ActionResult Index()
        {
            var model = CasbinHelper.GetRBACModel();
            var e = new Enforcer(model);
            e.BuildRoleLinks();

            var tmp1 = e.GetUsersForRole("role3");
            var tmp2 = e.GetRolesForUser("general");
            var tmp3 = e.GetPermissionsForUser("admin");

            //owner在manage的權限中，有Role3這個角色
            var tmp6 = e.HasRoleForUser("role3", "owner", "manage");        //true

            //admin在Home有那些限制
            var tmp5 = e.GetPermissionsForUserInDomain("admin", "home");    //3 row -> {home, account} {home, general} {home, admin}

            //角色Role3在Home下有甚麼權限
            var tmp4 = e.GetRolesForUserInDomain("role3", "home");          //2 row -> [general, owner]

            //admin權限可以讀取Home/Account
            bool result = e.Enforce("admin", "Home", "Account", "read");

            return View();
        }
    }
}