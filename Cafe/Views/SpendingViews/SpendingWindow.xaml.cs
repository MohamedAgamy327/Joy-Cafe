using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.SpendingViews
{
    public partial class SpendingWindow : MetroWindow
    {
        public SpendingWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Spnending");
        }
    }
}
