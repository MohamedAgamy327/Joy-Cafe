using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.BillItemDataModel
{
    public class BillItemAddDataModel : ValidatableBindableBase
    {
        private int _itemID;
        [Required]
        public int ItemID
        {
            get { return _itemID; }
            set { SetProperty(ref _itemID, value); }
        }

        private int? _qty;
        [Required]
        [Range(1, int.MaxValue)]
        public int? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }
    }
}
