using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class ProductImage
    {
        public ProductImage()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        
        public string ImageUrl { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}