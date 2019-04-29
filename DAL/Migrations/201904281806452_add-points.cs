namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Point", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Point");
        }
    }
}
