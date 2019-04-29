using System.Collections.Generic;
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

        public List<ShiftItemDisplayDataModel> GetShiftItems()
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate == null).OrderByDescending(o=>o.RegistrationDate).Select(s => new ShiftItemDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }

        public List<BillItemDisplayDataModel> GetBillItems(int billID)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.BillID == billID).OrderByDescending(o => o.RegistrationDate).Select(s => new BillItemDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }
    }
}
