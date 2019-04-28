using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace Cafe.Views.MembershipViews
{
    public partial class ClientMembershipAddDialog : CustomDialog
    {
        public ClientMembershipAddDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            First.Focus();
        }
    }
}
