using DAL.BindableBaseService;
using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class BillItem : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _billID;
        [Required]
        public int BillID
        {
            get { return _billID; }
            set { SetProperty(ref _billID, value); }
        }

        private int _itemID;
        [Required]
        public int ItemID
        {
            get { return _itemID; }
            set { SetProperty(ref _itemID, value); }
        }

        private decimal? _price;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private int? _qty;
        [Required]
        [Range(1, int.MaxValue)]
        public int? Qty
        {
            get { return _qty; }
            set { SetProperty(ref _qty, value); }
        }

        private decimal? _tolal;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? Total
        {
            get { return _tolal = Qty * Price; }
            set { SetProperty(ref _tolal, value); }
        }

        private DateTime _registrationDate;
        [Required]
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { SetProperty(ref _registrationDate, value); }
        }

        public virtual Bill Bill { get; set; }

        public virtual Item Item { get; set; }
    }
}
