using RBACExample.Attribute;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;

namespace RBACExample.Controllers
{
    [AuthBreadcrumb("預設目錄")]
    public class HomeController : Controller
    {
        public ActionResult Login(string returnUrl, string role)
        {
            FormsAuthentication.SetAuthCookie(role, false);
            return Redirect(returnUrl);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Index()
        {
            return View();
        }

        [AuthBreadcrumb("帳戶資訊")]
        public ActionResult Account()
        {
            RBAC.RBACContext.AddRolePermission();
            return Content(string.Format("user is IsAuthenticated:{0}", User.Identity.IsAuthenticated));
        }

        [AuthBreadcrumb("一般功能")]
        public ActionResult General()
        {
            return Content(string.Format("user is in role general:{0}", User.IsInRole("general")));
        }

        [AuthBreadcrumb("管理功能")]
        public ActionResult Admin()
        {
            return Content(string.Format("user is in role admin:{0}", User.IsInRole("admin")));
        }
    }
}