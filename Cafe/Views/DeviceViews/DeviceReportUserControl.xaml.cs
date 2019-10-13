using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.DeviceViews
{
    public partial class DeviceReportUserControl : UserControl
    {
        public DeviceReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("DeviceReport");
        }
    }
}
