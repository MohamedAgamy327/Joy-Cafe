using DAL.BindableBaseService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public BillViewModel()
        {
         //   Destination = "BillDisplay";
            Destination = "BillDay";
            _currentViewModel = new BillDayViewModel();
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
                        case "BillDisplay":
                            CurrentViewModel = new BillDisplayViewModel();
                            break;
                        case "BillDay":
                            CurrentViewModel = new BillDayViewModel();
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
