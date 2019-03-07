using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.MembershipViews
{
    public partial class ClientMembershipUserControl : UserControl
    {
        public ClientMembershipUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientMembership");
        }
    }
}
