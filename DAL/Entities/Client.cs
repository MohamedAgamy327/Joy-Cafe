using DAL.BindableBaseService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Client : ValidatableBindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int? _points;
        public int? Points
        {
            get { return _points; }
            set { SetProperty(ref _points, value); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

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
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        public virtual ICollection<Bill> Bills { get; set; }

        public virtual ICollection<ClientMembershipMinute> ClientMembershipMinutes { get; set; }

        public virtual ICollection<ClientMembership> ClientsMemberships { get; set; }
    }

}
