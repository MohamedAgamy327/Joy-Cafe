using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.DeviceViews
{
    public partial class DeviceWindow : MetroWindow
    {
        public DeviceWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Device");
        }
    }
}
