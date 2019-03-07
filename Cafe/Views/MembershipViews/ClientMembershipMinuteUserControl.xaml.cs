using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.MembershipViews
{
    public partial class ClientMembershipMinuteUserControl : UserControl
    {
        public ClientMembershipMinuteUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientMembershipMinute");
        }
    }
}
