using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillItemDataModel;
using System.Collections.Generic;

namespace BLL.BillItemService
{
    public interface IBillItemRepository : IGenericRepository<BillItem>
    {
        decimal GetBillItemsTotal();

        IEnumerable<ShiftItemDisplayDataModel> GetShiftItems();

        IEnumerable<BillItemDisplayDataModel> GetBillItems(int billID);
    }
}
