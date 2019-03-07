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
        static List<ClientMembershipMinute> clientMembershipMinutes { get; set; }

        public ClientMembershipMinuteRepository(GeneralDBContext context)
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
                clientMembershipMinutes = GetAll().ToList();
            return clientMembershipMinutes.Where(w => (w.Client.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Client.Name).Count();
        }

        public List<ClientMembershipMinutesDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return clientMembershipMinutes.Where(w => (w.Client.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Client.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize)
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
