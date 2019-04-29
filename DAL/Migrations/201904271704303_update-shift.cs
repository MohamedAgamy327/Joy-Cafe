namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateshift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "TotalMinimum", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shifts", "TotalDevices", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Shifts", "TotalItems", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "TotalItems");
            DropColumn("dbo.Shifts", "TotalDevices");
            DropColumn("dbo.Shifts", "TotalMinimum");
        }
    }
}
