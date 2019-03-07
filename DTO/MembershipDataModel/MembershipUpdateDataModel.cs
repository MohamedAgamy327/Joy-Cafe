using DAL.BindableBaseService;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace DTO.MembershipDataModel
{
    public class MembershipUpdateDataModel : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _deviceTypeID;
        [Required]
        public int DeviceTypeID
        {
            get { return _deviceTypeID; }
            set { SetProperty(ref _deviceTypeID, value); }
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

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { SetProperty(ref _isAvailable, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }
    }
}
