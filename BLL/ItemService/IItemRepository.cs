using BLL.RepositoryService;
using DAL.Entities;
using DTO.ItemDataModel;
using System.Collections.Generic;

namespace BLL.ItemService
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        int GetRecordsNumber(bool isNew, string key);

        List<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
