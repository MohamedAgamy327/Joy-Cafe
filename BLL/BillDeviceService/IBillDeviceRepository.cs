using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using System.Collections.Generic;

namespace BLL.BillDeviceService
{
    public interface IBillDeviceRepository : IGenericRepository<BillDevice>
    {
        BillDevice GetLast(int billID);
        List<BillDevicesDisplayDataModel> GetBillDevices(int billID);
    }
}
