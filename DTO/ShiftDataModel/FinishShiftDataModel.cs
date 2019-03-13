using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.ShiftDataModel
{
    public class FinishShiftDataModel : ValidatableBindableBase
    {
        private string _currentUserName;
        public string CurrentUserName
        {
            get { return _currentUserName; }
            set { SetProperty(ref _currentUserName, value); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        private decimal? _safeStart;
        public decimal? SafeStart
        {
            get { return _safeStart; }
            set { SetProperty(ref _safeStart, value); }
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
            set { SetProperty(ref _spending, value); }
        }

        private decimal? _total;
        public decimal? Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }

        private decimal? _safeEnd;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? SafeEnd
        {
            get { return _safeEnd; }
            set { SetProperty(ref _safeEnd, value); }
        }

        private bool _newShift;
        public bool NewShift
        {
            get { return _newShift; }
            set { SetProperty(ref _newShift, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

    }
}
