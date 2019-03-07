using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.ShiftViews
{
    public partial class ShiftWindow : MetroWindow
    {
        public ShiftWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Shift");
        }
    }
}
