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
        static List<Membership> memberships { get; set; }

        public MembershipRepository(GeneralDBContext context)
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
                memberships = GetAll().ToList();
            return memberships.Where(s => s.Name.Contains(key)).Count();
        }

        public List<MembershipDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return memberships.Where(w => (w.Name).Contains(key)).OrderBy(o => o.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new MembershipDisplayDataModel
            {
                Membership = s,
                DeviceType = s.DeviceType,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.ClientsMemberships.Count > 0 ? false : true
            }).ToList();
        }
    }
}
