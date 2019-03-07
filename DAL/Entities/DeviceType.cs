using DAL.BindableBaseService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class DeviceType : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal? _singleHourPrice;
        public decimal? SingleHourPrice
        {
            get { return _singleHourPrice; }
            set { SetProperty(ref _singleHourPrice, value); }
        }

        private decimal? _singleMinutePrice;
        public decimal? SingleMinutePrice
        {
            get { return _singleMinutePrice; }
            set { SetProperty(ref _singleMinutePrice, value); }
        }

        private decimal? _multiHourPrice;
        public decimal? MultiHourPrice
        {
            get { return _multiHourPrice; }
            set { SetProperty(ref _multiHourPrice, value); }
        }

        private decimal? _multiMinutePrice;
        public decimal? MultiMinutePrice
        {
            get { return _multiMinutePrice; }
            set { SetProperty(ref _multiMinutePrice, value); }
        }

        private bool _birthday;
        public bool Birthday
        {
            get { return _birthday; }
            set { SetProperty(ref _birthday, value); }
        }

        private decimal? _birthdayHourPrice;
        public decimal? BirthdayHourPrice
        {
            get { return _birthdayHourPrice; }
            set { SetProperty(ref _birthdayHourPrice, value); }
        }

        private decimal? _birthdayMinutePrice;
        public decimal? BirthdayMinutePrice
        {
            get { return _birthdayMinutePrice; }
            set { SetProperty(ref _birthdayMinutePrice, value); }
        }

        public virtual ICollection<Membership> Memberships { get; set; }

        public virtual ICollection<ClientMembershipMinute> ClientMembershipMinutes { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
