using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.BillDataModel
{
    public class BillCancelDataModel : ValidatableBindableBase
    {
        private string _cancelReason;
        [Required]
        public string CancelReason
        {
            get { return _cancelReason; }
            set { SetProperty(ref _cancelReason, value); }
        }
    }
}
