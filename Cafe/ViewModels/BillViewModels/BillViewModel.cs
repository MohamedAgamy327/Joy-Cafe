using DAL.BindableBaseService;
using DAL.ConstString;
using DTO.UserDataModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillViewModel : ValidatableBindableBase
    {
        public BillViewModel()
        {
            _currentViewModel = new DevicesViewModel();
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
                if (UserData.Role == RoleText.Cashier)
                {
                    new MainViewModel().ExecuteShutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
