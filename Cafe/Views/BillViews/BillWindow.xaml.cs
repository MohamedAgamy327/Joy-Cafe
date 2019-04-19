using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews
{
    public partial class BillWindow :  MetroWindow
    {
        public BillWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Bill");
        }
    }
}
