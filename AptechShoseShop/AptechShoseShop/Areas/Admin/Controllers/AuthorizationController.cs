using AptechShoseShop.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class AuthorizationController : Controller
    {
        readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Authorization
        public ActionResult Index(string checkEmail)
        {
            if (checkEmail != null)
            {
                ViewBag.HasAuthor = "Quyền này đã được phân cho " + checkEmail;
            }
            var listAuthor = db.UserRoles;
            return View(listAuthor.ToList());
        }

        [HttpPost]
        public ActionResult AddRole(int RoleId, int UserId)
        {
            var check = db.UserRoles.Where(x => x.RoleId == RoleId && x.UserId == UserId).FirstOrDefault();
            if (check == null)
            {
                UserRole u = new UserRole()
                {
                    RoleId = RoleId,
                    UserId = UserId
                };
                db.UserRoles.Add(u);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("Index", new { checkEmail = check.User.Email });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var rs = db.UserRoles.Find(id);
            if (rs != null)
            {
                db.UserRoles.Remove(rs);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}