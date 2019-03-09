using BLL.RepositoryService;
using DAL.Entities;
using DTO.SpendingDataModel;
using System;
using System.Collections.Generic;

namespace BLL.SpendingService
{
    public interface ISpendingRepository : IGenericRepository<Spending>
    {
        List<string> GetStatementSuggetions();
        decimal? GetTotalAmount(string key, DateTime dtFrom, DateTime dtTo);
        int GetRecordsNumber(string key);
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);
        List<SpendingDisplayDataModel> Search(string key, int userID);
        List<SpendingDisplayDataModel> Search(string key, int pageNumber, int pageSize);        
        List<SpendingDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
