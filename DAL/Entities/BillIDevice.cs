using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class BillDevice : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _billID;
        [Required]
        public int BillID
        {
            get { return _billID; }
            set { SetProperty(ref _billID, value); }
        }

        private int _deviceID;
        [Required]
        public int DeviceID
        {
            get { return _deviceID; }
            set { SetProperty(ref _deviceID, value); }
        }

        private decimal? _minutePrice;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? MinutePrice
        {
            get { return _minutePrice; }
            set { SetProperty(ref _minutePrice, value); }
        }

        private decimal? _tolal;
        public decimal? Total
        {
            get { return _tolal = Duration * MinutePrice; }
            set { SetProperty(ref _tolal, value); }
        }

        private int? _duration;
        public int? Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        private string _gameType;
        [Required]
        public string GameType
        {
            get { return _gameType; }
            set { SetProperty(ref _gameType, value); }
        }

        private DateTime _startDate;
        [Required]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        public virtual Bill Bill { get; set; }

        public virtual Device Device { get; set; }
    }
}
