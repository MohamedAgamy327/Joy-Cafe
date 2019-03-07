using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.UserDataModel
{
    public class UserAddDataModel : ValidatableBindableBase
    {
        private int _roleID;
        [Required]
        public int RoleID
        {
            get { return _roleID; }
            set { SetProperty(ref _roleID, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _password;
        [Required]
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
    }
}
