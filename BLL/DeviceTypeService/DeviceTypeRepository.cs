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

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public DeviceType GetNotPriced()
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().FirstOrDefault(f => f.SingleHourPrice == 0);
        }

        public DeviceType GetByName(string name)
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().SingleOrDefault(s => s.Name == name);
        }

        public DeviceType GetByIdName(int id, string name)
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Name == name);
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().Where(s => s.Name.Contains(key)).Count();
        }

        public IEnumerable<DeviceType> GetAll()
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().ToList();
        }

        public IEnumerable<DeviceTypeDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.DevicesTypes.AsNoTracking().Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceTypeDisplayDataModel
            {
                DeviceType = s,
                DevicesCount = s.Devices.Count,
                CanDelete = s.Devices.Count > 0 || s.Memberships.Count > 0 || s.ID == 1 || s.ID == 2 || s.ID == 3 || s.ID == 4 || s.ID == 5 || s.ID == 6 || s.ID == 7 ? false : true
            }).ToList();
        }
    }
}
