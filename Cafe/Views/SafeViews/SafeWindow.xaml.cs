using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.SafeViews
{
    public partial class SafeWindow : MetroWindow
    {
        public SafeWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Safe");
        }
    }
}
