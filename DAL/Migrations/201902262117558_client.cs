namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class client : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Telephone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Telephone", c => c.String());
        }
    }
}
