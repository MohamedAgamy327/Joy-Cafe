using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillItemDataModel;
using DTO.ItemDataModel;
using System;

namespace BLL.BillItemService
{
    public class BillItemRepository : GenericRepository<BillItem>, IBillItemRepository
    {
        public BillItemRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID).Count();
        }

        public int GetRecordsNumber(int deviceId, DateTime dtFrom, DateTime dtTo)
        {
            if (deviceId != 0)
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.Bill.BillDevices.OrderByDescending(o => o.EndDate).FirstOrDefault().DeviceID == deviceId).GroupBy(p => p.ItemID).Count();
            }
            else
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo).GroupBy(p => p.ItemID).Count();
            }

        }

        public decimal GetBillItemsTotal()
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(f => f.Bill.Type == BillTypeText.Items && f.Bill.EndDate == null).Sum(s => s.Total) ?? 0;
        }

        public decimal? TotalAmount(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).Sum(s => s.Total);
        }

        public decimal? TotalQty(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).Sum(s => s.Qty);
        }

        public decimal? TotalQty(int deviceId, DateTime dtFrom, DateTime dtTo)
        {
            if (deviceId != 0)
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.Bill.BillDevices.OrderByDescending(o => o.EndDate).FirstOrDefault().DeviceID == deviceId).Sum(s => s.Qty);
            }
            else
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo).Sum(s => s.Qty);
            }
        }

        public decimal? TotalAmount(int deviceId, DateTime dtFrom, DateTime dtTo)
        {
            if (deviceId != 0)
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.Bill.BillDevices.OrderByDescending(o => o.EndDate).FirstOrDefault().DeviceID == deviceId).Sum(s => s.Total);
            }
            else
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo).Sum(s => s.Total);
            }
        }

        public IEnumerable<ShiftItemDisplayDataModel> GetShiftItems()
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate == null).OrderByDescending(o => o.RegistrationDate).Select(s => new ShiftItemDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }

        public IEnumerable<BillItemDisplayDataModel> GetBillItems(int billID)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.BillID == billID).OrderByDescending(o => o.RegistrationDate).Select(s => new BillItemDisplayDataModel
            {
                BillItem = s,
                Item = s.Item
            }).ToList();
        }

        public IEnumerable<ItemReportDataModel> Search(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID)
            .Select(s => new ItemReportDataModel
            {
                ID = s.FirstOrDefault().ItemID,
                Name = s.FirstOrDefault().Item.Name,
                Qty = s.Sum(k => k.Qty),
                Amount = s.Sum(c => c.Total),
            }).OrderByDescending(t => t.Qty).ToList();
        }

        public IEnumerable<ItemReportDataModel> Search(int deviceId, DateTime dtFrom, DateTime dtTo)
        {
            if (deviceId != 0)
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.Bill.BillDevices.OrderByDescending(o => o.EndDate).FirstOrDefault().DeviceID == deviceId).GroupBy(p => p.ItemID)
                 .Select(s => new ItemReportDataModel
                 {
                     ID = s.FirstOrDefault().ItemID,
                     Name = s.FirstOrDefault().Item.Name,
                     Qty = s.Sum(k => k.Qty),
                     Amount = s.Sum(c => c.Total),
                 }).OrderByDescending(t => t.Qty).ToList();
            }

            else
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo).GroupBy(p => p.ItemID)
                 .Select(s => new ItemReportDataModel
                 {
                     ID = s.FirstOrDefault().ItemID,
                     Name = s.FirstOrDefault().Item.Name,
                     Qty = s.Sum(k => k.Qty),
                     Amount = s.Sum(c => c.Total),
                 }).OrderByDescending(t => t.Qty).ToList();
            }
        }

        public IEnumerable<ItemReportDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && (w.Item.Name).Contains(key)).GroupBy(p => p.ItemID)
            .Select(s => new ItemReportDataModel
            {
                ID = s.FirstOrDefault().ItemID,
                Name = s.FirstOrDefault().Item.Name,
                Qty = s.Sum(k => k.Qty),
                Amount = s.Sum(c => c.Total),
            }).OrderByDescending(t => t.Qty).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }

        public IEnumerable<ItemReportDataModel> Search(int deviceId, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo)
        {
            if (deviceId != 0)
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.Bill.BillDevices.OrderByDescending(o => o.EndDate).FirstOrDefault().DeviceID == deviceId).GroupBy(p => p.ItemID)
                 .Select(s => new ItemReportDataModel
                 {
                     ID = s.FirstOrDefault().ItemID,
                     Name = s.FirstOrDefault().Item.Name,
                     Qty = s.Sum(k => k.Qty),
                     Amount = s.Sum(c => c.Total),
                 }).OrderByDescending(t => t.Qty).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }

            else
            {
                return GeneralDBContext.BillsItems.AsNoTracking().Where(w => w.Bill.Type == BillTypeText.Items && w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo).GroupBy(p => p.ItemID)
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
}
