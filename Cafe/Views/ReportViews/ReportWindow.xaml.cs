using Cafe.ViewModels;
using MahApps.Metro.Controls;

namespace Cafe.Views.ReportViews
{
    public partial class ReportWindow : MetroWindow
    {
        public ReportWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Report");
        }
    }
}
