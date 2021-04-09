using AptechShoseShop.Models.Client;
using AptechShoseShop.Models.Entites;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class HomeController : Controller
    {
        private AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.Name);
                TbUser u = db.TbUsers.Find(userId);
            };

            var policies = db.Policies.ToList();
            ViewBag.Policies = policies;

            ///San pham moi nhat
            var products = db.Products.Where(x => x.StatusId == 1).OrderByDescending(x => x.Id).Take(12);
            var lastedProducts = products.Select(s => new ProductClientVM
            {
                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });
            ViewBag.LastedProducts = lastedProducts.ToList();

            //San pham giam gia nhieu nhat
            var disProducts = db.Products.Where(x => x.DiscountRatio > 20).OrderByDescending(x => x.DiscountRatio).Take(10);
            var listDisProducts = disProducts.Select(s => new ProductClientVM
            {
                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });
            ViewBag.ListDisProducts = listDisProducts.ToList();

            //San pham ban chay nhat
            var bestProducts = db.Products.Where(x => x.StatusId == 1);
            var bestseller = db.OrderDetails.AsEnumerable()
                          .GroupBy(x => x.ProductId)
                          .Select(t => new { ID = t.Key, Value = t.Sum(s => s.Quantity) })
                          .OrderByDescending(x => x.Value).Select(s => s.ID);
            var BestProducts = (from a in bestseller
                                join b in bestProducts
                                on a equals b.Id
                                select b).Take(4);
            var listBestProducts = BestProducts.Select(s => new ProductClientVM
            {
                Id = s.Id,
                ProductName = s.ProductName,
                ImgUrl = s.ProductImageId != null ? "/Data/Products/" + s.Id + "/" + s.ProductImage.ImageUrl : "https://martialartsplusinc.com/wp-content/uploads/2017/04/default-image-620x600.jpg",
                UnitPrice = s.UnitPrice.ToString(),
                DiscountRatio = s.DiscountRatio.ToString(),
                salePrice = (s.UnitPrice - ((s.UnitPrice * s.DiscountRatio) / 100)).ToString(),
                CategoryName = s.Category.CategoryName
            });

            ViewBag.ListBestProducts = listBestProducts.ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}