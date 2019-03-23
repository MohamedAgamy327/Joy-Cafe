using BLL.RepositoryService;
using DAL.Entities;
using DTO.DeviceTypeDataModel;
using System.Collections.Generic;

namespace BLL.DeviceTypeService
{
    public interface IDeviceTypeRepository : IGenericRepository<DeviceType>
    {
        int GetRecordsNumber(string key);
        List<DeviceTypeDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
