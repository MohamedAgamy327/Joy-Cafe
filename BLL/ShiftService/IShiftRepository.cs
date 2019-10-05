using BLL.RepositoryService;
using DAL.Entities;
using DTO.ShiftDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ShiftService
{
    public interface IShiftRepository : IGenericRepository<Shift>
    {
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalMinimum(DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalDevices(DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalItems(DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalDiscount(DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalSpending(DateTime dtFrom, DateTime dtTo);

        decimal? GetTotalIncome(DateTime dtFrom, DateTime dtTo);

        Shift GetActive();

        IEnumerable<Shift> Search(string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ShiftDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
