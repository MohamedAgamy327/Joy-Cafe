using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.ShiftDataModel
{
    public class StartShiftDataModel : ValidatableBindableBase
    {
        private decimal? _safeStart;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal? SafeStart
        {
            get { return _safeStart; }
            set { SetProperty(ref _safeStart, value); }
        }
    }
}
