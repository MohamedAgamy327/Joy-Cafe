using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillService
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        int GetRecordsNumber(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal GetTotalMinimum(int userId, DateTime dtStart);

        decimal GetTotalDevices(int userId, DateTime dtStart);

        decimal GetTotalItems(int userId, DateTime dtStart);

        decimal GetTotalDiscount(int userId, DateTime dtStart);

        decimal? DevicesSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? ItemsSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? DiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? TotalAfterDiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        Bill GetItemsBill();

        Bill GetById(int id);

        Bill GetLastBill(int deviceId);

        IEnumerable<BillDayDataModel> Search(DateTime date);

        IEnumerable<Bill> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<BillDisplayDataModel> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
