using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.DeviceDataModel
{
    public class DeviceAddDataModel : ValidatableBindableBase
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

        private int? _order;
        [Required]
        [Range(1, int.MaxValue)]
        public int? Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

    }
}
