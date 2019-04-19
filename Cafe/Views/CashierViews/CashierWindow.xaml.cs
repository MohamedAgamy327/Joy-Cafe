using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.CashierViews
{
    public partial class CashierWindow : MetroWindow
    {
        public CashierWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Cashier");
        }
    }
}
