using DAL.BindableBaseService;
using DAL.Entities;

namespace DTO.BillDataModel
{
    public class BillDisplayDataModel : ValidatableBindableBase
    {
        private Bill _bill;
        public Bill Bill
        {
            get { return _bill; }
            set { SetProperty(ref _bill, value); }
        }
    }
}
