using BLL.RepositoryService;
using DAL.Entities;
using DTO.MembershipDataModel;
using System.Collections.Generic;

namespace BLL.MembershipService
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        int GetRecordsNumber(string key);

        Membership GetByNameDeviceType(string name, int deviceTypeId);

        Membership GetByIdNameDeviceType(int id, string name, int deviceTypeId);

        IEnumerable<Membership> Search(int deviceTypeId);

        IEnumerable<MembershipDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
