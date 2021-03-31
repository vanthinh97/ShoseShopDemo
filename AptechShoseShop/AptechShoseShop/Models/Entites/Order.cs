using System;
using System.Collections.Generic;

namespace AptechShoseShop.Models.Entites
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public int StatusId { get; set; }
        public int? UserId { get; set; }
        public string OrderNote { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual StatusOrder Status { get; set; }
        public virtual TbUser User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}