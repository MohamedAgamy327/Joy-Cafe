namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cancledreasonbill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "CancelReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "CancelReason");
        }
    }
}
