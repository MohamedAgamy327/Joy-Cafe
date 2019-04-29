namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpointssystem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "CurrentMembershipMinutes", c => c.Int());
            AddColumn("dbo.Bills", "CurrentPoints", c => c.Int());
            AddColumn("dbo.Bills", "UsedPoints", c => c.Int());
            AddColumn("dbo.Bills", "PointsAfterUse", c => c.Int());
            AddColumn("dbo.Bills", "EarnedPoints", c => c.Int());
            AddColumn("dbo.Clients", "Points", c => c.Int());
            DropColumn("dbo.Bills", "MembershipMinutes");
            DropColumn("dbo.Bills", "Point");
            DropColumn("dbo.Clients", "Point");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Point", c => c.Int());
            AddColumn("dbo.Bills", "Point", c => c.Int());
            AddColumn("dbo.Bills", "MembershipMinutes", c => c.Int());
            DropColumn("dbo.Clients", "Points");
            DropColumn("dbo.Bills", "EarnedPoints");
            DropColumn("dbo.Bills", "PointsAfterUse");
            DropColumn("dbo.Bills", "UsedPoints");
            DropColumn("dbo.Bills", "CurrentPoints");
            DropColumn("dbo.Bills", "CurrentMembershipMinutes");
        }
    }
}
