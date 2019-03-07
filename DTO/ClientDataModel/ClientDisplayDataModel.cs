using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ClientDataModel
{
    public class ClientDisplayDataModel : ValidatableBindableBase
    {
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
    }
}
