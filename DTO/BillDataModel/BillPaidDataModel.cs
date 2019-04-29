using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.BillDataModel
{
    public class BillPaidDataModel : ValidatableBindableBase
    {

        public BillPaidDataModel()
        {
            Discount = 0;
            Ratio = 0;
            UsedPoints = 0;
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

        private decimal? _minimum;
        public decimal? Minimum
        {
            get { return _minimum; }
            set { SetProperty(ref _minimum, value); }
        }

        private int? _usedPoints;
        [Required]
        [Range(0, int.MaxValue)]
        public int? UsedPoints
        {
            get { return _usedPoints; }
            set { SetProperty(ref _usedPoints, value); }
        }

    }
}
