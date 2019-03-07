using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.SafeDataModel
{
    public class SafeDisplayDataModel : ValidatableBindableBase
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private Safe _safe;
        public Safe Safe
        {
            get { return _safe; }
            set { SetProperty(ref _safe, value); }
        }
    }
}
