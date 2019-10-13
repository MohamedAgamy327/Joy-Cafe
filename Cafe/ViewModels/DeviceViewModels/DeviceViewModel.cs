using DAL.BindableBaseService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Cafe.ViewModels.DeviceViewModels
{
    public class DeviceViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public DeviceViewModel()
        {
            Destination = "DeviceTypeDisplay";
            _currentViewModel = new DeviceTypeDisplayViewModel();
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethod));
            }
        }
        private void NavigateToViewMethod(string destination)
        {
            try
            {
                if (Destination != destination)
                {
                    Destination = destination;
                    switch (destination)
                    {
                        case "DeviceTypeDisplay":
                            CurrentViewModel = new DeviceTypeDisplayViewModel();
                            break;
                        case "DeviceDisplay":
                            CurrentViewModel = new DeviceDisplayViewModel();
                            break;
                        case "DeviceReport":
                            CurrentViewModel = new DeviceReportViewModel();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
