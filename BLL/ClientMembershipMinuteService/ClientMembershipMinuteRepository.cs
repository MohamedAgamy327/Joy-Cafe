using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.ClientMembershipMinutesDataModel;

namespace BLL.ClientMembershipMinuteService
{
    public class ClientMembershipMinuteRepository : GenericRepository<ClientMembershipMinute>, IClientMembershipMinuteRepository
    {
        public ClientMembershipMinuteRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.ClientMembershipMinutes.Where(w => (w.Client.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Client.Name).Count();
        }

        public List<ClientMembershipMinutesDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.ClientMembershipMinutes.Where(w => (w.Client.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Client.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .Select(s => new ClientMembershipMinutesDisplayDataModel
                {
                    Client = s.Client,
                    ClientMembershipMinute = s,
                    DeviceType = s.DeviceType
                })
                .ToList();
        }
    }
}
