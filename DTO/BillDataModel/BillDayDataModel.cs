using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillDataModel
{
    public class BillDayDataModel : ValidatableBindableBase
    {
        private Bill _bill;
        public Bill Bill
        {
            get { return _bill; }
            set { SetProperty(ref _bill, value); }
        }

        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
    }
}
