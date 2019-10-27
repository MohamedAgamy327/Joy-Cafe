using DAL.BindableBaseService;
using DAL.ConstString;
using DTO.UserDataModel;
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
            if (UserData.Role == RoleText.Admin)
            {
                _taxVisibility = Visibility.Visible;
            }
            else if (UserData.Role == RoleText.Tax)
            {
                _taxVisibility = Visibility.Collapsed;
            }
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private Visibility _taxVisibility;
        public Visibility TaxVisibility
        {
            get { return _taxVisibility; }
            set { SetProperty(ref _taxVisibility, value); }
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
