namespace AptechShoseShop.Models.Entites
{
    public class Banner
    {
        public Banner(string name, string description, string urlImageBanner, string linkBanner, int position)
        {
            Name = name;
            Description = description;
            UrlImageBanner = urlImageBanner;
            LinkBanner = linkBanner;
            Position = position;
        }
        public Banner()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlImageBanner { get; set; }
        public string LinkBanner { get; set; }
        public int Position { get; set; }
    }
}