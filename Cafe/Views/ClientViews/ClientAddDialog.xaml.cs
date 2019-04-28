using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Cafe.Views.ClientViews
{
    public partial class ClientAddDialog : CustomDialog
    {
        public ClientAddDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            First.Focus();
            foreach (FrameworkElement item in pnl.Children)
            {
                First.Focus();
                if (item is TextBox)
                {
                    TextBox txt = item as TextBox;
                    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }
    }
}
