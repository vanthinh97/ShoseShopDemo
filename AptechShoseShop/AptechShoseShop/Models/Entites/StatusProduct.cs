using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class StatusProduct
    {
        public StatusProduct()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}