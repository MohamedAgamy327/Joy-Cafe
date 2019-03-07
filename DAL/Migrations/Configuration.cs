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
                IList<Role> defaultRoles = new List<Role>();

                defaultRoles.Add(new Role() { Name = RoleText.Admin });
                defaultRoles.Add(new Role() { Name = RoleText.Cashier });

                context.Roles.AddRange(defaultRoles);

                context.Users.Add(new User { Name = RoleText.Admin, Password = "123", RoleID = 1, IsWorked = true });

                IList<DeviceType> defaultDevicesTypes = new List<DeviceType>();

                defaultDevicesTypes.Add(new DeviceType() { Name = "PlayStation 3", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });
                defaultDevicesTypes.Add(new DeviceType() { Name = "PlayStation 4", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });
                defaultDevicesTypes.Add(new DeviceType() { Name = "VIP ROOM", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });
                defaultDevicesTypes.Add(new DeviceType() { Name = "PremiuM RooM", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });
                defaultDevicesTypes.Add(new DeviceType() { Name = "Royal RooM", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });
                defaultDevicesTypes.Add(new DeviceType() { Name = "VR ROOM", MultiHourPrice = 0, MultiMinutePrice = 0, SingleHourPrice = 0, SingleMinutePrice = 0 });

                context.DevicesTypes.AddRange(defaultDevicesTypes);
                base.Seed(context);
            }
        }
    }
}
