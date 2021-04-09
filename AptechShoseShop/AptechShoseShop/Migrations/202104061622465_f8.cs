namespace AptechShoseShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UrlIcon = c.String(),
                        PolicyName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Policies");
        }
    }
}
