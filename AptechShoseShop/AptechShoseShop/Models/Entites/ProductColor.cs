using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }

        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
    }
}