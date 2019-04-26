using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BillService
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public Bill GetLastBill(int deviceId)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().Where(w => w.Bill.EndDate != null && w.DeviceID == deviceId).OrderByDescending(o => o.EndDate).FirstOrDefault()?.Bill;
        }

        public int GetRecordsNumber(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            switch (billCase)
            {
                case BillCaseText.All:
                    return GeneralDBContext.Bills.Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                case BillCaseText.Available:
                    return GeneralDBContext.Bills.Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                case BillCaseText.Canceled:
                    return GeneralDBContext.Bills.Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                case BillCaseText.Deleted:
                    return GeneralDBContext.Bills.Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                default:
                    return 0;
            }
        }
        public List<BillDayDataModel> Search(DateTime date)
        {
            return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Date == date && w.Type == BillTypeText.Devices).OrderBy(o => o.ID).Select(s => new BillDayDataModel
            {
                Bill = s,
                User = s.User,
                Client = s.Client
            }).ToList();
        }

        public List<BillDisplayDataModel> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            switch (billCase)
            {
                case BillCaseText.All:
                    return GeneralDBContext.Bills.Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                    {
                        Bill = s,
                        User = s.User,
                        Client = s.Client,
                        Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                    }).ToList();
                case BillCaseText.Available:
                    return GeneralDBContext.Bills.Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                    {
                        Bill = s,
                        User = s.User,
                        Client = s.Client,
                        Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                    }).ToList();
                case BillCaseText.Canceled:
                    return GeneralDBContext.Bills.Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                    {
                        Bill = s,
                        User = s.User,
                        Client = s.Client,
                        Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                    }).ToList();
                case BillCaseText.Deleted:
                    return GeneralDBContext.Bills.Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                    {
                        Bill = s,
                        User = s.User,
                        Client = s.Client,
                        Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                    }).ToList();
                default:
                    return null;

            }

        }
    }
}
