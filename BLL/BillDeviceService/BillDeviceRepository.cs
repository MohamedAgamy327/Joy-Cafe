using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BillDeviceService
{
    public class BillDeviceRepository : GenericRepository<BillDevice>, IBillDeviceRepository
    {
        public BillDeviceRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public BillDevice GetLastBill(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().OrderByDescending(o => o.EndDate).FirstOrDefault(f => f.BillID == billID);
        }

        public BillDevice GetByBill(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().SingleOrDefault(f => f.BillID == billID && f.EndDate == null);
        }

        public IEnumerable<BillDeviceDisplayDataModel> GetBillDevices(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().Where(w => w.BillID == billID).OrderByDescending(o => o.StartDate).Select(s => new BillDeviceDisplayDataModel
            {
                BillDevice = s,
                Device = s.Device,
                DeviceType = s.Device.DeviceType,
                EndDate = s.EndDate.HasValue ? s.EndDate : DateTime.Now
            }).ToList();
        }

        public IEnumerable<DeviceReportDataModel> Search(int deviceId, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().Where(w => w.Bill.EndDate != null && w.Bill.EndDate >= dtFrom && w.Bill.EndDate <= dtTo && w.Bill.Canceled == false && w.DeviceID == deviceId).GroupBy(p => p.GameType).ToList()
                .Select(s => new DeviceReportDataModel
                {
                    DeviceType = s.FirstOrDefault().Device.DeviceType,
                    Type = s.FirstOrDefault().GameType,
                    Hours = ((int)s.Sum(k => k.Duration) / 60).ToString("D2") + ":" + ((int)s.Sum(k => k.Duration) % 60).ToString("D2"),
                    Amount = s.Sum(c => c.Total),
                    Minutes=s.Sum(c=>c.Duration)
                }).ToList();
        }
    }
}
