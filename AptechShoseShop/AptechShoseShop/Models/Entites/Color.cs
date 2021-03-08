using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Color
    {
        public Color()
        {
            ProductColors = new HashSet<ProductColor>();
        }

        public int Id { get; set; }
        public string ColorName { get; set; }

        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}