namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class updatebilldelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "IsDeleted");
        }
    }
}
