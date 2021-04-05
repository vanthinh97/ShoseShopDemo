using AptechShoseShop.Models.Client;
using AptechShoseShop.Models.Entites;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class OrderController : Controller
    {
        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Order
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                #region
                //int accountId = int.Parse(User.Identity.Name);
                //var orderDetails = db.OrderDetails.Where(x => x.Order.UserId == accountId);

                //List<OrderVM> ListOrderVM = new List<OrderVM>();
                //OrderVM orderVM;

                //foreach (var item in orderDetails)
                //{
                //    orderVM = new OrderVM();
                //    orderVM.OrderDate = item.Order.OrderDate;
                //    orderVM.ProductId = item.Product.Id;
                //    orderVM.ProductColor = item.ColorName;
                //    orderVM.ProductSize = item.SizeName;
                //    orderVM.Price = (item.UnitPrice - ((item.UnitPrice * item.DiscountRatio) / 100)).ToString();
                //    orderVM.Quantity = item.Quantity;
                //    double tien = (item.UnitPrice - ((item.UnitPrice * item.DiscountRatio.Value) / 100)) * item.Quantity;
                //    orderVM.Total = tien.ToString();

                //    ListOrderVM.Add(orderVM);
                //}
                #endregion

                List<OrderVM> listResult = new List<OrderVM>();
                int userId = int.Parse(User.Identity.Name);
                OrderVM o;
                foreach (var item in db.Orders.Where(x => x.UserId == userId))
                {
                    o = new OrderVM(item.Id, item.OrderDate.ToString(), item.Status.StatusName, listProduct(item.Id));
                    listResult.Add(o);
                }


                return View(listResult);
            }

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
        public List<ProductOrderVM> listProduct(int orderId)
        {
            List<ProductOrderVM> list = new List<ProductOrderVM>();
            double SubTotal = 0;
            foreach (var item in db.OrderDetails.Where(x => x.OrderId == orderId))
            {
                var unitPrice = item.UnitPrice - ((item.UnitPrice * item.DiscountRatio.Value) / 100);
                var Url = item.Product.ProductImageId != null ? "/Data/products/" + item.Product.Id + "/" + item.Product.ProductImage.ImageUrl
                            : "https://i.pinimg.com/originals/2e/06/83/2e06833f1d8a0f3fd8ff602e60be77d5.jpg";
                double Tien = unitPrice * item.Quantity;
                var Total = Tien.ToString();
                SubTotal += Tien;
                list.Add(new ProductOrderVM(item.ProductId, Url, item.Product.ProductName, unitPrice.ToString(),
                    item.Quantity, item.SizeName, item.ColorName, Total.ToString(), SubTotal.ToString()));
            }
            return list;
        }

        [HttpPost]
        public ActionResult Cancel(int id)
        {
            var order = db.Orders.Find(id);
            var orderDetail = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            //foreach (var item in orderDetail)
            //{
            //    db.OrderDetails.Remove(item);
            //};
            //db.Orders.Remove(order);
            order.StatusId = 4;
            db.SaveChanges();

            return Content("OK");
        }
    }
}