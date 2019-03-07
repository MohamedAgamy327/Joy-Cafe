using BLL.RepositoryService;
using DAL.Entities;
using DTO.ClientMembershipMinutesDataModel;
using System.Collections.Generic;

namespace BLL.ClientMembershipMinuteService
{
    public interface IClientMembershipMinuteRepository : IGenericRepository<ClientMembershipMinute>
    {
        int GetRecordsNumber(bool isNew, string key);

        List<ClientMembershipMinutesDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
