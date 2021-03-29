using AptechShoseShop.Models.Accounts;
using AptechShoseShop.Models.Entites;
using AptechShoseShop.Models.Security;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Accounts
        public ActionResult Index()
        {
            int userId = int.Parse(User.Identity.Name);
            var user = db.TbUsers.Find(userId);
            ViewBag.Url = "/Data/users/" + user.Id + "/" + user.Avatar;
            return View(user);
        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file)
        {
            string fileName = file.FileName;
            int userId = int.Parse(User.Identity.Name);
            TbUser user = db.TbUsers.Find(userId);

            string strFolder = Server.MapPath("~/data/users/" + user.Id);



            if (System.IO.File.Exists(strFolder + @"\" + user.Avatar))
            {
                System.IO.File.Delete(strFolder + @"\" + user.Avatar);
            }



            if (!Directory.Exists(strFolder))
            {
                Directory.CreateDirectory(strFolder);
            }
            file.SaveAs(strFolder + @"\" + fileName);

            user.Avatar = fileName;
            db.SaveChanges();
            return Content("/data/users/" + user.Id + "/" + fileName);
        }

        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            var account = db.TbUsers.Find(id);
            return View(account);
        }

        [HttpPost]
        public ActionResult EditProfile(int Id, string FullName, int Gender, string NumberPhone, string Address)
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
            db.SaveChanges();

            return RedirectToAction("EditProfile");
        }

        [HttpGet]
        public ActionResult ChangePW(int id)
        {
            TbUser user = db.TbUsers.Find(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePW(ChangePW changePW)
        {
            int userId = int.Parse(User.Identity.Name);
            TbUser user = db.TbUsers.Find(userId);

            if (!user.Password.Equals(MySecurity.EncryptPassword(changePW.oldPassword)))
            {
                return Json("Mật khẩu cũ không đúng, hãy nhập lại!");
            }

            user.Password = MySecurity.EncryptPassword(changePW.newPassword);
            db.SaveChanges();

            return Json("Mật khẩu đã thay đổi!");
        }
    }
}