using DAL.BindableBaseService;

namespace DTO.ShiftDataModel
{
    public class ShiftsReportDataModel : ValidatableBindableBase
    {
        private decimal? _totalMinimum;
        public decimal? TotalMinimum
        {
            get { return _totalMinimum; }
            set { SetProperty(ref _totalMinimum, value); }
        }

        private decimal? _totalDevices;
        public decimal? TotalDevices
        {
            get { return _totalDevices; }
            set { SetProperty(ref _totalDevices, value); }
        }

        private decimal? _totalItems;
        public decimal? TotalItems
        {
            get { return _totalItems; }
            set { SetProperty(ref _totalItems, value); }
        }

        private decimal? _totalDiscount;
        public decimal? TotalDiscount
        {
            get { return _totalDiscount; }
            set { SetProperty(ref _totalDiscount, value); }
        }

        private decimal? _totalSpendings;
        public decimal? TotalSpending
        {
            get { return _totalSpendings; }
            set { SetProperty(ref _totalSpendings, value); }
        }

        private decimal? _totalIncome;
        public decimal? TotalIncome
        {
            get { return _totalIncome; }
            set { SetProperty(ref _totalIncome, value); }
        }

        private decimal? _totalNet;
        public decimal? TotalNet
        {
            get { return _totalNet; }
            set { SetProperty(ref _totalNet, value); }
        }
    }
}
