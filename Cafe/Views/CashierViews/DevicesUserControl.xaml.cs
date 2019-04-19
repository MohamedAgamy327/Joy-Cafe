using Cafe.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cafe.Views.CashierViews
{
    public partial class DevicesUserControl : UserControl
    {
        public DevicesUserControl()
        {
            InitializeComponent();
            Unloaded += (s, e) => ViewModelLocator.Cleanup("Devices");
        }

        private void MyListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                listView1.UnselectAll();
        }

        private void MyListView2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                listView2.UnselectAll();
        }
    }
}
