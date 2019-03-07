using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.UserDataModel
{
    public class UserDisplayDataModel : ValidatableBindableBase
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

        private User _user;
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private Role _role;
        public Role Role
        {
            get { return _role; }
            set { SetProperty(ref _role, value); }
        }
    }
}
