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

        public DeviceTypeRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.DevicesTypes.Where(s => s.Name.Contains(key)).Count();
        }

        public List<DeviceTypeDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.DevicesTypes.Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceTypeDisplayDataModel
            {
                DeviceType = s,
                DevicesCount = s.Devices.Count,
                CanDelete = s.Devices.Count > 0 || s.Memberships.Count > 0 || s.ID == 1 || s.ID == 2 || s.ID == 3 || s.ID == 4 || s.ID == 5 || s.ID == 6 || s.ID == 7 ? false : true
            }).ToList();
        }
    }
}
