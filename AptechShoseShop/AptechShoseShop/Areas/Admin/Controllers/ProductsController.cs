using AptechShoseShop.Models.Admin;
using AptechShoseShop.Models.Entites;
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
        public ActionResult Index()
        {
            var product = db.Products.OrderByDescending(x => x.Id).ToList();
            return View(product);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ///Viết lại ProductVM có thêm kiểu dữ liệu size cho thuộc tính size 
            var showCate = db.Categories.OrderBy(x => x.Position);

            List<StatusProduct> statusPro = db.StatusProducts.ToList();
            SelectList statusProList = new SelectList(statusPro, "Id", "StatusName", "1");
            ViewBag.StatusPro = statusProList;

            List<Size> size = db.Sizes.ToList();
            SelectList sizeList = new SelectList(size, "Id", "SizeName", "1");
            ViewBag.Size = sizeList;

            List<Color> color = db.Colors.ToList();
            SelectList colorList = new SelectList(color, "Id", "ColorName", "1");
            ViewBag.Color = colorList;

            string date = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.date = date;

            return View(showCate);
        }

        [HttpPost]
        public ActionResult Create(ProductVM product, HttpPostedFileBase[] ProductImageId, List<int> sizeId, List<int> colorId)
        {

            Product newPro = new Product()
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                DiscountRatio = product.DiscountRatio,
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
            ProductSize proSize = new ProductSize();
            foreach (var item in sizeId)
            {
                proSize.ProductId = newPro.Id;
                proSize.SizeId = item;
                db.ProductSizes.Add(proSize);
                db.SaveChanges();
            }

            ProductColor proColor = new ProductColor();
            foreach (var item in colorId)
            {
                proColor.ProductId = newPro.Id;
                proColor.ColorId = item;
                db.ProductColors.Add(proColor);
                db.SaveChanges();
            };

            //Kiểm tra chưa thêm ảnh thì k được lưu
            if (ProductImageId[0] == null)
            {
                ///ViewBag.AddImage = "Bạn chưa thêm ảnh";
                return RedirectToAction("Index");
            }

            ///Lưu vào bảng ProIamge
            ProductImage proImage = new ProductImage();

            foreach (var item in ProductImageId)
            {
                proImage.ImageUrl = item.FileName;
                proImage.ProductId = newPro.Id;
                db.ProductImages.Add(proImage);
                db.SaveChanges();
            }

            ///Lấy id của url đầu tiên của Pro
            string findUrl = ProductImageId[0].FileName;
            var defaultImage = db.ProductImages.Where(x => x.ImageUrl == findUrl).FirstOrDefault();
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


        public ActionResult DeletePro(int id)
        {
            ///Tìm sản phẩm
            var product = db.Products.Find(id);

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
                    return Json("Bạn không được quyền xóa sản phẩm này");
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
                    return Json("Bạn không được quyền xóa sản phẩm này");
                }
            }

            return RedirectToAction("Index");
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