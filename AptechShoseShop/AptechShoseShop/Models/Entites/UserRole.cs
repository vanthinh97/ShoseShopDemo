using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual TbUser User { get; set; }
    }
}