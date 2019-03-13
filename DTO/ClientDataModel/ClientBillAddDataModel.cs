using DAL.BindableBaseService;
using System.ComponentModel.DataAnnotations;

namespace DTO.ClientDataModel
{
    public class ClientBillAddDataModel : ValidatableBindableBase
    {
        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _telephone;
        [Required]
        [Phone]
        [StringLength(11, MinimumLength = 11)]
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }
    }
}

