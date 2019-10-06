using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.ItemDataModel;
using System.Data.Entity;
using DAL.ConstString;
using System;

namespace BLL.ItemService
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Items.AsNoTracking().Where(s => s.Name.Contains(key)).Count();
        }

        public Item GetByName(string name)
        {
            return GeneralDBContext.Items.AsNoTracking().SingleOrDefault(s => s.Name == name);
        }

        public Item GetByIdName(int id, string name)
        {
            return GeneralDBContext.Items.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Name == name);
        }

        public IEnumerable<Item> Search()
        {
            return GeneralDBContext.Items.AsNoTracking().Where(f => f.IsAvailable == true).OrderByDescending(o => o.BillsItems.Count).ThenBy(o => o.Name).ToList();
        }

        public IEnumerable<ItemDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Items.AsNoTracking().Where(w => (w.Name).Contains(key)).OrderBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new ItemDisplayDataModel
            {
                Item = s,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.BillsItems.Count > 0 ? false : true
            }).ToList(); ;
        }

    }
}
