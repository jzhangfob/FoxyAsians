namespace FoxyAsians.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetRoles", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", new[] { "AppUser_Id" });
            DropColumn("dbo.AspNetRoles", "AppUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetRoles", "AppUser_Id");
            AddForeignKey("dbo.AspNetRoles", "AppUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
