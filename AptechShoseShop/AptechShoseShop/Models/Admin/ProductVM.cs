using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Admin
{
    public class ProductVM
    {
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
    }
}