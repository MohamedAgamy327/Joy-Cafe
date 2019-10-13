using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillDeviceService
{
    public interface IBillDeviceRepository : IGenericRepository<BillDevice>
    {
        BillDevice GetByBill(int billID);

        BillDevice GetLastBill(int billID);

        IEnumerable<DeviceReportDataModel> Search(int deviceId, DateTime dtFrom, DateTime dtTo);

        IEnumerable<BillDeviceDisplayDataModel> GetBillDevices(int billID);
    }
}
