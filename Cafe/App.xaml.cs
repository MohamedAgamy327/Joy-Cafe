using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Cafe
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
