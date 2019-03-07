using Utilities.Paging;

namespace DTO.SpendingDataModel
{
    public class SpendingReportDataModel : PagingWPF
    {
        private decimal? _totalAmount;
        public decimal? TotalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
        }
    }
}
