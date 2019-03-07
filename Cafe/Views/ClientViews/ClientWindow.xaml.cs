using MahApps.Metro.Controls;
using Cafe.ViewModels;

namespace Cafe.Views.ClientViews
{
    public partial class ClientWindow : MetroWindow
    {
        public ClientWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("Client");
        }
    }
}
