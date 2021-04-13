﻿using AptechShoseShop.Models.Entites;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();

        // GET: Admin/Categories
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Categories.OrderBy(x => x.Position).ToList());
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult CreateCate()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult CreateCate(Category cate)
        {
            //if (cate.Position >= 0)
            //{

            //};

            Category newCate = new Category()
            {
                CategoryName = cate.CategoryName,
                Position = cate.Position
            };
            db.Categories.Add(newCate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult _EditCate(int id)
        {
            var cate = db.Categories.Find(id);
            return PartialView(cate);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult EditCate(int id, string categoryName, int position)
        {
            var cate = db.Categories.Find(id);
            cate.CategoryName = categoryName;
            cate.Position = position;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCate(int id)
        {
            Category t = db.Categories.Find(id);
            db.Categories.Remove(t);
            db.SaveChanges();
            return Content("OK");
        }
    }
}
