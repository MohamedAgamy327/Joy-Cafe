using DAL.BindableBaseService;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DeviceDataModel
{
    public class DeviceFreeDataModel : ValidatableBindableBase
    {
        private string _deviceTypeIcon;
        public string DeviceTypeIcon
        {
            get { return _deviceTypeIcon; }
            set { SetProperty(ref _deviceTypeIcon, value); }
        }

        private Device _device;
        public Device Device
        {
            get { return _device; }
            set { SetProperty(ref _device, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }
    }
}
