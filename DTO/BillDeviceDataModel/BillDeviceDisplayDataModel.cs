using DAL.BindableBaseService;
using DAL.Entities;
using System;

namespace DTO.BillDeviceDataModel
{
    public class BillDeviceDisplayDataModel : ValidatableBindableBase
    {
        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        private Device _device;
        public Device Device
        {
            get { return _device; }
            set { SetProperty(ref _device, value); }
        }

        private BillDevice _billDevice;
        public BillDevice BillDevice
        {
            get { return _billDevice; }
            set { SetProperty(ref _billDevice, value); }
        }

        private int? _duration;
        public int? Duration
        {
            get
            {
                return (int)(Convert.ToDateTime(EndDate) - BillDevice.StartDate).TotalMinutes;
            }
            set { SetProperty(ref _duration, value); }
        }

        private decimal? _tolal;
        public decimal? Total
        {
            get { return _tolal = Duration * BillDevice.MinutePrice; }
            set { SetProperty(ref _tolal, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

    }
}
