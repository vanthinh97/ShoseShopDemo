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
            SelectList sizeList = new SelectList(size, "Id", "SizeName");
            ViewBag.Size = sizeList;

            List<Color> color = db.Colors.ToList();
            SelectList colorList = new SelectList(color, "Id", "ColorName");
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
    }
}