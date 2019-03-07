using DAL.BindableBaseService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Device : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int? _billID;
        public int? BillID
        {
            get { return _billID; }
            set { SetProperty(ref _billID, value); }
        }

        private int _deviceTypeID;
        [Required]
        public int DeviceTypeID
        {
            get { return _deviceTypeID; }
            set { SetProperty(ref _deviceTypeID, value); }
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { SetProperty(ref _isAvailable, value); }
        }

        private string _case;
        [Required]
        public string Case
        {
            get { return _case; }
            set { SetProperty(ref _case, value); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public virtual DeviceType DeviceType { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual ICollection<BillDevice> BillsDevices { get; set; }
    }
}
