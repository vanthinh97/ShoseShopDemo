using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }
    }
}