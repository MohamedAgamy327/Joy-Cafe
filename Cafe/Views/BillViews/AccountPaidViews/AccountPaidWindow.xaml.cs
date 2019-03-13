using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews.AccountPaidViews
{
    public partial class AccountPaidWindow : MetroWindow
    {
        public AccountPaidWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("AccountPaid");
        }
    }
}
