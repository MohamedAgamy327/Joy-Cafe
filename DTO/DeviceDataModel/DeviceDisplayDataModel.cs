using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.DeviceDataModel
{
    public class DeviceDisplayDataModel : ValidatableBindableBase
    {
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
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
