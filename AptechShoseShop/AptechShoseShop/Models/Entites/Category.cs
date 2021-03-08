using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int? Position { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}