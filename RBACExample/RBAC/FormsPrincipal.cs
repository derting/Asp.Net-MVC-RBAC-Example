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
}