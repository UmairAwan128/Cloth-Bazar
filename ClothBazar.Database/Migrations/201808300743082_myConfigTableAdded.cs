namespace ClothBazar.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myConfigTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.myConfigs",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.myConfigs");
        }
    }
}
