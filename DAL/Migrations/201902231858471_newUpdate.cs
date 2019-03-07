namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Safes", "Type", c => c.Boolean(nullable: false));
            DropColumn("dbo.Safes", "AmountType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Safes", "AmountType", c => c.Boolean(nullable: false));
            DropColumn("dbo.Safes", "Type");
        }
    }
}
