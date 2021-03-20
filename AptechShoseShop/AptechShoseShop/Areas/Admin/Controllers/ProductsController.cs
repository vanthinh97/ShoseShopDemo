using AptechShoseShop.Models.Admin;
using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Products
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.Id).ToList();
            return View(product);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var showCate = db.Categories.OrderBy(x => x.Position);

            List<StatusProduct> statusPro = db.StatusProducts.ToList();
            SelectList statusProList = new SelectList(statusPro, "Id", "StatusName");
            ViewBag.StatusPro = statusProList;

            string date = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.date = date;

            return View(showCate);
        }

        [HttpPost]
        public ActionResult Create(ProductVM product, HttpPostedFileBase[] ProductImageId)
        {
            Product newPro = new Product()
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                DiscountRatio = product.DiscountRatio,
                DiscountExpiry = product.DiscountExpiry,
                CategoryId = product.CategoryId,
                Description = product.Description,
                CreatedDate = product.CreatedDate,
                CreatedBy = product.CreatedBy,
                StatusId = product.StatusId
            };
            db.Products.Add(newPro);
            db.SaveChanges();


            ///Lưu vào bảng ProIamge
            ProductImage proImage = new ProductImage();

            foreach (var item in ProductImageId)
            {
                proImage.ImageUrl = item.FileName;
                proImage.ProductId = newPro.Id;
                db.ProductImages.Add(proImage);
                db.SaveChanges();
            }

            ///Lấy id của url đầu tiên của Pro
            string findUrl = ProductImageId[0].FileName;
            var defaultImage = db.ProductImages.Where(x => x.ImageUrl == findUrl).SingleOrDefault();
            newPro.ProductImageId = defaultImage.Id;
            db.SaveChanges();

            return RedirectToAction("Create");
        }
    }
}