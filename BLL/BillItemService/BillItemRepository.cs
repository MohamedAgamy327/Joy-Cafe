using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.RepositoryService;
using DAL;
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

        public List<BillItemDisplayDataModel> GetBillItems(int billID)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.BillID == billID).Select(s => new BillItemDisplayDataModel
            {
                BillItem=s,
                Item=s.Item
            }).ToList();
        }
    }
}
