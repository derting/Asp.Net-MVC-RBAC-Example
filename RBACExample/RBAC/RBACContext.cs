using System;
using System.Collections.Generic;
using System.Linq;

namespace RBACExample.RBAC
{
    public static class RBACContext
    {
        [ThreadStatic]
        private static RBACUser _User;

        private static Func<string, RBACUser> _SetRBACUser;
        private static List<RBACRole> _allRolePermissions = new List<RBACRole>();

        public static void AddRolePermission()
        {
            var role = _allRolePermissions.First(y => y.RoleName == "Role1");
            

            var _role1 = new List<RBACPermission>();
            _role1.Add(new RBACPermission() { PermissionName = "admin" });
            _role1.Add(new RBACPermission() { PermissionName = "general" });
            role.Permissions = _role1;
        }

        public static void RolePermissionInitial()
        {
            var myRoles = new List<RBACRole>();

            var role1 = new List<RBACPermission>();
            role1.Add(new RBACPermission() { PermissionName = "admin" });
            myRoles.Add(new RBACRole
            {
                RoleName = "Role1",
                Permissions = role1
            });

            myRoles.Add(new RBACRole
            {
                RoleName = "Role2",
                Permissions = new List<RBACPermission>
                {
                    new RBACPermission {PermissionName="general"}
                }
            });

            _allRolePermissions = myRoles;
        }

        public static void SetRBACUser(Func<string, RBACUser> setRBACUser)
        {
            _SetRBACUser = setRBACUser;
        }

        public static RBACUser GetRBACUser(string roleName)
        {

            RolePermissionInitial();
            return _User = new RBACUser()
            {
                UserName = roleName,
                Roles = _allRolePermissions.Where(y => y.RoleName == roleName).ToList()
            };


            //if (_User == null)
            //{
            //    RolePermissionInitial();
            //    return _User = new RBACUser()
            //    {
            //        UserName = roleName,
            //        Roles = _allRolePermissions.Where(y => y.RoleName == roleName).ToList()
            //    };
            //} else
            //{
            //    return _User;
            //}
        }

        public static void Clear()
        {
            _SetRBACUser = null;
        }
    }
}