namespace AptechShoseShop.Models.Admin
{
    public class ProInOrderDetailVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SellPrice { get; set; }
        public int? Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
    }
}