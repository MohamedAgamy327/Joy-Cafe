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

        public int GetRecordsNumber( string key)
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
            return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DevicePlayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                Color = s.Case == CaseText.Busy ? "#CCD24726" :
                    s.Case == CaseText.Free ? "#E577B900" : "#000000",
                DeviceTypeIcon = s.DeviceType.Name == DeviceTypeText.PlayStation3 ? "MonitorPlay" :
                    s.DeviceType.Name == DeviceTypeText.PlayStation4 ? "SocialPlaystation" :
                    s.DeviceType.Name == DeviceTypeText.VIP ? "Vip" :
                    s.DeviceType.Name == DeviceTypeText.Premium ? "AdobePremierpro" :
                    s.DeviceType.Name == DeviceTypeText.Royal ? "SocialSpotify" :
                    s.DeviceType.Name == DeviceTypeText.VR ? "SmileyGlasses" : "Xbox",
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
                    DeviceTypeIcon = s.DeviceType.Name == DeviceTypeText.PlayStation3 ? "MonitorPlay" :
                          s.DeviceType.Name == DeviceTypeText.PlayStation4 ? "SocialPlaystation" :
                          s.DeviceType.Name == DeviceTypeText.VIP ? "Vip" :
                          s.DeviceType.Name == DeviceTypeText.Premium ? "AdobePremierpro" :
                          s.DeviceType.Name == DeviceTypeText.Royal ? "SocialSpotify" :
                          s.DeviceType.Name == DeviceTypeText.VR ? "SmileyGlasses" : "Xbox"
                }).ToList();
            else
                return GeneralDBContext.Devices.AsNoTracking().Where(w => w.IsAvailable == true && w.Case == CaseText.Free).OrderBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Select(s => new DeviceFreeDataModel
                {
                    Device = s,
                    DeviceType = s.DeviceType,
                    DeviceTypeIcon = s.DeviceType.Name == DeviceTypeText.PlayStation3 ? "MonitorPlay" :
                          s.DeviceType.Name == DeviceTypeText.PlayStation4 ? "SocialPlaystation" :
                          s.DeviceType.Name == DeviceTypeText.VIP ? "Vip" :
                          s.DeviceType.Name == DeviceTypeText.Premium ? "AdobePremierpro" :
                          s.DeviceType.Name == DeviceTypeText.Royal ? "SocialSpotify" :
                          s.DeviceType.Name == DeviceTypeText.VR ? "SmileyGlasses" : "Xbox"
                }).ToList();
        }
    }
}
