using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.ClientMembershipDataModel
{
    public class ClientMembershipAddDataModel : ValidatableBindableBase
    {
        private int _userID;
        [Required]
        public int UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }

        private int _membershipID;
        [Required]
        public int MembershipID
        {
            get { return _membershipID; }
            set { SetProperty(ref _membershipID, value); }
        }

        private int _clientID;
        [Required]
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

    }
}
