using DAL.BindableBaseService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Cafe.ViewModels.SpendingViewModels
{
    public class SpendingViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public SpendingViewModel()
        {
            Destination = "SpendingDisplay";
            _currentViewModel = new SpendingDisplayViewModel();
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
                        case "SpendingDisplay":
                            CurrentViewModel = new SpendingDisplayViewModel();
                            break;
                        case "SpendingReport":
                            CurrentViewModel = new SpendingReportViewModel();
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
