using System;
using System.Security.Principal;
using System.Web.Security;

namespace RBACExample.RBAC
{
    interface IRBACPrincipal : IPrincipal
    {
        string Role { get; set; }
        string Name { get; set; }
        DateTime LoginTime { get; set; }
    }

    public class RBACPrincipal : IRBACPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public RBACPrincipal(string userName)
        {
            this.Identity = new GenericIdentity(userName);
        }

        public string Role { get; set; }
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }
    }

    //public class FormsPrincipal<TUserData> : IRBACPrincipal where TUserData : class, new()
    //{
    //    private IIdentity _identity;
    //    private TUserData _userData;

    //    public FormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)
    //    {
    //        if (ticket == null)
    //            throw new ArgumentNullException("ticket");
    //        if (userData == null)
    //            throw new ArgumentNullException("userData");

    //        _identity = new FormsIdentity(ticket);
    //        _userData = userData;
    //    }

    //    public TUserData UserData
    //    {
    //        get { return _userData; }
    //    }

    //    public IIdentity Identity
    //    {
    //        get { return _identity; }
    //    }

    //    public string Role { get; set; }
    //    public string Name { get; set; }
    //    public DateTime LoginTime { get; set; }

    //    public bool IsInRole(string role)
    //    {
    //        return false;
    //    }
    //}
}