using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.CashierViews.BillItemsViews
{
    public partial class BillItemsWindow : MetroWindow
    {
        public BillItemsWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("BillItems");
        }
    }
}
