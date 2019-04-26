using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.DeviceDataModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.DeviceService
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Devices.Where(s => (s.Name + s.DeviceType.Name).Contains(key)).Count();
        }

        public List<DeviceDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Devices.Where(w => (w.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Order).ThenBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceDisplayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.BillsDevices.Count > 0 ? false : true
            }).ToList();
        }

        public List<DevicePlayDataModel> GetAvailable()
        {
            string url = @"../../Resources/Icons/";
            return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true).OrderBy(o => o.Order).ThenBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DevicePlayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                DeviceTypeImage = s.Case == DeviceCaseText.Free ? (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(DeviceCaseText.Free)) ?? OtherDeviceTypeText.OtherFree)) :
                s.Case == DeviceCaseText.Paused ? (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(DeviceCaseText.Paused)) ?? OtherDeviceTypeText.OtherPaused))
                : (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType)) ?? OtherDeviceTypeText.Other + s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType + ".PNG")
                ),
                GameType = s.Case != DeviceCaseText.Free ? s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType : ""
            }).ToList();

        }

        public List<DeviceFreeDataModel> GetFree(string gameType)
        {
            string url = @"../../Resources/Icons/";
            if (gameType == GamePlayTypeText.Birthday)
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == DeviceCaseText.Free && w.DeviceType.Birthday == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeImage = (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(DeviceCaseText.Free)) ?? OtherDeviceTypeText.OtherFree))
                }).ToList();
            else
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == DeviceCaseText.Free).OrderBy(o => o.Order).ThenBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeImage = (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(DeviceCaseText.Free)) ?? OtherDeviceTypeText.OtherFree))
                }).ToList();
        }

    }
}
