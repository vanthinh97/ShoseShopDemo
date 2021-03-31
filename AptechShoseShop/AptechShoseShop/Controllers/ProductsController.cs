﻿using AptechShoseShop.Models.Client;
using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ///var products = db.Products.Where(x => x.StatusId == 1).OrderByDescending(x => x.Id).Take(12);
            var products = db.Products.Where(x => x.StatusId == 1);


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

            Random rnd = new Random();

            List<ProductClientVM> RdProduct = endProducts.ToList();
            RdProduct = RdProduct.OrderBy(x => rnd.Next()).Take(12).ToList();

            return View(RdProduct);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);

            //if (product == null)
            //{
            //    return View();
            //}

            var ListUrl = db.ProductImages.Where(x => x.ProductId == product.Id).ToList();

            List<string> MainListUrl = new List<string>();

            for (int i = 0; i < ListUrl.Count; i++)
            {
                var eachUrl = "/Data/Products/" + product.Id + "/" + ListUrl[i].ImageUrl;
                MainListUrl.Add(eachUrl);
            }

            ViewBag.MainListUrl = MainListUrl;
            ViewBag.NewPrice = product.UnitPrice - (product.UnitPrice * product.DiscountRatio) / 100;

            var ListProCate = db.Products.Where(y => y.CategoryId == product.CategoryId)
                    .OrderByDescending(z => z.Id).Take(5).ToList();
            var MainListProCate = ListProCate.Select(s => new ProductClientVM
            {
                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });
            ViewBag.MainListProCate = MainListProCate.ToList();

            var listColor = db.ProductColors.Where(x => x.ProductId == id).ToList();
            ViewBag.MainListColor = listColor;

            var listSize = db.ProductSizes.Where(x => x.ProductId == id).ToList();
            ViewBag.MainListSize = listSize;

            return View(product);
        }
    }
}