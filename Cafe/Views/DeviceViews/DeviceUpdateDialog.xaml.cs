﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Cafe.Views.DeviceViews
{
    public partial class DeviceUpdateDialog : CustomDialog
    {
        public DeviceUpdateDialog()
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
                if (item is NumericUpDown)
                {
                    NumericUpDown nud = item as NumericUpDown;
                    nud.GetBindingExpression(NumericUpDown.ValueProperty).UpdateSource();
                }
            }
        }
    }
}
