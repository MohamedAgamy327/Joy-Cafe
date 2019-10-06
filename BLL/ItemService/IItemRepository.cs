using BLL.RepositoryService;
using DAL.Entities;
using DTO.ItemDataModel;
using System.Collections.Generic;

namespace BLL.ItemService
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        int GetRecordsNumber(string key);

        Item GetByName(string name);

        Item GetByIdName(int id, string name);

        IEnumerable<Item> Search();

        IEnumerable<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize);

    }
}
