namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class updateBill : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Devices", "BillID");
            AddForeignKey("dbo.Devices", "BillID", "dbo.Bills", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "BillID", "dbo.Bills");
            DropIndex("dbo.Devices", new[] { "BillID" });
        }
    }
}
