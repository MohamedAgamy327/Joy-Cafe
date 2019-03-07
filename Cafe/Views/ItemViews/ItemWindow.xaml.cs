using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.ItemViews
{
    public partial class ItemWindow : MetroWindow
    {
        public ItemWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Item");
        }
    }
}
