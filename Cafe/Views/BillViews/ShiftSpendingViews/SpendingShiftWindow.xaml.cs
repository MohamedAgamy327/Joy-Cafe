using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews.ShiftSpendingViews
{
    public partial class SpendingShiftWindow : MetroWindow
    {
        public SpendingShiftWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("SpendingShift");
        }
    }
}
