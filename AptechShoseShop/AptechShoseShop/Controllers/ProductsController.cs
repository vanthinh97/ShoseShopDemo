using AptechShoseShop.Models.Client;
using AptechShoseShop.Models.Entites;
using PagedList;
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
        public ActionResult Index(string kw, int? cateid, string sortby, int? page)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1;

            ///var products = db.Products.Where(x => x.StatusId == 1).OrderByDescending(x => x.Id).Take(12);

            var showCate = db.Categories.ToList();
            ViewBag.ShowCate = showCate;

            ViewBag.Kw = kw;
            ViewBag.Cateid = cateid;
            ViewBag.SortBy = sortby;

            IEnumerable<Product> products = db.Products.Where(x => x.StatusId == 1);
            products = !string.IsNullOrEmpty(kw) ? getKw(kw, products) : products;
            products = cateid != null ? getCate(cateid, products) : products;
            products = !string.IsNullOrEmpty(sortby) ? getSortby(sortby, products) : products;

            ///Lay ra cai anh thu 2
            ///var listImage = db.ProductImages.Where(x => x.ProductId == s.Id)
            var endProducts = products.Select(s => new ProductClientVM
            {
                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });

            if (sortby == "name_defau" || sortby == null)
            {
                Random rnd = new Random();

                List<ProductClientVM> RdProduct = endProducts.ToList();
                RdProduct = RdProduct.OrderBy(x => rnd.Next()).ToList();

                return View(RdProduct.ToPagedList(pageNumber, pageSize));
            }

            return View(endProducts.ToPagedList(pageNumber, pageSize));
        }

        public IEnumerable<Product> getCate(int? cateid, IEnumerable<Product> p)
        {
            p = p.Where(x => x.CategoryId == cateid);
            return p;
        }
        public IEnumerable<Product> getKw(string kw, IEnumerable<Product> p)
        {
            kw = kw.ToLower();
            ViewBag.Kw = kw;
            p = p.Where(x => x.ProductName.ToLower().Contains(kw));
            return p;
        }
        public IEnumerable<Product> getSortby(string sortby, IEnumerable<Product> p)
        {
            switch (sortby)
            {
                case "name":
                    p = p.OrderBy(s => s.ProductName);
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.ProductName);
                    break;
                case "price":
                    p = p.OrderBy(x => x.UnitPrice);
                    break;
                case "price_desc":
                    p = p.OrderByDescending(x => x.UnitPrice);
                    break;
                //case "best_sell":
                //    string query = "select PD.*" +
                //        "from Products as PD left join OrderDetails as OD" +
                //        "on OD.ProductId = PD.Id" +
                //        "group by PD.Id, PD.ProductName, PD.UnitPrice, PD.DiscountRatio, PD.DiscountExpiry, PD.IsActive, PD.CategoryId, PD.ImgUrl, PD.Description" +
                //        "order by COUNT(OD.ProductId) desc";
                //    p = db.Database.SqlQuery<Product>(query);
                //    break;
                default:
                    p = p.OrderByDescending(x => x.Id);
                    break;
            }
            return p;
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