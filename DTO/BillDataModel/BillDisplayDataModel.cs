using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillDataModel
{
    public class BillDisplayDataModel : ValidatableBindableBase
    {
        private string _case;
        public string Case
        {
            get { return _case; }
            set { SetProperty(ref _case, value); }
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

        private Bill _bill;
        public Bill Bill
        {
            get { return _bill; }
            set { SetProperty(ref _bill, value); }
        }
    }
}
