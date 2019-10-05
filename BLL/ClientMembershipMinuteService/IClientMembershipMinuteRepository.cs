using BLL.RepositoryService;
using DAL.Entities;
using DTO.ClientMembershipMinutesDataModel;
using System.Collections.Generic;

namespace BLL.ClientMembershipMinuteService
{
    public interface IClientMembershipMinuteRepository : IGenericRepository<ClientMembershipMinute>
    {
        int GetRecordsNumber(string key);

        ClientMembershipMinute GetByDeviceTypeClient(int deviceTypeID, int clientId);

        IEnumerable<ClientMembershipMinutesDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
