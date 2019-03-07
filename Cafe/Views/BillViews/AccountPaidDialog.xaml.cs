using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace Cafe.Views.BillViews
{
    public partial class AccountPaidDialog : CustomDialog
    {
        public AccountPaidDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
            btn2.Click += Btn2_Click;
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            nud1.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            nud2.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            nud1.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
            nud2.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
        }
    }
}
