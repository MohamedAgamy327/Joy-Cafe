using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.MembershipViews
{
    public partial class MembershipWindow : MetroWindow
    {
        public MembershipWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Membership");
        }
    }
}
