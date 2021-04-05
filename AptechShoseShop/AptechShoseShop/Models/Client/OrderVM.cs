using System.Collections.Generic;

namespace AptechShoseShop.Models.Client
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public string StatusName { get; set; }
        public List<ProductOrderVM> ListProduct { get; set; }
        public OrderVM(int orderId, string orderDate, string statusName, List<ProductOrderVM> listProduct)
        {
            OrderId = orderId;
            ListProduct = listProduct;
            OrderDate = orderDate;
            StatusName = statusName;
        }
        //public OrderVM()
        //{

        //}
    }
    public class ProductOrderVM
    {

        public int ProductId { get; set; }
        public string ProImg { get; set; }
        public string ProductName { get; set; }
        public string UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string TotalPrice { get; set; }
        public string SubTotalPrice { get; set; }
        public ProductOrderVM(int productId, string proImg, string productName, string unitPrice, int quantity, string size,
            string color, string totalPrice, string subTotalPrice)
        {
            ProductId = productId;
            ProImg = proImg;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Size = size;
            Color = color;
            TotalPrice = totalPrice;
            SubTotalPrice = subTotalPrice;
        }
        //public ProductOrderVM()
        //{

        //}
    }
}