using DAL.BindableBaseService;

namespace DTO.BillDataModel
{
    public class BillPaidDataModel : ValidatableBindableBase
    {
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

        private decimal? _minimum;
        public decimal? Minimum
        {
            get { return _minimum; }
            set { SetProperty(ref _minimum, value); }
        }

        public BillPaidDataModel()
        {
            Discount = 0;
            Ratio = 0;
        }

    }
}
