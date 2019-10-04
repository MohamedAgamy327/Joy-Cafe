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

        decimal? DevicesSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? ItemsSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? DiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        decimal? TotalAfterDiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        Bill GetLastBill(int deviceId);

        List<BillDayDataModel> Search(DateTime date);

        List<Bill> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo);

        List<BillDisplayDataModel> Search(string billCase,string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
