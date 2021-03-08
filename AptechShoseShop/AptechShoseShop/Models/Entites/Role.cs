using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}