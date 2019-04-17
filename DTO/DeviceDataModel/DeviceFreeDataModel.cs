using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.DeviceDataModel
{
    public class DeviceFreeDataModel : ValidatableBindableBase
    {
        private string _deviceTypeImage;
        public string DeviceTypeImage
        {
            get { return _deviceTypeImage; }
            set { SetProperty(ref _deviceTypeImage, value); }
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
