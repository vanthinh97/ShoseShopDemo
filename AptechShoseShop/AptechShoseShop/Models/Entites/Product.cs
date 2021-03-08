using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductColors = new HashSet<ProductColor>();
            ProductImages = new HashSet<ProductImage>();
            ProductSizes = new HashSet<ProductSize>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountRatio { get; set; }
        public DateTime DiscountExpiry { get; set; }
        public int CategoryId { get; set; }
        public int? ProductImageId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int StatusId { get; set; }

        public virtual Category Category { get; set; }
        public virtual TbUser CreatedByNavigation { get; set; }
        public virtual ProductImage ProductImage { get; set; }
        public virtual StatusProduct Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}