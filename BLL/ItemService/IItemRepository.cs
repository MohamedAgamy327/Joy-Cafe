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

        Item GetByName(string name);

        Item GetByIdName(int id, string name);

        IEnumerable<Item> Search();

        IEnumerable<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        IEnumerable<ItemReportDataModel> Search(string key, DateTime dtFrom, DateTime dtTo);

        IEnumerable<ItemReportDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);
    }
}
