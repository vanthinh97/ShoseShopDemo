using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AptechShoseShop.Models.Entites;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.OrderBy(x => x.Position).ToList());
        }

        [HttpGet]
        public ActionResult CreateCate()
        {
            return View();
        }

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

        public ActionResult DeleteCate(int id)
        {
            Category t = db.Categories.Find(id);
            db.Categories.Remove(t);
            db.SaveChanges();
            return Content("OK");
        }
    }
}
