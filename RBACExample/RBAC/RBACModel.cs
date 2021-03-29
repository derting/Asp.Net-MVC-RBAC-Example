using System.Collections.Generic;

namespace RBACExample.RBAC
{
    public class RBACUser
    {
        public string UserName { get; set; }

        public ICollection<RBACRole> Roles { get; set; } = new List<RBACRole>();
    }

    public class RBACRole
    {
        public string RoleName { get; set; }

        public ICollection<RBACPermission> Permissions { get; set; } = new List<RBACPermission>();
    }

    public class RBACPermission
    {
        public string PermissionName { get; set; }
    }
}