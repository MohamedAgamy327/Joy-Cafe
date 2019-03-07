using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.MembershipDataModel
{
    public class MembershipDisplayDataModel : ValidatableBindableBase
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

        private Membership _membership;
        public Membership Membership
        {
            get { return _membership; }
            set { SetProperty(ref _membership, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

    }
}
