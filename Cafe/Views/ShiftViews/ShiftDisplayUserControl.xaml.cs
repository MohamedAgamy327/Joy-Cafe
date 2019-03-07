using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ShiftViews
{ 
    public partial class ShiftDisplayUserControl : UserControl
    {
        public ShiftDisplayUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ShiftDisplay");
        }
    }
}
