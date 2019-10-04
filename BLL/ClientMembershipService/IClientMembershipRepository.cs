using BLL.RepositoryService;
using DAL.Entities;
using DTO.ClientMembershipDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ClientMembershipService
{
    public interface IClientMembershipRepository : IGenericRepository<ClientMembership>
    {
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);

        List<ClientMembershipDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
