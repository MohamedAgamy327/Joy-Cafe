using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.UserViews
{
    public partial class UserDisplayUserControl : UserControl
    {
        public UserDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("UserDisplay");
        }
    }
}
