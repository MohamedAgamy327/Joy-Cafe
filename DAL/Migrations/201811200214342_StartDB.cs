namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class StartDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        ClientID = c.Int(),
                        PlayedMinutes = c.Int(),
                        MembershipMinutes = c.Int(),
                        MembershipMinutesPaid = c.Int(),
                        MembershipMinutesAfterPaid = c.Int(),
                        RemainderMinutes = c.Int(),
                        Point = c.Int(),
                        Type = c.String(),
                        Minimum = c.Decimal(precision: 18, scale: 2),
                        DevicesSum = c.Decimal(precision: 18, scale: 2),
                        ItemsSum = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Ratio = c.Decimal(precision: 18, scale: 2),
                        TotalAfterDiscount = c.Decimal(precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Date = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.ClientID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.BillDevices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(nullable: false),
                        DeviceID = c.Int(nullable: false),
                        MinutePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        Duration = c.Int(),
                        GameType = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bills", t => t.BillID, cascadeDelete: true)
                .ForeignKey("dbo.Devices", t => t.DeviceID, cascadeDelete: true)
                .Index(t => t.BillID)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(),
                        DeviceTypeID = c.Int(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                        Case = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeID, cascadeDelete: true)
                .Index(t => t.DeviceTypeID);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SingleHourPrice = c.Decimal(precision: 18, scale: 2),
                        SingleMinutePrice = c.Decimal(precision: 18, scale: 2),
                        MultiHourPrice = c.Decimal(precision: 18, scale: 2),
                        MultiMinutePrice = c.Decimal(precision: 18, scale: 2),
                        Birthday = c.Boolean(nullable: false),
                        BirthdayHourPrice = c.Decimal(precision: 18, scale: 2),
                        BirthdayMinutePrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClientMembershipMinutes",
                c => new
                    {
                        ClientID = c.Int(nullable: false),
                        DeviceTypeID = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientID, t.DeviceTypeID })
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.DeviceTypeID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(nullable: false),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClientMemberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        MembershipID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clients", t => t.ClientID, cascadeDelete: true)
                .ForeignKey("dbo.Memberships", t => t.MembershipID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.MembershipID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeviceTypeID = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeID, cascadeDelete: true)
                .Index(t => t.DeviceTypeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleID = c.Int(nullable: false),
                        IsWorked = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Safes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CanDelete = c.Boolean(nullable: false),
                        AmountType = c.Boolean(nullable: false),
                        Statement = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        SafeStart = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Income = c.Decimal(precision: 18, scale: 2),
                        Spending = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                        SafeEnd = c.Decimal(precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Spendings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Statement = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.BillItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Qty = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bills", t => t.BillID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.BillID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsAvailable = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.BillItems", "BillID", "dbo.Bills");
            DropForeignKey("dbo.Devices", "DeviceTypeID", "dbo.DeviceTypes");
            DropForeignKey("dbo.ClientMembershipMinutes", "DeviceTypeID", "dbo.DeviceTypes");
            DropForeignKey("dbo.Spendings", "UserID", "dbo.Users");
            DropForeignKey("dbo.Shifts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Safes", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.ClientMemberships", "UserID", "dbo.Users");
            DropForeignKey("dbo.Bills", "UserID", "dbo.Users");
            DropForeignKey("dbo.Memberships", "DeviceTypeID", "dbo.DeviceTypes");
            DropForeignKey("dbo.ClientMemberships", "MembershipID", "dbo.Memberships");
            DropForeignKey("dbo.ClientMemberships", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.ClientMembershipMinutes", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.Bills", "ClientID", "dbo.Clients");
            DropForeignKey("dbo.BillDevices", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.BillDevices", "BillID", "dbo.Bills");
            DropIndex("dbo.BillItems", new[] { "ItemID" });
            DropIndex("dbo.BillItems", new[] { "BillID" });
            DropIndex("dbo.Spendings", new[] { "UserID" });
            DropIndex("dbo.Shifts", new[] { "UserID" });
            DropIndex("dbo.Safes", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Memberships", new[] { "DeviceTypeID" });
            DropIndex("dbo.ClientMemberships", new[] { "ClientID" });
            DropIndex("dbo.ClientMemberships", new[] { "MembershipID" });
            DropIndex("dbo.ClientMemberships", new[] { "UserID" });
            DropIndex("dbo.ClientMembershipMinutes", new[] { "DeviceTypeID" });
            DropIndex("dbo.ClientMembershipMinutes", new[] { "ClientID" });
            DropIndex("dbo.Devices", new[] { "DeviceTypeID" });
            DropIndex("dbo.BillDevices", new[] { "DeviceID" });
            DropIndex("dbo.BillDevices", new[] { "BillID" });
            DropIndex("dbo.Bills", new[] { "ClientID" });
            DropIndex("dbo.Bills", new[] { "UserID" });
            DropTable("dbo.Items");
            DropTable("dbo.BillItems");
            DropTable("dbo.Spendings");
            DropTable("dbo.Shifts");
            DropTable("dbo.Safes");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Memberships");
            DropTable("dbo.ClientMemberships");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientMembershipMinutes");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.Devices");
            DropTable("dbo.BillDevices");
            DropTable("dbo.Bills");
        }
    }
}
