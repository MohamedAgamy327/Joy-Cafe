using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ItemViews
{
    public partial class DevicesItemsReportUserControl : UserControl
    {
        public DevicesItemsReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("DevicesItemsReport");
        }
    }
}
