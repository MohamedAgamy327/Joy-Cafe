using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillItemDataModel;
using DTO.ItemDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillItemService
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);

        int GetRecordsNumber(int deviceId, DateTime dtFrom, DateTime dtTo);

        decimal GetBillItemsTotal();

        decimal? TotalQty(int deviceId, DateTime dtFrom, DateTime dtTo);

        decimal? TotalAmount(int deviceId, DateTime dtFrom, DateTime dtTo);

        decimal? TotalQty(string key, DateTime dtFrom, DateTime dtTo);

        decimal? TotalAmount(string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ShiftItemDisplayDataModel> GetShiftItems();

        IEnumerable<BillItemDisplayDataModel> GetBillItems(int billID);

        IEnumerable<ItemReportDataModel> Search(string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ItemReportDataModel> Search(int deviceId, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ItemReportDataModel> Search(int deviceId, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ItemReportDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);
    }
}
