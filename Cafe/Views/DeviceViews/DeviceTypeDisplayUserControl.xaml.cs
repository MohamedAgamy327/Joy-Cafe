using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.DeviceViews
{

    public partial class DeviceTypeDisplayUserControl : UserControl
    {
        public DeviceTypeDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("DeviceTypeDisplay");
        }
    }
}
