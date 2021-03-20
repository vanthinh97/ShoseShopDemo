using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class Product
    {
        public Product()
        {
            ProductImages = new HashSet<ProductImage>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductColors = new HashSet<ProductColor>();
            ProductSizes = new HashSet<ProductSize>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountRatio { get; set; }
        public DateTime DiscountExpiry { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        /// <summary>
        /// Id ảnh đại diện
        /// </summary>
        public int? ProductImageId { get; set; }
        
        public virtual ProductImage ProductImage { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual TbUser TbUser { get; set; }
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual StatusProduct StatusProduct { get; set; }

        [ForeignKey("ProductId")]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}