using DAL.BindableBaseService;

namespace DTO.BillDataModel
{
    public class BillCaseDataModel : ValidatableBindableBase
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
