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

        Spending GetByDateTime(DateTime registrationDate);

        IEnumerable<string> GetStatementSuggetions();

        IEnumerable<SpendingDisplayDataModel> Search(string key, int userID);

        IEnumerable<Spending> Search(string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<SpendingDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        IEnumerable<SpendingDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
