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
using DAL;

namespace BLL.UnitOfWorkService
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeneralDBContext _context;

        private IUserRepository users;
        private IBillRepository bills;
        private IBillDeviceRepository billsDevices;
        private IBillItemRepository billsItems;
        private IClientRepository clients;
        private IClientMembershipRepository clientsMemberships;
        private IClientMembershipMinuteRepository clientMembershipMinutes;
        private IDeviceRepository devices;
        private IDeviceTypeRepository devicesTypes;
        private IItemRepository items;
        private IMembershipRepository memberships;
        private IRoleRepository roles;
        private ISafeRepository safes;
        private IShiftRepository shifts;
        private ISpendingRepository spendings;


        public UnitOfWork(GeneralDBContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                if (users == null)
                {
                     users = new UserRepository(_context);
                }
                return users;
            }
        }

        public IBillRepository Bills
        {
            get
            {
                if (bills == null)
                {
                    bills = new BillRepository(_context);
                }
                return bills;
            }
        }

        public IBillDeviceRepository BillsDevices
        {
            get
            {
                if (billsDevices == null)
                {
                    billsDevices = new BillDeviceRepository(_context);
                }
                return billsDevices;
            }
        }

        public IBillItemRepository BillsItems
        {
            get
            {
                if (billsItems == null)
                {
                    billsItems = new BillItemRepository(_context);
                }
                return billsItems;
            }
        }

        public IClientRepository Clients
        {
            get
            {
                if (clients == null)
                {
                    clients = new ClientRepository(_context);
                }
                return clients;
            }
        }

        public IClientMembershipRepository ClientsMemberships
        {
            get
            {
                if (clientsMemberships == null)
                {
                    clientsMemberships = new ClientMembershipRepository(_context);
                }
                return clientsMemberships;
            }
        }

        public IClientMembershipMinuteRepository ClientMembershipMinutes
        {
            get
            {
                if (clientMembershipMinutes == null)
                {
                    clientMembershipMinutes = new ClientMembershipMinuteRepository(_context);
                }
                return clientMembershipMinutes;
            }
        }

        public IDeviceRepository Devices
        {
            get
            {
                if (devices == null)
                {
                    devices = new DeviceRepository(_context);
                }
                return devices;
            }
        }

        public IDeviceTypeRepository DevicesTypes
        {
            get
            {
                if (devicesTypes == null)
                {
                    devicesTypes = new DeviceTypeRepository(_context);
                }
                return devicesTypes;
            }
        }

        public IItemRepository Items
        {
            get
            {
                if (items == null)
                {
                    items = new ItemRepository(_context);
                }
                return items;
            }
        }

        public IMembershipRepository Memberships
        {
            get
            {
                if (memberships == null)
                {
                    memberships = new MembershipRepository(_context);
                }
                return memberships;
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = new RoleRepository(_context);
                }
                return roles;
            }
        }

        public ISafeRepository Safes
        {
            get
            {
                if (safes == null)
                {
                    safes = new SafeRepository(_context);
                }
                return safes;
            }
        }

        public IShiftRepository Shifts
        {
            get
            {
                if (shifts == null)
                {
                    shifts = new ShiftRepository(_context);
                }
                return shifts;
            }
        }

        public ISpendingRepository Spendings
        {
            get
            {
                if (spendings == null)
                {
                    spendings = new SpendingRepository(_context);
                }
                return spendings;
            }
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
