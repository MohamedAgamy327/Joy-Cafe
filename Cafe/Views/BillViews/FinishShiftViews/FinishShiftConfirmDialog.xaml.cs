using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Cafe.Views.BillViews.FinishShiftViews
{
    public partial class FinishShiftConfirmDialog : CustomDialog
    {
        public FinishShiftConfirmDialog()
        {
            InitializeComponent();
            btn.Click += Btn_Click;
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in pnl.Children)
            {
                if (item is TextBox)
                {
                    TextBox txt = item as TextBox;
                    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }
            }
        }
    }
}
