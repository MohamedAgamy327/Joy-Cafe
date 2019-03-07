using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.SpendingDataModel
{
    public class SpendingAddDataModel : ValidatableBindableBase
    {
        private string _statement;
        [Required]
        public string Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        private decimal? _amount;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
    }
}
