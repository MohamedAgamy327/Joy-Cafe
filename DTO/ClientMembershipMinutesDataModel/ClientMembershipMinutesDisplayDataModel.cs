using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ClientMembershipMinutesDataModel
{
    public class ClientMembershipMinutesDisplayDataModel : ValidatableBindableBase
    {
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private ClientMembershipMinute _clientMembershipMinute;
        public ClientMembershipMinute ClientMembershipMinute
        {
            get { return _clientMembershipMinute; }
            set { SetProperty(ref _clientMembershipMinute, value); }
        }

    }
}
