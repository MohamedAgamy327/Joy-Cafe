using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.BillViews
{
    public partial class DevicesUserControl : UserControl
    {
        public DevicesUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("Devices");
        }
    }
}
