using BLL.RepositoryService;
using DAL.Entities;
using DTO.DeviceTypeDataModel;
using System.Collections.Generic;

namespace BLL.DeviceTypeService
{
    public interface IDeviceTypeRepository : IGenericRepository<DeviceType>
    {
        int GetRecordsNumber(string key);

        DeviceType GetByName(string name);

        DeviceType GetByIdName(int id, string name);

        DeviceType GetNotPriced();

        IEnumerable<DeviceType> GetAll();

        IEnumerable<DeviceTypeDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
