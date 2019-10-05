using BLL.RepositoryService;
using DAL.Entities;
using DTO.SafeDataModel;
using System;
using System.Collections.Generic;

namespace BLL.SafeService
{
    public interface ISafeRepository : IGenericRepository<Safe>
    {
        int GetRecordsNumber(string key);

        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);
     
        decimal? GetCurrentAccount();

        decimal GetTotalIncome(int userId, DateTime dtStart);

        decimal GetTotalSpendings(int userId, DateTime dtStart);

        decimal? GetTotalIncome(string key, DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalOutgoings(string key, DateTime dtFrom, DateTime dtTo);

        Safe GetByDateTime(DateTime registrationDate);

        IEnumerable<string> GetStatementSuggetions();

        IEnumerable<SafeDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        IEnumerable<SafeDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
