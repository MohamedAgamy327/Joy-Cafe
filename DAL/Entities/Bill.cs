using DAL.BindableBaseService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Bill : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int? _userID;
        public int? UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }

        private int? _clientID;
        public int? ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        private int? _playedMinutes;
        public int? PlayedMinutes
        {
            get { return _playedMinutes; }
            set { SetProperty(ref _playedMinutes, value); }
        }

        // membership

        private int? _currentMembershipMinutes;
        public int? CurrentMembershipMinutes
        {
            get { return _currentMembershipMinutes; }
            set { SetProperty(ref _currentMembershipMinutes, value); }
        }

        private int? _membershipMinutesPaid;
        public int? MembershipMinutesPaid
        {
            get { return _membershipMinutesPaid; }
            set { SetProperty(ref _membershipMinutesPaid, value); }
        }

        private int? _membershipMinutesAfterPaid;
        public int? MembershipMinutesAfterPaid
        {
            get { return _membershipMinutesAfterPaid; }
            set { SetProperty(ref _membershipMinutesAfterPaid, value); }
        }

        private int? _remainderMinutes;
        public int? RemainderMinutes
        {
            get { return _remainderMinutes; }
            set { SetProperty(ref _remainderMinutes, value); }
        }

        // end of membership

        // points 

        private int? _currentPoints;
        public int? CurrentPoints
        {
            get { return _currentPoints; }
            set { SetProperty(ref _currentPoints, value); }
        }

        private int? _usedPoints;
        public int? UsedPoints
        {
            get { return _usedPoints; }
            set { SetProperty(ref _usedPoints, value); }
        }

        private int? _pointsAfterUsed;
        public int? PointsAfterUsed
        {
            get { return _pointsAfterUsed; }
            set { SetProperty(ref _pointsAfterUsed, value); }
        }

        private int? _earnedPoints;
        public int? EarnedPoints
        {
            get { return _earnedPoints; }
            set { SetProperty(ref _earnedPoints, value); }
        }
       
        // end points 

        private string _type;
        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _cancelReason;
        public string CancelReason
        {
            get { return _cancelReason; }
            set { SetProperty(ref _cancelReason, value); }
        }

        private decimal? _minimum;
        public decimal? Minimum
        {
            get { return _minimum; }
            set { SetProperty(ref _minimum, value); }
        }

        private decimal? _devicesSum;
        public decimal? DevicesSum
        {
            get { return _devicesSum; }
            set { SetProperty(ref _devicesSum, value); }
        }

        private decimal? _itemsSum;
        public decimal? ItemsSum
        {
            get { return _itemsSum; }
            set { SetProperty(ref _itemsSum, value); }
        }

        private decimal? _total;
        public decimal? Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private decimal? _discount;
        public decimal? Discount
        {
            get { return _discount; }
            set { SetProperty(ref _discount, value); }
        }

        private decimal? _ratio;
        public decimal? Ratio
        {
            get { return _ratio; }
            set { SetProperty(ref _ratio, value); }
        }

        private decimal? _totalAfterDiscount;
        public decimal? TotalAfterDiscount
        {
            get { return _totalAfterDiscount; }
            set { SetProperty(ref _totalAfterDiscount, value); }
        }

        private bool _deleted;
        public bool Deleted
        {
            get { return _deleted; }
            set { SetProperty(ref _deleted, value); }
        }

        private bool _canceled;
        public bool Canceled
        {
            get { return _canceled; }
            set { SetProperty(ref _canceled, value); }
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

        private DateTime? _date;
        [Column(TypeName = "Date")]
        public DateTime? Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        public virtual Client Client { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<BillItem> BillItems { get; set; }

        public virtual ICollection<BillDevice> BillDevices { get; set; }
    }
}
