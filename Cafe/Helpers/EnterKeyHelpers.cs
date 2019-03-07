using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Cafe.Helpers
{
    public static class EnterKeyHelpers
    {
        public static ICommand GetEnterKeyCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(EnterKeyCommandProperty);
        }

        public static void SetEnterKeyCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(EnterKeyCommandProperty, value);
        }

        public static readonly DependencyProperty EnterKeyCommandProperty =
            DependencyProperty.RegisterAttached(
                "EnterKeyCommand",
                typeof(ICommand),
                typeof(EnterKeyHelpers),
                new PropertyMetadata(null, OnEnterKeyCommandChanged));

        static void OnEnterKeyCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ICommand command = (ICommand)e.NewValue;
            FrameworkElement fe = (FrameworkElement)target;
            System.Windows.Controls.Control control = (System.Windows.Controls.Control)target;
            control.KeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    // make sure the textbox binding updates its source first
                    BindingExpression b = control.GetBindingExpression(NumericUpDown.ValueProperty);
                    if (b != null)
                    {
                        b.UpdateSource();
                    }
                    command.Execute(null);
                }
            };
        }
    }
}
