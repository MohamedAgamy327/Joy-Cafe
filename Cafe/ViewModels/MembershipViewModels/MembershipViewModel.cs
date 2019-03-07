using Cafe.ViewModels.ClientViewModels;
using DAL.BindableBaseService;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cafe.ViewModels.MembershipViewModels
{
    public class MembershipViewModel : ValidatableBindableBase
    {
        static string Destination { get; set; }

        public MembershipViewModel()
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
            if (Destination != destination)
            {
                Destination = destination;
                switch (destination)
                {
                    case "ClientDisplay":
                        CurrentViewModel = new ClientDisplayViewModel();
                        break;
                    case "Membership":
                        CurrentViewModel = new MembershipDisplayViewModel();
                        break;
                    case "ClientMembership":
                        CurrentViewModel = new ClientMembershipViewModel();
                        break;
                    case "ClientMembershipMinute":
                        CurrentViewModel = new ClientMembershipMinuteViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
