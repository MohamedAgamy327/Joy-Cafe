using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.MainDataModel
{
    public class LoginDataModel : ValidatableBindableBase
    {
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
