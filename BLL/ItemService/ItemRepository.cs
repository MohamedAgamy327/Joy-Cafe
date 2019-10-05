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

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.Date >= dtFrom && w.Bill.Date <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID).Count();
        }

        public decimal? TotalAmount(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.Date >= dtFrom && w.Bill.Date <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).Sum(s => s.Total);
        }

        public decimal? TotalQty(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.Date >= dtFrom && w.Bill.Date <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).Sum(s => s.Qty);
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

        public IEnumerable<ItemReportDataModel> Search(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.Date >= dtFrom && w.Bill.Date <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID)
            .Select(s => new ItemReportDataModel
            {
                ID = s.FirstOrDefault().ItemID,
                Name = s.FirstOrDefault().Item.Name,
                Qty = s.Sum(k => k.Qty),
                Amount = s.Sum(c => c.Total),
            }).OrderByDescending(t => t.Qty).ToList();

        }

        public IEnumerable<ItemReportDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.Date >= dtFrom && w.Bill.Date <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID)
            .Select(s => new ItemReportDataModel
            {
                ID = s.FirstOrDefault().ItemID,
                Name = s.FirstOrDefault().Item.Name,
                Qty = s.Sum(k => k.Qty),
                Amount = s.Sum(c => c.Total),
            }).OrderByDescending(t => t.Qty).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }

    }
}
