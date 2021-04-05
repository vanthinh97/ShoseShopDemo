namespace AptechShoseShop.Models.Admin
{
    public class OrderDetailVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

    }
}