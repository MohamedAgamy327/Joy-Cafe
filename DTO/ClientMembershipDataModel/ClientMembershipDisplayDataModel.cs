using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ClientMembershipDataModel
{
    public class ClientMembershipDisplayDataModel : ValidatableBindableBase
    {
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private ClientMembership _clientMembership;
        public ClientMembership ClientMembership
        {
            get { return _clientMembership; }
            set { SetProperty(ref _clientMembership, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        private Membership _membership;
        public Membership Membership
        {
            get { return _membership; }
            set { SetProperty(ref _membership, value); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
    }
}
