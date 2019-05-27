using DAL.BindableBaseService;

namespace DTO.ItemDataModel
{
    public class ItemPrintDataModel : ValidatableBindableBase
    {

        public int? Qty { get; set; }

        public string Name { get; set; }
    }
}
