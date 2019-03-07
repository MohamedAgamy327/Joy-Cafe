using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.BillViews
{
    public partial class ItemsWindow : MetroWindow
    {
        public ItemsWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Items");
        }
    }
}
