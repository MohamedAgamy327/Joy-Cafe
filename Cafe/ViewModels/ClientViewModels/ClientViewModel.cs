using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using DAL.BindableBaseService;

namespace Cafe.ViewModels.ClientViewModels
{
    public class ClientViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public ClientViewModel()
        {
            Destination = "ClientDisplay";
            _currentViewModel = new ClientDisplayViewModel();
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
                        case "ClientDisplay":
                            CurrentViewModel = new ClientDisplayViewModel();
                            break;
                        case "ClientPoint":
                            CurrentViewModel = new ClientPointViewModel();
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
