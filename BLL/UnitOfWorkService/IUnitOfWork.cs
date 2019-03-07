using BLL.BillDeviceService;
using BLL.BillItemService;
using BLL.BillService;
using BLL.ClientMembershipMinuteService;
using BLL.ClientMembershipService;
using BLL.ClientService;
using BLL.DeviceService;
using BLL.DeviceTypeService;
using BLL.ItemService;
using BLL.MembershipService;
using BLL.RoleService;
using BLL.SafeService;
using BLL.ShiftService;
using BLL.SpendingService;
using BLL.UserService;
using System;

namespace BLL.UnitOfWorkService
{
    public interface IUnitOfWork : IDisposable
    {
        IBillRepository Bills { get;  }
        IBillDeviceRepository BillsDevices { get; }
        IBillItemRepository BillsItems { get; }
        IClientRepository Clients { get; }
        IClientMembershipRepository ClientsMemberships { get; }
        IClientMembershipMinuteRepository ClientMembershipMinutes { get; }
        IDeviceRepository Devices { get; }
        IDeviceTypeRepository DevicesTypes { get; }
        IItemRepository Items { get; }
        IMembershipRepository Memberships { get; }
        IRoleRepository Roles { get; }
        ISafeRepository Safes { get; }
        IShiftRepository Shifts { get; }
        ISpendingRepository Spendings { get; }
        IUserRepository Users { get; }

        int Complete();
    }
}
