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
        static List<Device> devices { get; set; }

        public DeviceRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(bool isNew, string key)
        {
            if (isNew)
                devices = GetAll().ToList();
            return devices.Where(s => (s.Name + s.DeviceType.Name).Contains(key)).Count();
        }

        public List<DeviceDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return devices.Where(w => (w.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceDisplayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.BillsDevices.Count > 0 ? false : true
            }).ToList();
        }

        public List<DevicePlayDataModel> GetAvailable()
        {
            return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DevicePlayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                Color = s.Case == CaseText.Busy ? "#CCD24726" :
                    s.Case == CaseText.Free ? "#E577B900" : "#000000",
                DeviceTypeIcon = s.DeviceType.Name == "PlayStation 3" ? "MonitorPlay" :
                    s.DeviceType.Name == "PlayStation 4" ? "SocialPlaystation" :
                    s.DeviceType.Name == "VIP ROOM" ? "Vip" :
                    s.DeviceType.Name == "PremiuM RooM" ? "AdobePremierpro" :
                    s.DeviceType.Name == "Royal RooM" ? "SocialSpotify" :
                    s.DeviceType.Name == "VR ROOM" ? "SmileyGlasses" : "Xbox",
                GameType = s.Case != CaseText.Free ? s.BillsDevices.OrderByDescending(o => o.StartDate).FirstOrDefault().GameType : ""
            }).ToList();
        }

        public List<DeviceFreeDataModel> GetFree(string gameType)
        {
            if (gameType == GamePlayTypeText.Birthday)
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == CaseText.Free && w.DeviceType.Birthday == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeIcon = s.DeviceType.Name == "PlayStation 3" ? "MonitorPlay" :
                          s.DeviceType.Name == "PlayStation 4" ? "SocialPlaystation" :
                          s.DeviceType.Name == "VIP ROOM" ? "Vip" :
                          s.DeviceType.Name == "PremiuM RooM" ? "AdobePremierpro" :
                          s.DeviceType.Name == "Royal RooM" ? "SocialSpotify" :
                          s.DeviceType.Name == "VR ROOM" ? "SmileyGlasses" : "Xbox"
                }).ToList();
            else
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == CaseText.Free).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeIcon = s.DeviceType.Name == "PlayStation 3" ? "MonitorPlay" :
                   s.DeviceType.Name == "PlayStation 4" ? "SocialPlaystation" :
                   s.DeviceType.Name == "VIP ROOM" ? "Vip" :
                   s.DeviceType.Name == "PremiuM RooM" ? "AdobePremierpro" :
                   s.DeviceType.Name == "Royal RooM" ? "SocialSpotify" :
                   s.DeviceType.Name == "VR ROOM" ? "SmileyGlasses" : "Xbox"
                }).ToList();
        }
    }
}
