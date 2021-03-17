using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AptechShoseShop.Models.Authorization
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

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

        public override string[] GetRolesForUser(string username)
        {
            //edit
            using (var db = new AptechShoseShopDbContext())
            {
                int userId = int.Parse(username);
                var uu = db.TbUsers.Find(userId);
                var rs = uu.UserRoles.Select(y => y.Role.RoleName).ToArray();
                return rs;
            }

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            //edit

            using (var db = new AptechShoseShopDbContext())
            {
                int userId = int.Parse(username);
                var rs = db.UserRoles.AsEnumerable().Where(x => x.UserId == userId)
                    .Select(x => x.Role.RoleName).Contains(roleName);
                return rs;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}