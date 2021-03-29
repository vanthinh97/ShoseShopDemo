namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "DiscountRatio", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "DiscountRatio", c => c.Double(nullable: false));
        }
    }
}
