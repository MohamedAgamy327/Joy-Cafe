using DAL.BindableBaseService;
using DAL.ConstString;
using DTO.UserDataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Cafe.ViewModels.CashierViewModels
{
    public class CashierViewModel : ValidatableBindableBase
    {
        public CashierViewModel()
        {
            _currentViewModel = new DevicesViewModel();
            _title = $"اسم المستخدم: {UserData.Name}";
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private RelayCommand _shutdown;
        public RelayCommand Shutdown
        {
            get
            {
                return _shutdown ?? (_shutdown = new RelayCommand(
                    ExecuteShutdown));
            }
        }
        private void ExecuteShutdown()
        {
            try
            {
                if (UserData.Role == RoleText.Cashier && !UserData.newShift)
                {
                    new MainViewModel().ExecuteShutdown();

                }
                else if (UserData.Role == RoleText.Cashier && !string.IsNullOrEmpty(UserData.Name))
                {
                    new MainViewModel().NavigateToViewMethodAsync("Cashier");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
