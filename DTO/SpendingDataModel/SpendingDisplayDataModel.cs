using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.SpendingDataModel
{
    public class SpendingDisplayDataModel : ValidatableBindableBase
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private Spending _spending;
        public Spending Spending
        {
            get { return _spending; }
            set { SetProperty(ref _spending, value); }
        }
    }
}
