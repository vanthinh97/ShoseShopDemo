using AptechShoseShop.Models;
using AptechShoseShop.Models.Entites;
using AptechShoseShop.Models.Security;
using System;
using System.Collections.Generic;
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
            //if (User.Identity.IsAuthenticated)
            //{
            //    int userId = int.Parse(User.Identity.Name);
            //    var account = db.TbUsers.Find(userId);
            //    return View(account);
            //}
            var user = db.TbUsers.Where(x => x.Id == 3).SingleOrDefault();
            ViewBag.Url = "/Data/users/" + user.Id + "/" + user.Avatar;
            return View(user);
        }


        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file)
        {
            string fileName = file.FileName;
            int userId = 3;
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
                ModelState.AddModelError("", "Email này đã tồn tại");
                return View();
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
            EmailManagement.SendMail(user.Email, "Chuc mung dang ky thanh cong", "<h1>Hello [Name], ban da dag ky</h1>".Replace("[Name]", newUser.FullName));
            return RedirectToAction("Index", "Home");
            ///return Redirect(Request.UrlReferrer.ToString());
        }
        ///

        [HttpGet]
        public ActionResult EditProfile()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    int userId = int.Parse(User.Identity.Name);
            //    var account = db.TbUsers.Find(userId);
            //    return View(account);
            //}

            //return Redirect(Request.UrlReferrer.ToString()); 

            var user = db.TbUsers.Where(x => x.Id == 3).SingleOrDefault();
            
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(int Id, string FullName, int Gender, string Address)
        {
            bool? GenderV2 = null;
            //if (Gender != -1)
            //{
            //    if (Gender != 1)
            //    {
            //        GenderV2 = false;
            //    }
            //    else
            //    {
            //        GenderV2 = true;
            //    }
            //}

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
            newUser.Address = Address;
            db.SaveChanges();

            return RedirectToAction("EditProfile");
        }

        public void Authen(int userId)
        {
            Response.Cookies["UserId"].Value = userId.ToString();
            FormsAuthentication.SetAuthCookie(userId.ToString(), true);
        }

    }
}