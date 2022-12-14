namespace ClothBazar.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNameAddressToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
