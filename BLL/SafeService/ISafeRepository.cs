using BLL.RepositoryService;
using DAL.Entities;
using DTO.SafeDataModel;
using System;
using System.Collections.Generic;

namespace BLL.SafeService
{
    public interface ISafeRepository : IGenericRepository<Safe>
    {
        decimal? GetCurrentAccount();
        decimal? GetTotalIncome(string key, DateTime dtFrom, DateTime dtTo);
        decimal? GetTotalOutgoings(string key, DateTime dtFrom, DateTime dtTo);
        List<string> GetStatementSuggetions();
        int GetRecordsNumber(string key);
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);
        List<SafeDisplayDataModel> Search(string key, int pageNumber, int pageSize);
        List<SafeDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
