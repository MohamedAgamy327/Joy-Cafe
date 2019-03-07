using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.ItemDataModel
{
    public class ItemDisplayDataModel : ValidatableBindableBase
    {
        private bool _canDelete;
        public bool CanDelete
        {
            get { return _canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private Item _item;
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
    }
}
