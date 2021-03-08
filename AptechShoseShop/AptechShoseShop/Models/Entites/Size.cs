using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Size
    {
        public Size()
        {
            ProductSizes = new HashSet<ProductSize>();
        }

        public int Id { get; set; }
        public string SizeName { get; set; }

        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}