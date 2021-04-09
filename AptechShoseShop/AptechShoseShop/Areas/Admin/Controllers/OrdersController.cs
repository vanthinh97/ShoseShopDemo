﻿using AptechShoseShop.Models.Admin;
using AptechShoseShop.Models.Entites;
using Newtonsoft.Json;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class OrdersController : Controller
    {
        AptechShoseShopDbContext db = new AptechShoseShopDbContext();
        // GET: Admin/Orders
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var orders = db.Orders.OrderByDescending(x => x.Id);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var status = db.StatusOrders.ToList();
            ViewBag.Status = status;

            var colors = db.Colors.ToList();
            ViewBag.Colors = colors;

            var sizes = db.Sizes.ToList();
            ViewBag.Sizes = sizes;

            return View();
        }

        [HttpGet]
        public string GetProductById(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return null;
            }

            ProInOrderDetailVM proFinded = new ProInOrderDetailVM()
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                SellPrice = (product.UnitPrice - ((product.UnitPrice * product.DiscountRatio) / 100)).ToString(),
            };

            return JsonConvert.SerializeObject(proFinded);
        }

        //[HttpGet]
        //public ActionResult BindingOrderDetail()
        //{
        //    var colors = db.Colors.ToList();
        //    ViewBag.Colors = colors;

        //    var sizes = db.Sizes.ToList();
        //    ViewBag.Sizes = sizes;

        //    return PartialView("_CreateOrderDeail");
        //}

        // TODO: them san pham
        [HttpPost]
        public ActionResult Create(OrderAdminVM postOrder)
        {
            //Order newOrder = new Order()
            //{
            //    StatusId = order.StatusId,
            //    CustomerName = order.CustomerName,
            //    CustomerEmail = order.CustomerEmail,
            //    CustomerPhone = order.CustomerPhone,
            //    CustomerAddress = order.CustomerAddress,
            //    OrderNote = order.OrderNote,
            //    OrderDate = DateTime.Now
            //};
            //db.Orders.Add(newOrder);

            //foreach (var item in proDetail)
            //{
            //    var product = db.Products.Find(item.ProductId);
            //    OrderDetail newProDetail = new OrderDetail()
            //    {
            //        OrderId = newOrder.Id,
            //        ProductId = product.Id,
            //        UnitPrice = product.UnitPrice,
            //        DiscountRatio = product.DiscountRatio,
            //        Quantity = item.Quantity.Value,
            //        ColorName = item.ColorName,
            //        SizeName = item.SizeName
            //    };
            //    db.OrderDetails.Add(newProDetail);
            //}

            //db.SaveChanges();

            return RedirectToAction("Index", "Orders");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var order = db.Orders.Find(id);
            var orderDetail = order.OrderDetails.ToList();
            double subTotal = 0;
            foreach (var item in orderDetail)
            {
                double unitPrice = item.UnitPrice - ((item.UnitPrice * item.DiscountRatio.Value) / 100);
                int quantity = item.Quantity;
                double Tien = unitPrice * quantity;
                subTotal += Tien;
            }
            ViewBag.SubTotal = (subTotal * 1.1).ToString();

            return View(order);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var order = db.Orders.Find(id);
            var status = db.StatusOrders.ToList();
            ViewBag.Status = status;

            var oldOrderDetails = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            OrderDetailVM OrderDetail;
            List<OrderDetailVM> listOrderDetail = new List<OrderDetailVM>();

            double subTotal = 0;
            foreach (var item in oldOrderDetails)
            {
                OrderDetail = new OrderDetailVM();
                OrderDetail.Id = item.Id;
                OrderDetail.ProductName = item.Product.ProductName;
                OrderDetail.UnitPrice = item.UnitPrice - ((item.UnitPrice * item.DiscountRatio.Value) / 100);
                OrderDetail.Quantity = item.Quantity;
                OrderDetail.Color = item.ColorName;
                OrderDetail.Size = item.SizeName;
                listOrderDetail.Add(OrderDetail);

                double Tien = OrderDetail.UnitPrice * OrderDetail.Quantity;
                subTotal += Tien;
            }
            ViewBag.listOrderDetail = listOrderDetail;
            ViewBag.SubTotal = (subTotal * 1.1).ToString();

            int FindProId = oldOrderDetails[0].ProductId;
            var colors = db.ProductColors.Where(x => x.ProductId == FindProId).ToList();
            ViewBag.Colors = colors;

            var sizes = db.ProductSizes.Where(x => x.ProductId == FindProId).ToList();
            ViewBag.Sizes = sizes;

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            var editOrder = db.Orders.Find(order.Id);

            if (editOrder == null)
            {
                return View("Index");
            }
            editOrder.CustomerName = order.CustomerName;
            editOrder.CustomerAddress = order.CustomerAddress;
            editOrder.CustomerEmail = order.CustomerEmail;
            editOrder.CustomerPhone = order.CustomerPhone;
            editOrder.StatusId = order.StatusId;

            db.SaveChanges();

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public ActionResult EditDetail(ChangeOrderDetail NewOrder)
        {
            var changeOrder = db.OrderDetails.Find(NewOrder.Id);
            changeOrder.Quantity = NewOrder.Quantity;
            changeOrder.ColorName = NewOrder.Color;
            changeOrder.SizeName = NewOrder.Size;
            db.SaveChanges();

            return Json("Bạn đã thay đổi đơn hàng!");
        }

        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);
            var orderDetail = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            foreach (var item in orderDetail)
            {
                db.OrderDetails.Remove(item);
            };
            db.Orders.Remove(order);
            db.SaveChanges();

            return Content("OK");
        }

        [HttpGet]
        public ActionResult _OrderDetail(int OrderId)
        {
            ViewBag.OrderId = OrderId;
            var data = db.OrderDetails.Where(x => x.OrderId == OrderId).ToList();
            return PartialView(data);
        }
    }
}