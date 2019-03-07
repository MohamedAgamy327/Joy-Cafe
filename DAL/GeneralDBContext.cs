using DAL.Entities;
using DAL.Migrations;
using System.Data.Entity;

namespace DAL
{
    public class GeneralDBContext : DbContext
    {
        public GeneralDBContext()
          : base("name=GeneralDBContext")
        {
            //Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GeneralDBContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
      
        public DbSet<Safe> Safes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<BillItem> BillsItems { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<BillDevice> BillsDevices { get; set; }
        public DbSet<DeviceType> DevicesTypes { get; set; }      
        public DbSet<ClientMembership> ClientsMemberships { get; set; }
        public DbSet<ClientMembershipMinute> ClientMembershipMinutes { get; set; }

    }
}
