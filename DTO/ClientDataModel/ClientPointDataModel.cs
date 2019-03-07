using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ClientDataModel
{
    public class ClientPointDataModel : ValidatableBindableBase
    {
        private int? _points;
        public int? Points
        {
            get { return _points; }
            set { SetProperty(ref _points, value); }
        }

        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
    }
}
