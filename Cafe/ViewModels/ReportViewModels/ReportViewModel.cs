using Cafe.ViewModels.BillViewModels;
using Cafe.ViewModels.ClientViewModels;
using Cafe.ViewModels.ShiftViewModels;
using Cafe.ViewModels.SpendingViewModels;
using DAL.BindableBaseService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cafe.ViewModels.ReportViewModels
{
    public class ReportViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public ReportViewModel()
        {
            Destination = "Clients";
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
            if (Destination != destination)
            {
                Destination = destination;
                switch (destination)
                {
                    case "Clients":
                        CurrentViewModel = new ClientDisplayViewModel();
                        break;
                    case "Bills":
                        CurrentViewModel = new BillDisplayViewModel();
                        break;
                    case "Shifts":
                        CurrentViewModel = new ShiftDisplayViewModel();
                        break;
                    case "Spendings":
                        CurrentViewModel = new SpendingReportViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
