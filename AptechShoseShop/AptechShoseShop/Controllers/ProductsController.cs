using AptechShoseShop.Models.Admin;
using AptechShoseShop.Models.Client;
using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class ProductsController : Controller
    {
         AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
           var products = db.Products.Where(x => x.StatusId == 1).OrderByDescending(x => x.Id);


            var endProducts = products.Select(s => new ProductClientVM
            {
                ///Lay ra cai anh thu 2
                ///var listImage = db.ProductImages.Where(x => x.ProductId == s.Id)

                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });

            return View(endProducts.ToList());
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            ///int id = 13;
            var product = db.Products.Find(id);

            //if (product == null)
            //{
            //    return View();
            //}

            var LisUrl = db.ProductImages.Where(x => x.ProductId == product.Id).ToList();

            List<string> MainListUrl = new List<string>();

            for (int i = 0; i < LisUrl.Count; i++)
            {
                var eachUrl = "/Data/Products/" + product.Id + "/" + LisUrl[i].ImageUrl;
                MainListUrl.Add(eachUrl);
            }

            ViewBag.MainListUrl = MainListUrl;
            ViewBag.NewPrice = product.UnitPrice - (product.UnitPrice * product.DiscountRatio) / 100;

            var ListProCate = db.Products.Where(y => y.CategoryId == product.CategoryId).ToList();
            var MainListProCate = ListProCate.Select(s => new ProductClientVM
            {
                ///Lay ra cai anh thu 2
                ///var listImage = db.ProductImages.Where(x => x.ProductId == s.Id)

                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });

            ViewBag.MainListProCate = MainListProCate.ToList();

            return View(product);
        }
    }
}