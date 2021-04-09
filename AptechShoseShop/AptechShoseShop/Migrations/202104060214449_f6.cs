namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbUsers", "CountLogin", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbUsers", "CountLogin");
        }
    }
}
