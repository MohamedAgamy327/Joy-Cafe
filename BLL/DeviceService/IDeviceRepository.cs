using BLL.RepositoryService;
using DAL.Entities;
using DTO.DeviceDataModel;
using System.Collections.Generic;

namespace BLL.DeviceService
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        int GetRecordsNumber(string key);

        List<DeviceDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        List<DevicePlayDataModel> GetAvailable();

        List<DeviceFreeDataModel> GetFree(string gameType);
    }
}
