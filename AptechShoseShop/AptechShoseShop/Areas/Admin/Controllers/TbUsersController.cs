using AptechShoseShop.Models.Entites;
using AptechShoseShop.Models.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class TbUsersController : Controller
    {
        private AptechShoseShopDbContext db = new AptechShoseShopDbContext();

        // GET: Admin/TbUsers
        [HttpGet]
        public ActionResult Index()
        {
            var tbUsers = db.TbUsers.Include(t => t.Status);
            return View(tbUsers.ToList());
        }

        // GET: Admin/TbUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TbUser tbUser = db.TbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View(tbUser);
        }

        // GET: Admin/TbUsers/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string FullName, int Gender, string Email, string Password, string NumberPhone, string Address)
        {
            bool? GenderV2 = null;

            if (Gender == 1)
            {
                GenderV2 = true;
            }
            else if (Gender == 0)
            {
                GenderV2 = false;
            }

            var emailUser = db.TbUsers.Where(x => x.Email == Email).SingleOrDefault();

            if (emailUser != null)
            {
                ModelState.AddModelError("", "Account already exists");
                return View();
            }

            TbUser newUser = new TbUser()
            {
                FullName = FullName,
                Gender = GenderV2,
                Email = Email,
                Password = MySecurity.EncryptPassword(Password),
                NumberPhone = NumberPhone,
                CreatedDate = DateTime.Now,
                Address = Address,
                StatusId = 1,
                CountLogin = 0
            };
            db.TbUsers.Add(newUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = db.TbUsers.Find(id);
            var status = db.StatusUsers.ToList();
            ViewBag.Status = status;

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int Id, string FullName, int Gender, string NumberPhone, string Address, int StatusId)
        {
            bool? GenderV2 = null;

            if (Gender == 1)
            {
                GenderV2 = true;
            }
            else if (Gender == 0)
            {
                GenderV2 = false;
            }

            var newUser = db.TbUsers.Find(Id);
            newUser.FullName = FullName;
            newUser.Gender = GenderV2;
            newUser.NumberPhone = NumberPhone;
            newUser.Address = Address;
            newUser.StatusId = StatusId;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var user = db.TbUsers.Find(id);
            var SameUser = int.Parse(User.Identity.Name);
            if (user.Id == SameUser)
            {
                return Content("You cannot delete yourself");
            }

            db.TbUsers.Remove(user);
            db.SaveChanges();
            return Content("OK");
        }
    }
}
