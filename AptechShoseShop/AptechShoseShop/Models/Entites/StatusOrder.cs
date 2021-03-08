using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class StatusOrder
    {
        public StatusOrder()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}