using Cafe.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace Cafe.Views.CashierViews.AccountPaidViews
{
    public partial class AccountPaidWindow : MetroWindow
    {
        public AccountPaidWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup("AccountPaid");
            btn.Click += Btn_Click;
            btn2.Click += Btn2_Click;
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in pnl.Children)
            {
                if (item is NumericUpDown)
                {
                    NumericUpDown nud = item as NumericUpDown;
                    nud.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in pnl.Children)
            {
                if (item is NumericUpDown)
                {
                    NumericUpDown nud = item as NumericUpDown;
                    nud.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
                }
            }
        }
    }
}
