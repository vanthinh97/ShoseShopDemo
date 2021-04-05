namespace AptechShoseShop.Models.Entites
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public double? DiscountRatio { get; set; }
        public int Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}