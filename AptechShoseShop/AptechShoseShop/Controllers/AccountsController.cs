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
        [HttpGet]
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
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = int.Parse(User.Identity.Name);

                ///Check role
                var userRole = db.UserRoles.Where(x => x.UserId == id);
                if (userRole != null)
                {
                    foreach (var item in userRole)
                    {
                        if (item.RoleId == 1 || item.RoleId == 2)
                        {
                            return RedirectToAction("Index", "DashBoard", new { area = "Admin" });
                        }
                    }
                }

                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.TbUsers.Where(x => x.Email.Equals(email)).FirstOrDefault();

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Do not leave it blank ! ");
                return View();
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Account does not exist !");
                return View();
            }

            if (user.TimeLock != null)
            {
                var timeCurrent = DateTime.Now;
                if (timeCurrent < user.TimeLock)
                {
                    user.CountLogin += 1;
                    if (user.CountLogin >= 6)
                    {
                        user.StatusId = 2;
                        user.TimeLock = null;
                        db.SaveChanges();
                        ModelState.AddModelError("", "The account has been locked, to retrieve your password click on \"Account is blocked?\"");
                        return View();
                    }
                    db.SaveChanges();
                    ModelState.AddModelError("", $"Account has been blocked until {user.TimeLock}");
                    return View();
                }
                else
                {
                    user.TimeLock = null;
                    db.SaveChanges();
                }
            }

            if (user.StatusId == 2)
            {
                ModelState.AddModelError("", "Account is blocked");
                return View();
            }

            if (user.Password.Equals(MySecurity.EncryptPassword(password)))
            {
                Authen(user.Id);
                user.CountLogin = 0;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                user.CountLogin += 1;
                if (user.CountLogin == 3)
                {
                    DateTime currentTime = DateTime.Now;
                    user.TimeLock = currentTime.AddMinutes(15);
                    db.SaveChanges();
                    ModelState.AddModelError("", "Account has been locked for 15 minutes");
                    return View();
                }
                if (user.CountLogin >= 6)
                {
                    user.StatusId = 2;
                    user.TimeLock = null;
                    db.SaveChanges();
                    ModelState.AddModelError("", "The account has been locked, to retrieve your password click on \"Account is blocked?\"");
                    return View();
                }
                db.SaveChanges();
                ModelState.AddModelError("", "Incorrect password");
            }

            return View();
        }

        [HttpGet]
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
                return Json("This email already exists");
            }

            if (user.Password == null)
            {
                return Json("Your email is valid!");
            }

            TbUser newUser = new TbUser()
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = MySecurity.EncryptPassword(user.Password),
                StatusId = 1,
                CreatedDate = DateTime.Now,
                CountLogin = 0
            };
            db.TbUsers.Add(newUser);
            db.SaveChanges();
            Authen(newUser.Id);

            //sendmail
            EmailManagement.SendMail(user.Email, "Aptech Shose Shop",
                "<h1>Hello [Name]! You have successfully registered an account at Aptech Shose Shop</h1>".Replace("[Name]", newUser.FullName));
            return RedirectToAction("Index", "Home");
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
                return Json("Old password is incorrect, please enter again!");
            }

            user.Password = MySecurity.EncryptPassword(changePW.newPassword);
            db.SaveChanges();

            return Json("Password has changed!");
        }

        [HttpPost]
        public ActionResult ForgotPW(string email)
        {
            var user = db.TbUsers.Where(x => x.Email == email).SingleOrDefault();

            if (user == null)
            {
                return Json("The email you entered is not correct");
            }


            Random rdom = new Random();
            var xPass = rdom.Next(100000, 1000000).ToString();
            ///string newPass = xPass.ToString("000000");

            user.Password = MySecurity.EncryptPassword(xPass);
            user.StatusId = 1;
            user.CountLogin = 0;
            user.TimeLock = null;
            db.SaveChanges();

            EmailManagement.SendMail(user.Email, "Aptech Shose Shop",
                "<h1>Hello [Name]! Your new password is [newPass]</h1>"
                .Replace("[Name]", user.FullName)
                .Replace("[newPass]", xPass));

            return Json("Please check your email for the password");
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



        [HttpPost]
        public ActionResult LoginCkout(string Email1, string Password1)
        {
            if (Email1 == "" || Password1 == "")
            {
                return Json("NOTNULL");
            }
            var user = db.TbUsers.Where(x => x.Email.Equals(Email1)).SingleOrDefault();
            if (user == null)
            {
                return Json("NULL");
            }
            if (!string.IsNullOrEmpty(Email1) && !string.IsNullOrEmpty(Password1))
            {
                if (user.Password.Equals(MySecurity.EncryptPassword(Password1)))
                {
                    Authen(user.Id);

                    //Response.Cookies["UserId"].Value = user.Id.ToString();
                    //FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);

                    return Json("OK");
                }
                else
                {
                    return Json("NOTCORRECT");
                }
            }
            return Json("OK");
        }


    }
}