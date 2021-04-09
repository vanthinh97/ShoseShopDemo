using AptechShoseShop.Models.Entites;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class DashBoardController : Controller
    {
        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/DashBoard
        public ActionResult Index()
        {
            var quanUser = db.TbUsers.Count();
            ViewBag.QuanUser = quanUser;

            var quanOrder = db.Orders.Count();
            ViewBag.QuanOrder = quanOrder;

            var quanPro = db.Products.Count();
            ViewBag.QuanPro = quanPro;

            return View();
        }
    }
}