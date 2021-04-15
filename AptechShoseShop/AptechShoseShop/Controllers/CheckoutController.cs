using AptechShoseShop.Models;
using AptechShoseShop.Models.Cart;
using AptechShoseShop.Models.Entites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            string emailOrderDetail = string.Empty;
            double subTotal = 0;
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

                string priceInEmail = (p.UnitPrice - ((p.UnitPrice * p.DiscountRatio) / 100)).ToString();
                string quanInEmail = item.quantity.ToString();
                subTotal += (p.UnitPrice - ((p.UnitPrice * p.DiscountRatio.Value) / 100)) * item.quantity;
                emailOrderDetail += "<tr>" +
                    "<td width='80%' class='purchase_item'><span class='f-fallback'>{" + od.Product.ProductName + "}</span></td>" +
                    "<td class='align-right' width='20%' class='purchase_item'><span class='f-fallback'>" + quanInEmail + " x $" + priceInEmail + "</span></td>" +
                    "</tr>";
            }
            db.SaveChanges();
            subTotal *= 1.1;

            //Send mail
            string CreateBody()
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Checkout/Receipt.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ purchase_date }", DateTime.Now.ToString("yyyy-MM-dd"));
                body = body.Replace("{name}", CustomerName);
                body = body.Replace("{receipt_id}", orders.Id.ToString());
                body = body.Replace("{OrderDate}", DateTime.Now.ToString());
                body = body.Replace("{receipt_details}", emailOrderDetail);
                body = body.Replace("{total}", subTotal.ToString());

                return body;
            }
            EmailManagement.SendMail(CustomerEmail, "Aptech Shose Shop", CreateBody());

            return Content("OK");
        }


    }
}