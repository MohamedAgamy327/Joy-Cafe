namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class client_telephone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Telephone", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Telephone", c => c.String(nullable: false));
        }
    }
}
