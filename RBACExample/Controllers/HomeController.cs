using Newtonsoft.Json;
using RBACExample.Attribute;
using RBACExample.Models;
using RBACExample.RBAC;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RBACExample.Controllers
{
    [AuthBreadcrumb("預設目錄", IsController: true)]
    public class HomeController : Controller
    {
        public ActionResult Login(string returnUrl, string role, string userName)
        {
            SetFormsAuthCookie(userName, role);
            return Redirect(returnUrl);
        }

        private void SetFormsAuthCookie(string userName, string role)
        {
            //序列化UserInfo
            UserInfo user = new UserInfo()
            {
                Role = role,
                Name = userName,
                LoginTime = DateTime.Now
            };
            var userInfo = JsonConvert.SerializeObject(user);

            //Ticket
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: userName, //可以放使用者Id
                issueDate: DateTime.UtcNow,//現在UTC時間
                expiration: DateTime.UtcNow.AddDays(30),//Cookie有效時間=現在時間往後+30天
                isPersistent: true,// 是否要記住我 true or false
                userData: userInfo, //可以放使用者角色名稱
                cookiePath: FormsAuthentication.FormsCookiePath);

            //Cookie
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;

            // 5. 寫登錄Cookie
            Response.Cookies.Remove(cookie.Name);
            Response.Cookies.Add(cookie);
        }

        public ActionResult Logoff()
        {
            // FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Index()
        {
            var tmp = (User as RBACPrincipal);
            //var model = CasbinHelper.GetRBACModel();
            //var e = new Enforcer(model);
            //e.BuildRoleLinks();

            //var tmp1 = e.GetUsersForRole("role3");
            //var tmp2 = e.GetRolesForUser("general");
            //var tmp3 = e.GetPermissionsForUser("admin");

            ////owner在manage的權限中，有Role3這個角色
            //var tmp6 = e.HasRoleForUser("role3", "owner", "manage");        //true

            ////admin在Home有那些限制
            //var tmp5 = e.GetPermissionsForUserInDomain("admin", "home");    //3 row -> {home, account} {home, general} {home, admin}

            ////角色Role3在Home下有甚麼權限
            //var tmp4 = e.GetRolesForUserInDomain("role3", "home");          //2 row -> [general, owner]

            ////admin權限可以讀取Home/Account
            //bool result = e.Enforce("admin", "Home", "Account", "read");

            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }


        [AuthBreadcrumb("帳戶資訊")]
        public ActionResult Account()
        {
            //RBAC.RBACContext.AddRolePermission();
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