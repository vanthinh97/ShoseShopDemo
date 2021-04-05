using AptechShoseShop.Models.Admin;
using AptechShoseShop.Models.Entites;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var product = db.Products.OrderByDescending(x => x.Id);
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ///int id = 6;
            int id = int.Parse(User.Identity.Name);
            ViewBag.UserID = id;

            ///Viết lại ProductVM có thêm kiểu dữ liệu size cho thuộc tính size 
            var showCate = db.Categories.OrderBy(x => x.Position);

            ///ViewBag bằng dropdowlist
            List<StatusProduct> statusPro = db.StatusProducts.ToList();
            SelectList statusProList = new SelectList(statusPro, "Id", "StatusName", "1");
            ViewBag.StatusPro = statusProList;

            ///ViewBag bình thường bằng checkBox input
            List<Size> size = db.Sizes.ToList();
            ViewBag.Size = size;

            List<Color> color = db.Colors.ToList();
            ViewBag.Color = color;

            string date = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.date = date;

            return View(showCate);
        }

        [HttpPost]
        [ValidateInput(false)] ///để bỏ qua lỗi dangerous Request của validate input
        public ActionResult Create(ProductVM product, HttpPostedFileBase[] ProductImageId, List<int> SizeId, List<int> ColorId)
        {

            Product newPro = new Product()
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                DiscountRatio = product.DiscountRatio != null ? product.DiscountRatio : 0,
                DiscountExpiry = product.DiscountExpiry,
                CategoryId = product.CategoryId,
                Description = product.Description,
                CreatedDate = product.CreatedDate,
                CreatedBy = product.CreatedBy,
                StatusId = product.StatusId
            };
            db.Products.Add(newPro);
            db.SaveChanges();

            ///Thêm vào bảng size và colorProduct
            ///Chỉ cần khởi tạo mới 1 lần, khi add xong thì ta phải new lại 1 đối tượng khác trong vòng lặp
            if (SizeId != null && SizeId.Count > 0)
            {
                ProductSize proSize;
                foreach (var item in SizeId)
                {
                    proSize = new ProductSize();
                    proSize.ProductId = newPro.Id;
                    proSize.SizeId = item;
                    db.ProductSizes.Add(proSize);
                }
            }


            if (ColorId != null && ColorId.Count > 0)
            {
                ProductColor proColor;
                foreach (var item in ColorId)
                {
                    proColor = new ProductColor();
                    proColor.ProductId = newPro.Id;
                    proColor.ColorId = item;
                    db.ProductColors.Add(proColor);
                }
            }


            //Kiểm tra chưa thêm ảnh thì k được lưu
            if (ProductImageId[0] == null)
            {
                ///ViewBag.AddImage = "Bạn chưa thêm ảnh";
                return RedirectToAction("Index");
            }

            ///Lưu vào bảng ProIamge
            ProductImage proImage;

            foreach (var item in ProductImageId)
            {
                proImage = new ProductImage();
                proImage.ImageUrl = item.FileName;
                proImage.ProductId = newPro.Id;
                db.ProductImages.Add(proImage);
            }
            db.SaveChanges();

            ///Lấy id của url đầu tiên của Pro
            string findUrl = ProductImageId[0].FileName;
            var defaultImage = db.ProductImages.Where(x => x.ProductId == newPro.Id).Where(x => x.ImageUrl == findUrl).FirstOrDefault();
            newPro.ProductImageId = defaultImage.Id;
            db.SaveChanges();

            //Tạo thư mục ảnh
            string strFolder = Server.MapPath("~/Data/Products/" + newPro.Id);

            if (!Directory.Exists(strFolder))
            {
                Directory.CreateDirectory(strFolder);
            }
            foreach (var item in ProductImageId)
            {
                item.SaveAs(strFolder + @"\" + item.FileName);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string ErrorCreatBy)
        {
            ///int id = 86;
            if (ErrorCreatBy != null)
            {
                ViewBag.ErrorCreatBy = "Bạn không được edit nếu không phải là người tạo";
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var listImage = db.ProductImages.Where(x => x.ProductId == id).ToList();
            ViewBag.ListImage = listImage;

            ViewBag.Url = "/Data/products/" + product.Id + "/" + product.ProductImage.ImageUrl;

            var cates = db.Categories.ToList();
            ViewBag.Cates = cates;

            var listProStatus = db.StatusProducts.ToList();
            ViewBag.ListProStatus = listProStatus;

            var listSize = db.Sizes.ToList();
            ViewBag.ListSize = listSize;
            var listProSize = db.ProductSizes.Where(x => x.ProductId == id).ToList();
            List<int> listSizeChecked = new List<int>();
            foreach (var item in listProSize)
            {
                listSizeChecked.Add(item.SizeId);
            }
            ViewBag.ListSizeChecked = listSizeChecked;

            var listColor = db.Colors.ToList();
            ViewBag.ListColor = listColor;
            var listProColor = db.ProductColors.Where(x => x.ProductId == id).ToList();
            List<int> listColorChecked = new List<int>();
            foreach (var item in listProColor)
            {
                listColorChecked.Add(item.ColorId);
            }
            ViewBag.ListColorChecked = listColorChecked;



            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM pro, HttpPostedFileBase[] ProductImageId, int[] ImgDelete, List<int> SizeId, List<int> ColorId)
        {
            int userId = int.Parse(User.Identity.Name);
            ///Lấy user Admin
            var userRole = db.UserRoles.Where(x => x.UserId == userId).Where(x => x.RoleId == 1).SingleOrDefault();

            var product = db.Products.Find(pro.Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            ///Không phải là người tạo và k phải admin thì k cho edit
            if (userId != product.CreatedBy && userRole == null)
            {
                return RedirectToAction("Edit", new { id = pro.Id, ErrorCreatBy = "Error" });
            }

            product.ProductName = pro.ProductName;
            product.UnitPrice = pro.UnitPrice;
            product.DiscountRatio = pro.DiscountRatio != null ? pro.DiscountRatio : 0;
            product.DiscountExpiry = pro.DiscountExpiry;
            product.CategoryId = pro.CategoryId;
            product.Description = pro.Description;
            product.CreatedDate = pro.CreatedDate;
            product.CreatedBy = pro.CreatedBy;
            product.StatusId = pro.StatusId;

            string strFolder = "";

            ///Thêm proImage và lưu ảnh mới vào file
            if (ProductImageId[0] != null)
            {
                ProductImage proImage;
                foreach (var item in ProductImageId)
                {
                    proImage = new ProductImage();
                    proImage.ImageUrl = item.FileName;
                    proImage.ProductId = pro.Id;
                    db.ProductImages.Add(proImage);
                    strFolder = Server.MapPath("~/Data/Products/" + pro.Id);
                    item.SaveAs(strFolder + @"\" + item.FileName);
                }
            }

            ///Xóa ảnh cũ
            foreach (var item in ImgDelete)
            {
                if (item > 0)
                {
                    var proDelete = db.ProductImages.Find(item);
                    strFolder = Server.MapPath("~/data/products/" + proDelete.ProductId);
                    if (System.IO.File.Exists(strFolder + @"\" + proDelete.ImageUrl))
                    {
                        System.IO.File.Delete(strFolder + @"\" + proDelete.ImageUrl);
                    }
                    db.ProductImages.Remove(proDelete);
                }
            }

            ///Thêm mới và xóa size
            if (SizeId != null && SizeId.Count > 0)
            {
                List<int> listOldSize = new List<int>();
                var proOldSize = db.ProductSizes.Where(x => x.ProductId == pro.Id).ToList();
                foreach (var item in proOldSize)
                {
                    listOldSize.Add(item.SizeId);
                }

                ProductSize proSize;
                foreach (var item in SizeId)
                {
                    if (!listOldSize.Contains(item))
                    {
                        proSize = new ProductSize();
                        proSize.ProductId = pro.Id;
                        proSize.SizeId = item;
                        db.ProductSizes.Add(proSize);
                    }
                }
                foreach (var item in proOldSize)
                {
                    if (!SizeId.Contains(item.SizeId))
                    {
                        db.ProductSizes.Remove(item);
                    }
                }
            }

            ///Thêm mới và xóa color
            if (ColorId != null && ColorId.Count > 0)
            {
                List<int> listOldColor = new List<int>();
                var proOldColor = db.ProductColors.Where(x => x.ProductId == pro.Id).ToList();
                foreach (var item in proOldColor)
                {
                    listOldColor.Add(item.ColorId);
                }

                ProductColor proColor;
                foreach (var item in ColorId)
                {
                    if (!listOldColor.Contains(item))
                    {
                        proColor = new ProductColor();
                        proColor.ProductId = pro.Id;
                        proColor.ColorId = item;
                        db.ProductColors.Add(proColor);
                    }
                }
                foreach (var item in proOldColor)
                {
                    if (!ColorId.Contains(item.ColorId))
                    {
                        db.ProductColors.Remove(item);
                    }
                }
            }
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = pro.Id });
        }

        public ActionResult Detail(int id)
        {
            var listImage = db.ProductImages.Where(x => x.ProductId == id).ToList();
            ViewBag.ListImage = listImage;

            var product = db.Products.Find(id);
            ViewBag.Url = "/Data/products/" + product.Id + "/" + product.ProductImage.ImageUrl;

            var cates = db.Categories.ToList();
            ViewBag.Cates = cates;

            var listProStatus = db.StatusProducts.ToList();
            ViewBag.ListProStatus = listProStatus;

            var listSize = db.Sizes.ToList();
            ViewBag.ListSize = listSize;
            var listProSize = db.ProductSizes.Where(x => x.ProductId == id).ToList();
            List<int> listSizeChecked = new List<int>();
            foreach (var item in listProSize)
            {
                listSizeChecked.Add(item.SizeId);
            }
            ViewBag.ListSizeChecked = listSizeChecked;

            var listColor = db.Colors.ToList();
            ViewBag.ListColor = listColor;
            var listProColor = db.ProductColors.Where(x => x.ProductId == id).ToList();
            List<int> listColorChecked = new List<int>();
            foreach (var item in listProColor)
            {
                listColorChecked.Add(item.ColorId);
            }
            ViewBag.ListColorChecked = listColorChecked;

            return View(product);
        }

        public ActionResult DeletePro(int id)
        {
            ///Tìm sản phẩm
            var product = db.Products.Find(id);
            string error = "Bạn không được quyền xóa sản phẩm này";

            ///Tìm id đang đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.Identity.Name);

                ///So sánh id đăng nhập có trùng với id tạo sp k
                ///Nếu trùng thì mới cho xóa
                if (product.CreatedBy == userId)
                {
                    ///Set NULL cho trường này thì mới xóa được trong bảng ProImage
                    product.ProductImageId = null;

                    ///Hàm xóa ảnh
                    DeleteProId(product.Id);

                    db.Products.Remove(product);
                    db.SaveChanges();

                    return Content("OK");
                }

                ///Lấy ra object của id đăng nhập trong bảng UserRole
                var account = db.UserRoles.Where(x => x.UserId == userId).FirstOrDefault();

                ///Check null
                if (account == null)
                {
                    return Json(error);
                }

                ///Nếu = 1, tức là = Admin được được phép xóa tất cả ảnh
                if (account.RoleId == 1)
                {
                    product.ProductImageId = null;

                    DeleteProId(product.Id);

                    db.Products.Remove(product);
                    db.SaveChanges();

                    return Content("OK");
                }
                else
                {
                    return Json(error);
                }
            }

            return Json(error);
        }

        public void DeleteProId(int productId)
        {
            var RmProductImage = db.ProductImages.Where(x => x.ProductId == productId).ToList();
            foreach (var item in RmProductImage)
            {
                db.ProductImages.Remove(item);
                db.SaveChanges();
            }
        }
    }
}