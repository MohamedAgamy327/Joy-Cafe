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
            return GeneralDBContext.Devices.Where(w => (w.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceDisplayDataModel
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
            return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DevicePlayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                DeviceTypeImage = s.Case == CaseText.Free ? (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(CaseText.Free)))) :
                s.Case == CaseText.Paused ? (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(CaseText.Paused) && m.Contains(s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType)))) :
                (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType)))),
                GameType = s.Case != CaseText.Free ? s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType : ""
            }).ToList();
        }

        public List<DeviceFreeDataModel> GetFree(string gameType)
        {
            string url = @"../../Resources/Icons/";
            if (gameType == GamePlayTypeText.Birthday)
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == CaseText.Free && w.DeviceType.Birthday == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeImage = (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(CaseText.Free))))
                }).ToList();
            else
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == CaseText.Free).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeImage = (url + (DevicesImagesList.ImageList.FirstOrDefault(m => m.Contains(s.DeviceType.Name) && m.Contains(CaseText.Free))))
                }).ToList();
        }

    }
}
