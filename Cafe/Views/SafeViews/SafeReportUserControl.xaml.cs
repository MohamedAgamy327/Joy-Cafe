using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.SafeViews
{
    public partial class SafeReportUserControl : UserControl
    {
        public SafeReportUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SafeReport");
        }
    }
}
