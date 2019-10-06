using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.DeviceDataModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace BLL.DeviceService
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Devices.AsNoTracking().Where(s => (s.Name + s.DeviceType.Name).Contains(key)).Count();
        }

        public Device GetById(int id)
        {
            return GeneralDBContext.Devices.Find(id);
        }

        public Device GetByBill(int billId)
        {
            return GeneralDBContext.Devices.AsNoTracking().SingleOrDefault(s => s.BillID == billId);
        }

        public Device GetByOrder(int order)
        {
            return GeneralDBContext.Devices.AsNoTracking().SingleOrDefault(s => s.Order == order);
        }

        public Device GetByIdOrder(int id, int order)
        {
            return GeneralDBContext.Devices.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Order == order);
        }

        public Device GetByNameDeviceType(string name, int deviceTypeId)
        {
            return GeneralDBContext.Devices.AsNoTracking().SingleOrDefault(s => s.Name == name && s.DeviceTypeID == deviceTypeId);
        }

        public Device GetByIdNameDeviceType(int id, string name, int deviceTypeId)
        {
            return GeneralDBContext.Devices.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Name == name && s.DeviceTypeID == deviceTypeId);
        }

        public IEnumerable<Device> GetAll()
        {
            return GeneralDBContext.Devices.AsNoTracking().OrderBy(o => o.Order).ToList();
        }

        public IEnumerable<DevicePlayDataModel> GetAvailable()
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

        public IEnumerable<DeviceDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Devices.AsNoTracking().Where(w => (w.Name + w.DeviceType.Name).Contains(key)).OrderBy(o => o.Order).ThenBy(o => o.DeviceType.Name).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new DeviceDisplayDataModel
            {
                Device = s,
                DeviceType = s.DeviceType,
                Status = s.IsAvailable == true ? GeneralText.Available : GeneralText.Unavailable,
                CanDelete = s.BillsDevices.Count > 0 ? false : true
            }).ToList();
        }

        public IEnumerable<DeviceFreeDataModel> GetFree(string gameType)
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
