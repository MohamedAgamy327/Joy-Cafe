using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.MembershipDataModel
{
    public class MembershipAddDataModel : ValidatableBindableBase
    {
        private int _deviceTypeID;
        [Required]
        public int DeviceTypeID
        {
            get { return _deviceTypeID; }
            set { SetProperty(ref _deviceTypeID, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private int? _minutes;
        [Required]
        [Range(1, int.MaxValue)]
        public int? Minutes
        {
            get { return _minutes; }
            set { SetProperty(ref _minutes, value); }
        }

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
    }
}
