namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ColorName", c => c.String());
            AddColumn("dbo.OrderDetails", "SizeName", c => c.String());
            AddColumn("dbo.Orders", "OrderNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderNote");
            DropColumn("dbo.OrderDetails", "SizeName");
            DropColumn("dbo.OrderDetails", "ColorName");
        }
    }
}
