using System.Collections.Generic;

namespace AptechShoseShop.Models.Admin
{
    public class OrderAdminVM
    {
        public int StatusId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string OrderDate { get; set; }
        public string OrderNote { get; set; }
        public IEnumerable<OrderDetailAdminVM> orderDetailAdminVMs { get; set; }

    }

    public class OrderDetailAdminVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
    }
}