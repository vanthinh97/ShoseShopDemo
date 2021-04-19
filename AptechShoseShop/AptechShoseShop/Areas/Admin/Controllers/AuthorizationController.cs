using AptechShoseShop.Models.Entites;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : Controller
    {
        readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Authorization
        [HttpGet]
        public ActionResult Index(string checkRole, string checkEmail)
        {
            if (checkEmail != null)
            {
                ViewBag.HasAuthor = "Role " + checkRole + " has been authorized " + checkEmail;
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
                return RedirectToAction("Index", new { checkRole = check.Role.RoleName, checkEmail = check.User.Email });
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