using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.MembershipViews
{
    public partial class MembershipDisplayUserControl : UserControl
    {
        public MembershipDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("MembershipDisplay");
        }
    }
}
