using BLL.RepositoryService;
using DAL.Entities;
using DTO.SpendingDataModel;
using System;
using System.Collections.Generic;

namespace BLL.SpendingService
{
    public interface ISpendingRepository : IGenericRepository<Spending>
    {
        int GetRecordsNumber(string key);

        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalAmount(string key, DateTime dtFrom, DateTime dtTo);

        List<string> GetStatementSuggetions();

        List<SpendingDisplayDataModel> Search(string key, int userID);

        List<Spending> Search(string key, DateTime dtFrom, DateTime dtTo);

        List<SpendingDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        List<SpendingDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
