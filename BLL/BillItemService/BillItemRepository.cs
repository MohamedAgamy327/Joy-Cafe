using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillItemDataModel;

namespace BLL.BillItemService
{
    public class BillItemRepository : GenericRepository<BillItem>, IBillItemRepository
    {
        public BillItemRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public List<BillItemsDisplayDataModel> GetBillItems()
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == GeneralText.Items && w.Bill.EndDate == null).OrderByDescending(o=>o.RegistrationDate).Select(s => new BillItemsDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }

        public List<BillItemsDisplayDataModel> GetBillItems(int billID)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.BillID == billID).OrderByDescending(o => o.RegistrationDate).Select(s => new BillItemsDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }
    }
}
