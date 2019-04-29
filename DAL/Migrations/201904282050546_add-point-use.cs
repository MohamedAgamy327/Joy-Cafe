namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addpointuse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "PointsAfterUsed", c => c.Int());
            DropColumn("dbo.Bills", "PointsAfterUse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "PointsAfterUse", c => c.Int());
            DropColumn("dbo.Bills", "PointsAfterUsed");
        }
    }
}
