using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;

namespace AptechShoseShop.Models.Admin
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double? DiscountRatio { get; set; }
        public DateTime DiscountExpiry { get; set; }
        public int CategoryId { get; set; }
        public int? ProductImageId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int StatusId { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
        public double SalePrice { get; set; }
        public string ImgUrl { get; set; }
    }
}