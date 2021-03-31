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

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Index()
        {
            //
            // Get User Information from Principal
            var tmp = (User as RBACPrincipal);
            
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


        public ActionResult PermissionDeny()
        {
            return Content("Permission deny.");
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
    }
}