using AptechShoseShop.Models.Cart;
using AptechShoseShop.Models.Entites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class CheckoutController : Controller
    {

        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Checkout
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string data,
            string CustomerName, string CustomerEmail,
            string CustomerAddress,
            string CustomerPhone, string OrderNote
            )
        {
            if (CustomerName == "" || CustomerEmail == "" || CustomerPhone == "" || CustomerAddress == "")
            {
                return Content("Kiểm tra lại thông tin");
            }

            var cart_items = JsonConvert.DeserializeObject<List<CartItem>>(data);
            if (cart_items == null || cart_items.Count == 0)
            {
                return Content("Ko có sp trong giỏ hàng");
            }

            int? userName = null;
            if (User.Identity.IsAuthenticated)
            {
                userName = int.Parse(User.Identity.Name);
            }

            Order orders = new Order()
            {
                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,
                CustomerAddress = CustomerAddress,
                CustomerPhone = CustomerPhone,
                OrderDate = DateTime.Now,
                OrderNote = OrderNote,
                UserId = userName != null ? userName : userName,
                StatusId = 1
            };
            db.Orders.Add(orders);

            foreach (var item in cart_items)
            {
                Product p = db.Products.Find(item.productid);
                OrderDetail od = new OrderDetail()
                {
                    OrderId = orders.Id,
                    ProductId = item.productid,
                    UnitPrice = p.UnitPrice,
                    DiscountRatio = p.DiscountRatio,
                    Quantity = item.quantity,
                    ColorName = item.ColorName,
                    SizeName = item.SizeName
                };
                db.OrderDetails.Add(od);
            }
            db.SaveChanges();
            return Content("OK");
        }
    }
}