using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.DeviceTypeDataModel
{
    public class DeviceTypeDisplayDataModel : ValidatableBindableBase
    {
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private int _devicesCount;
        public int DevicesCount
        {
            get { return _devicesCount; }
            set { SetProperty(ref _devicesCount, value); }
        }
    }
}
