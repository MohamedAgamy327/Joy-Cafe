using Cafe.ViewModels;
using System.Windows.Controls;

namespace Cafe.Views.ClientViews
{
    public partial class ClientPointUserControl : UserControl
    {
        public ClientPointUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("ClientPoint");
        }
    }
}
