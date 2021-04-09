namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbUsers", "TimeLock", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbUsers", "TimeLock");
        }
    }
}
