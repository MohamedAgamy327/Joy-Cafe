namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class devicesorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "Order", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "Order");
        }
    }
}
