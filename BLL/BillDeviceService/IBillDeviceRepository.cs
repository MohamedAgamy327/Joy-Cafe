using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using System.Collections.Generic;

namespace BLL.BillDeviceService
{
    public interface IBillDeviceRepository : IGenericRepository<BillDevice>
    {
        BillDevice GetByBill(int billID);

        BillDevice GetLastBill(int billID);

        IEnumerable<BillDeviceDisplayDataModel> GetBillDevices(int billID);
    }
}
