using BLL.RepositoryService;
using DAL.Entities;
using DTO.DeviceDataModel;
using System.Collections.Generic;

namespace BLL.DeviceService
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        int GetRecordsNumber(string key);

        Device GetById(int id);

        Device GetByBill(int billId);

        Device GetByOrder(int order);

        Device GetByIdOrder(int id,int order);

        Device GetByNameDeviceType(string name, int deviceTypeId);

        Device GetByIdNameDeviceType(int id,string name, int deviceTypeId);

        IEnumerable<DeviceDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        IEnumerable<DevicePlayDataModel> GetAvailable();

        IEnumerable<DeviceFreeDataModel> GetFree(string gameType);
    }
}
