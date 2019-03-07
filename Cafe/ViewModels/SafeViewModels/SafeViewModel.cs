using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using DAL.BindableBaseService;

namespace Cafe.ViewModels.SafeViewModels
{
    public class SafeViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public SafeViewModel()
        {
            Destination = "SafeDisplay";
            _currentViewModel = new SafeDisplayViewModel();
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
                        case "SafeDisplay":
                            CurrentViewModel = new SafeDisplayViewModel();
                            break;
                        case "SafeReport":
                            CurrentViewModel = new SafeReportViewModel();
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
