using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews.ShiftSpendingViews
{
    public partial class ShiftSpendingWindow : MetroWindow
    {
        public ShiftSpendingWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("ShiftSpending");
        }
    }
}
