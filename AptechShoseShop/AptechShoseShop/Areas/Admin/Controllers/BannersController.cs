using AptechShoseShop.Models.Entites;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class BannersController : Controller
    {
        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Banners
        public ActionResult Index()
        {
            var listBanner = db.Banners.OrderBy(x => x.Position).ToList();
            return View(listBanner);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string Name, string Description, string LinkBanner,
                                int Position, HttpPostedFileBase UrlImageBanner)
        {
            var url = UrlImageBanner.FileName;
            Banner newBanner = new Banner(Name, Description,
                                        url, LinkBanner, Position);
            db.Banners.Add(newBanner);
            db.SaveChanges();
            string strFolder = Server.MapPath("~/Data/banners/" + newBanner.Id);

            if (!Directory.Exists(strFolder))
            {
                Directory.CreateDirectory(strFolder);
            }
            UrlImageBanner.SaveAs(strFolder + @"\" + UrlImageBanner.FileName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var Banner = db.Banners.Find(id);
            return View(Banner);
        }

        [HttpPost]
        public ActionResult Edit(int Id, string Name, string Description, string LinkBanner,
                                int Position, HttpPostedFileBase UrlImageBanner)
        {
            var Banner = db.Banners.Find(Id);
            Banner.Name = Name;
            Banner.Description = Description;
            Banner.LinkBanner = LinkBanner;
            Banner.Position = Position;
            if (UrlImageBanner != null)
            {
                string UrlName = UrlImageBanner.FileName;
                string strFolder = Server.MapPath("~/Data/banners/" + Banner.Id);

                if (System.IO.File.Exists(strFolder + @"\" + Banner.UrlImageBanner))
                {
                    System.IO.File.Delete(strFolder + @"\" + Banner.UrlImageBanner);
                }

                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                UrlImageBanner.SaveAs(strFolder + @"\" + UrlImageBanner.FileName);

                Banner.UrlImageBanner = UrlName;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteBanner(int id)
        {
            db.Banners.Remove(db.Banners.Find(id));
            db.SaveChanges();
            return Content("OK");
        }
    }
}