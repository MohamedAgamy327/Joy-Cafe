using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ClientMembershipMinute : ValidatableBindableBase
    {
        private int _clientID;
        [Key, Column(Order = 1)]
        [Required]
        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        private int _deviceTypeID;
        [Key, Column(Order = 2)]
        [Required]
        public int DeviceTypeID
        {
            get { return _deviceTypeID; }
            set { SetProperty(ref _deviceTypeID, value); }
        }

        private int _minutes;
        [Required]
        [Range(1, int.MaxValue)]
        public int Minutes
        {
            get { return _minutes; }
            set { SetProperty(ref _minutes, value); }
        }

        public virtual Client Client { get; set; }

        public virtual DeviceType DeviceType { get; set; }
    }
}
