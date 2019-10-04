using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ItemViews
{
    public partial class ItemReportUserControl : UserControl
    {
        public ItemReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ItemReport");
        }
    }
}
