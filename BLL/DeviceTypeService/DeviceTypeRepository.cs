using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.DeviceTypeDataModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BLL.DeviceTypeService
{
    public class DeviceTypeRepository : GenericRepository<DeviceType>, IDeviceTypeRepository
    {
        static List<DeviceType> deviceTypes { get; set; }

        public DeviceTypeRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(bool isNew, string key)
        {
            if (isNew)
                deviceTypes = GetAll().ToList();
            return deviceTypes.Where(s => s.Name.Contains(key)).Count();
        }

        public List<DeviceTypeDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return deviceTypes.Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceTypeDisplayDataModel
            {
                DeviceType = s,
                DevicesCount = s.Devices.Count,
                CanDelete = s.Devices.Count > 0 || s.Memberships.Count > 0 || s.ID == 1 || s.ID == 2 || s.ID == 3 || s.ID == 4 || s.ID == 5 || s.ID == 6 ? false : true
            }).ToList();
        }
    }
}
