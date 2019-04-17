using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.DeviceDataModel
{
    public  class DevicePlayDataModel : ValidatableBindableBase
    {
        private string _deviceTypeImage;
        public string DeviceTypeImage
        {
            get { return _deviceTypeImage; }
            set { SetProperty(ref _deviceTypeImage, value); }
        }

        private string _gameType;
        public string GameType
        {
            get { return _gameType; }
            set { SetProperty(ref _gameType, value); }
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
