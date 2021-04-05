namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderDetails", "DiscountRatio", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderDetails", "DiscountRatio", c => c.Double(nullable: false));
        }
    }
}
