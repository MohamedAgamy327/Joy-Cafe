using BLL.RepositoryService;
using DAL.Entities;
using DTO.ItemDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ItemService
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        int GetRecordsNumber(string key);

        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);

        decimal? TotalQty(string key, DateTime dtFrom, DateTime dtTo);

        decimal? TotalAmount(string key, DateTime dtFrom, DateTime dtTo);

        List<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        List<ItemReportDataModel> Search(string key, DateTime dtFrom, DateTime dtTo);

        List<ItemReportDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);
    }
}
