using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.UserViews
{
    public partial class UserWindow : MetroWindow
    {
        public UserWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("User");
        }
    }
}
