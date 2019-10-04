namespace DAL.Migrations
{
    using DAL.ConstString;
    using DAL.Entities;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GeneralDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GeneralDBContext context)
        {
            if (context.Users.FirstOrDefault() == null)
            {
                context.Roles.AddRange(new List<Role> {
                    new Role() { Name = RoleText.Admin },
                    new Role() { Name = RoleText.Cashier }
                });

                context.Users.AddRange(new List<User>{
                  new User { Name = RoleText.Admin, Password = "123", RoleID = 1, IsWorked = true },
                  new User { Name = $"{RoleText.Cashier} 1", Password = "123", RoleID = 2, IsWorked = true },
                  new User { Name = $"{RoleText.Cashier} 2", Password = "123", RoleID = 2, IsWorked = true }
                });

                context.DevicesTypes.AddRange(new List<DeviceType> {
                    new DeviceType() { Name = DeviceTypeText.PS3, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.PS4, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.PS44K, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.VIP, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.Premium, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.Royal, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 },
                    new DeviceType() { Name = DeviceTypeText.VR, MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 }
                });
                
                base.Seed(context);
            }
            if (context.Roles.Count() < 3)
            {
                context.Roles.Add(new Role
                {
                    Name = RoleText.Tax
                });
                base.Seed(context);
            }
        }
    }
}
