using System;
using System.Web.Security;

namespace RBACExample.RBAC
{
    public class DelegateRoleProvider : RoleProvider
    {
        private static Func<string, string[]> _GetRolesForUser;

        private static Func<string, string, bool> _IsUserInRole;

        public static void SetGetRolesForUser(Func<string, string[]> getRolesForUser)
        {
            _GetRolesForUser = getRolesForUser;
        }

        public static void SetIsUserInRole(Func<string, string, bool> isUserInRole)
        {
            _IsUserInRole = isUserInRole;
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] result = _GetRolesForUser(username);
            
            return result;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var result = _IsUserInRole(username, roleName);

            return result;
        }

        #region NotImplemented

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        //public override string[] GetRolesForUser(string username)
        //{
        //    throw new NotImplementedException();
        //}

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        //public override bool IsUserInRole(string username, string roleName)
        //{
        //    throw new NotImplementedException();
        //}

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion NotImplemented
    }
}