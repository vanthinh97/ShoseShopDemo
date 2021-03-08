using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class StatusUser
    {
        public StatusUser()
        {
            TbUsers = new HashSet<TbUser>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<TbUser> TbUsers { get; set; }
    }
}