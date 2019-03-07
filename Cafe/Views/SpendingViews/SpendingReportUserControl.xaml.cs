using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.SpendingViews
{
    public partial class SpendingReportUserControl : UserControl
    {
        public SpendingReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingReport");
        }
    }
}
