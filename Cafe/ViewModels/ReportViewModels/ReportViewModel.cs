using Cafe.ViewModels.BillViewModels;
using Cafe.ViewModels.ClientViewModels;
using Cafe.ViewModels.ShiftViewModels;
using Cafe.ViewModels.SpendingViewModels;
using DAL.BindableBaseService;
using DAL.ConstString;
using DTO.UserDataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;

namespace Cafe.ViewModels.ReportViewModels
{
    public class ReportViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public ReportViewModel()
        {
            Destination = "Clients";
            _currentViewModel = new ClientDisplayViewModel();
            if (UserData.Role == RoleText.Admin)
            {
                _taxVisibility = Visibility.Visible;
            }
            else if (UserData.Role == RoleText.Tax)
            {
                _taxVisibility = Visibility.Collapsed;
            }
        }

        private Visibility _taxVisibility;
        public Visibility TaxVisibility
        {
            get { return _taxVisibility; }
            set { SetProperty(ref _taxVisibility, value); }
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
