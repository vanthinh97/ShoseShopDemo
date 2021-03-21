namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbUsers", "NumberPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbUsers", "NumberPhone");
        }
    }
}
