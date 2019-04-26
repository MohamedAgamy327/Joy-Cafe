namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cancledbill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "Canceled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bills", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Bills", "Canceled");
            DropColumn("dbo.Bills", "Deleted");
        }
    }
}
