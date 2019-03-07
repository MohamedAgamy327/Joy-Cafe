using Utilities.Paging;

namespace DTO.SafeDataModel
{
    public class SafeReportDataModel : PagingWPF
    {
        private decimal? _currentAccount;
        public decimal? CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }

        private decimal? _totalIncome;
        public decimal? TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                SetProperty(ref _totalIncome, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _totalOutgoings;
        public decimal? TotalOutgoings
        {
            get { return _totalOutgoings; }
            set
            {
                SetProperty(ref _totalOutgoings, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _difference;
        public decimal? Difference
        {
            get { return _difference = _totalIncome + _totalOutgoings; }
            set { SetProperty(ref _difference, value); }
        }
    }
}
