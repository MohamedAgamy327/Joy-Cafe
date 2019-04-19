using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.BillViews
{
    public partial class BillDayUserControl : UserControl
    {
        public BillDayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("BillDay");
        }
    }
}
