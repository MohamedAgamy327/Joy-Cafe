using BLL.RepositoryService;
using DAL.Entities;
using DTO.MembershipDataModel;
using System.Collections.Generic;

namespace BLL.MembershipService
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        int GetRecordsNumber(bool isNew, string key);

        List<MembershipDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
