using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.MembershipDataModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.MembershipService
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Memberships.AsNoTracking().Where(s => s.Name.Contains(key)).Count();
        }

        public Membership GetByNameDeviceType(string name, int deviceTypeId)
        {
            return GeneralDBContext.Memberships.AsNoTracking().SingleOrDefault(s => s.Name == name && s.DeviceTypeID == deviceTypeId);
        }

        public Membership GetByIdNameDeviceType(int id, string name, int deviceTypeId)
        {
            return GeneralDBContext.Memberships.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Name == name && s.DeviceTypeID == deviceTypeId);
        }

        public IEnumerable<Membership> Search(int deviceTypeId)
        {
            return GeneralDBContext.Memberships.AsNoTracking().Where(f => f.DeviceTypeID == deviceTypeId && f.IsAvailable == true).ToList();
        }

        public IEnumerable<MembershipDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Memberships.AsNoTracking().Where(w => (w.Name).Contains(key)).OrderBy(o => o.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new MembershipDisplayDataModel
            {
                Membership = s,
                DeviceType = s.DeviceType,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.ClientsMemberships.Count > 0 ? false : true
            }).ToList();
        }

    }
}
