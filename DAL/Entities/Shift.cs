using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Shift : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _userID;
        [Required]
        public int UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }

        private decimal? _safeStart;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal? SafeStart
        {
            get { return _safeStart; }
            set { SetProperty(ref _safeStart, value);}
        }

        private decimal? _income;
        public decimal? Income
        {
            get { return _income; }
            set { SetProperty(ref _income, value); }
        }

        private decimal? _spending;
        public decimal? Spending
        {
            get { return _spending; }
            set { SetProperty(ref _spending, value);}
        }

        private decimal? _total;
        public decimal? Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private decimal? _safeEnd;
        public decimal? SafeEnd
        {
            get { return _safeEnd; }
            set { SetProperty(ref _safeEnd, value); }
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

        public virtual User User { get; set; }
    }
}
