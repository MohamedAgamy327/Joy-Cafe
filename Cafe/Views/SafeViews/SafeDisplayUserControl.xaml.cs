using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.SafeViews
{
    public partial class SafeDisplayUserControl : UserControl
    {
        public SafeDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeDisplay");
        }
    }
}
