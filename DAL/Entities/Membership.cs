using DAL.BindableBaseService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Membership : ValidatableBindableBase
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

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { SetProperty(ref _isAvailable, value); }
        }

        public virtual DeviceType DeviceType { get; set; }

        public virtual ICollection<ClientMembership> ClientsMemberships { get; set; }

        
    }
}
