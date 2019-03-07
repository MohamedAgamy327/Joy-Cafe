using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ClientMembership : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

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

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private DateTime _registrationDate;
        [Required]
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { SetProperty(ref _registrationDate, value); }
        }

        private DateTime _date;
        [Column(TypeName = "Date")]
        [Required]
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public virtual User User { get; set; }

        public virtual Client Client { get; set; }

        public virtual Membership Membership { get; set; }

    }
}
