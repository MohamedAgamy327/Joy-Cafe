using DAL.BindableBaseService;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace DTO.UserDataModel
{
    public class UserUpdateDataModel : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _roleID;
        [Required]
        public int RoleID
        {
            get { return _roleID; }
            set { SetProperty(ref _roleID, value); }
        }

        private bool _isWorked;
        public bool IsWorked
        {
            get { return _isWorked; }
            set { SetProperty(ref _isWorked, value); }
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

        private Role _role;
        public  Role Role
        {
            get { return _role; }
            set { SetProperty(ref _role, value); }
        }
    }
}
