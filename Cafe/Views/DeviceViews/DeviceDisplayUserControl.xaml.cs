using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.DeviceViews
{
    public partial class DeviceDisplayUserControl : UserControl
    {
        public DeviceDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("DeviceDisplay");
        }
    }
}
