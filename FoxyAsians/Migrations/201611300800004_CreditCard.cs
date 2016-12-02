namespace FoxyAsians.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreditCard1", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreditCard2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreditCard2");
            DropColumn("dbo.AspNetUsers", "CreditCard1");
        }
    }
}
