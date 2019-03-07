using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ShiftDataModel
{
    public class ShiftDisplayDataModel : ValidatableBindableBase
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private Shift _shift;
        public Shift Shift
        {
            get { return _shift; }
            set { SetProperty(ref _shift, value); }
        }
    }
}
