using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ItemViews
{
    public partial class ItemUserControl : UserControl
    {
        public ItemUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ItemDisplay");
        }
    }
}
