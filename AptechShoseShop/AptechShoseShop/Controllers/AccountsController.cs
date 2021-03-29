using AptechShoseShop.Models;
using AptechShoseShop.Models.Accounts;
using AptechShoseShop.Models.Entites;
using AptechShoseShop.Models.Security;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AptechShoseShop.Controllers
{
    ///[Authorize(Roles = "HRManager,Finance")]
    public class AccountsController : Controller
    {
        private readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Accounts
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


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.TbUsers.Where(x => x.Email.Equals(email)).FirstOrDefault();

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Không được để trống ! ");
                return View();
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
                return View();
            }

            if (user.Password.Equals(MySecurity.EncryptPassword(password)))
            {
                Authen(user.Id);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Mật khẩu không đúng");
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TbUser user)
        {
            var emailUser = db.TbUsers.Where(x => x.Email == user.Email).SingleOrDefault();

            if (emailUser != null)
            {
                return Json("Email này đã tồn tại");
            }

            if (user.Password == null)
            {
                return Json("Email của bạn hợp lệ!");
            }

            TbUser newUser = new TbUser()
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = MySecurity.EncryptPassword(user.Password),
                StatusId = 1,
                CreatedDate = DateTime.Now
            };
            db.TbUsers.Add(newUser);
            db.SaveChanges();
            Authen(newUser.Id);


            //sendmail
            EmailManagement.SendMail(user.Email, "Aptech Shose Shop",
                "<h1>Hello [Name]! bạn đã đăng ký tài khoản thành công tại Aptech Shose Shop</h1>".Replace("[Name]", newUser.FullName));
            return RedirectToAction("Index", "Home");
        }
        ///

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

        [HttpPost]
        public ActionResult ForgotPW(string email)
        {
            var user = db.TbUsers.Where(x => x.Email == email).SingleOrDefault();

            if (user == null)
            {
                return Json("Email bạn nhập vào không đúng");
            }


            Random rdom = new Random();
            var xPass = rdom.Next(100000, 1000000).ToString();
            ///string newPass = xPass.ToString("000000");

            user.Password = MySecurity.EncryptPassword(xPass);
            db.SaveChanges();

            EmailManagement.SendMail(user.Email, "Aptech Shose Shop",
                "<h1>Hello [Name]! Mật khẩu mới của bạn là [newPass]</h1>"
                .Replace("[Name]", user.FullName)
                .Replace("[newPass]", xPass));

            return Json("Bạn hãy kiểm tra email để lấy mật khẩu");
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cookies["UserId"].Expires = DateTime.Now.AddDays(-10);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public void Authen(int userId)
        {
            Response.Cookies["UserId"].Value = userId.ToString();
            FormsAuthentication.SetAuthCookie(userId.ToString(), true);
        }

    }
}