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

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public BillDevice GetLast(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().OrderByDescending(o => o.EndDate).FirstOrDefault(f => f.BillID == billID);
        }

        List<BillDevicesDisplayDataModel> IBillDeviceRepository.GetBillDevices(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().Where(w => w.BillID == billID).OrderByDescending(o => o.StartDate).Select(s => new BillDevicesDisplayDataModel
            {
                BillDevice = s,
                Device = s.Device,
                DeviceType = s.Device.DeviceType,
                EndDate = s.EndDate.HasValue ? s.EndDate : DateTime.Now
            }).ToList();
        }
    }
}
