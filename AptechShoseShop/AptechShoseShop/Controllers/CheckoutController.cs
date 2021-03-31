using AptechShoseShop.Models.Cart;
using AptechShoseShop.Models.Entites;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AptechShoseShop.Controllers
{
    public class CheckoutController : Controller
    {

        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Checkout
        public ActionResult Index()
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
            if ((CustomerName ?? CustomerEmail ?? CustomerAddress) == "")
            {
                ModelState.AddModelError("", "Kiểm tra lại thông tin");
                return View();
            }

            var cart_items = JsonConvert.DeserializeObject<List<CartItem>>(data);
            if (cart_items == null || cart_items.Count == 0)
            {
                return Content("Ko có sp trong giỏ hàng");
            }


            Order orders = new Order()
            {
                CustomerName = CustomerName,
                CustomerEmail = CustomerEmail,
                CustomerAddress = CustomerAddress,
                CustomerPhone = CustomerPhone,
                OrderNote = OrderNote,
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