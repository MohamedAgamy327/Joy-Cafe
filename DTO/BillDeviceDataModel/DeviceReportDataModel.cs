using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillDeviceDataModel
{
    public class DeviceReportDataModel : ValidatableBindableBase
    {
        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _hours;
        public string Hours
        {
            get { return _hours; }
            set { SetProperty(ref _hours, value); }
        }

        private int? _minutes;
        public int? Minutes
        {
            get { return _minutes; }
            set { SetProperty(ref _minutes, value); }
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }

    }
}
