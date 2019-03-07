using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.SpendingViews
{
    public partial class SpendingDisplayUserControl : UserControl
    {
        public SpendingDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("SpendingDisplay");
        }
    }
}
