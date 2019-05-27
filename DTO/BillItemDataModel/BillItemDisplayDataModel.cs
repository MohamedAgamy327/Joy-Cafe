using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillItemDataModel
{
    public class BillItemDisplayDataModel : ValidatableBindableBase
    {
        private BillItem _billItem;
        public BillItem BillItem
        {
            get { return _billItem; }
            set { SetProperty(ref _billItem, value); }
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

    }
}
