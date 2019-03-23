using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.ItemDataModel;
using System.Data.Entity;
using DAL.ConstString;

namespace BLL.ItemService
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Items.Where(s => s.Name.Contains(key)).Count();
        }

        public List<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Items.Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new ItemDisplayDataModel
            {
                Item = s,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.BillsItems.Count > 0 ? false : true
            }).ToList(); ;
        }
    }
}
