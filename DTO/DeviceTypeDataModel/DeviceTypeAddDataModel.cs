using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.DeviceTypeDataModel
{
    public class DeviceTypeAddDataModel : ValidatableBindableBase
    {
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal? _singleHourPrice;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal? SingleHourPrice
        {
            get { return _singleHourPrice; }
            set
            {
                SetProperty(ref _singleHourPrice, value);
                OnPropertyChanged("SingleMinutePrice");
            }
        }

        private decimal? _singleMinutePrice;
        public decimal? SingleMinutePrice
        {
            get { return _singleMinutePrice = SingleHourPrice / 60; }
            set { SetProperty(ref _singleMinutePrice, value); }
        }

        private decimal? _multiHourPrice;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal? MultiHourPrice
        {
            get { return _multiHourPrice; }
            set
            {
                SetProperty(ref _multiHourPrice, value);
                OnPropertyChanged("MultiMinutePrice");
            }
        }

        private decimal? _multiMinutePrice;
        public decimal? MultiMinutePrice
        {
            get { return _multiMinutePrice = MultiHourPrice / 60; }
            set { SetProperty(ref _multiMinutePrice, value); }
        }

        private bool _birthday;
        public bool Birthday
        {
            get { return _birthday; }
            set
            {
                SetProperty(ref _birthday, value);
                OnPropertyChanged("BirthdayVisibility");
            }
        }

        private string _birthdayVisibility;
        public string BirthdayVisibility
        {
            get
            {
                if (Birthday)
                    return _birthdayVisibility = "Visible";
                else
                    return _birthdayVisibility = "Collapsed";
            }
            set { SetProperty(ref _birthdayVisibility, value); }
        }

        private decimal? _birthdayHourPrice;
        public decimal? BirthdayHourPrice
        {
            get { return _birthdayHourPrice; }
            set
            {
                SetProperty(ref _birthdayHourPrice, value);
                OnPropertyChanged("BirthdayMinutePrice");
            }
        }

        private decimal? _birthdayMinutePrice;
        public decimal? BirthdayMinutePrice
        {
            get { return _birthdayMinutePrice = BirthdayHourPrice / 60; }
            set { SetProperty(ref _birthdayMinutePrice, value); }
        }
    }
}
