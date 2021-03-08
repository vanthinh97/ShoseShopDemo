using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AptechShoseShop.Models.Entites
{
    public class AptechShoseShopDbContext : DbContext
    {
        public AptechShoseShopDbContext() : base("name=AptechShoseShop") { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductSize> ProductSizes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<StatusProduct> StatusProducts { get; set; }
        public virtual DbSet<StatusUser> StatusUsers { get; set; }
        public virtual DbSet<TbUser> TbUsers { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}