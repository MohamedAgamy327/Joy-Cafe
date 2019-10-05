using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using DTO.BillItemDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillShowViewModel : ValidatableBindableBase
    {
        public static int BillID { get; set; }

        MetroWindow currentWindow;

        public BillShowViewModel()
        {
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private Visibility _isMembership;
        public Visibility IsMembership
        {
            get { return _isMembership; }
            set { SetProperty(ref _isMembership, value); }
        }

        private Bill _selectedBill;
        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set { SetProperty(ref _selectedBill, value); }
        }

        private ObservableCollection<BillItemDisplayDataModel> _billItems;
        public ObservableCollection<BillItemDisplayDataModel> BillItems
        {
            get { return _billItems; }
            set { SetProperty(ref _billItems, value); }
        }

        private ObservableCollection<BillDeviceDisplayDataModel> _billDevices;
        public ObservableCollection<BillDeviceDisplayDataModel> BillDevices
        {
            get { return _billDevices; }
            set { SetProperty(ref _billDevices, value); }
        }

        private RelayCommand _loaded;
        public RelayCommand Loaded
        {
            get
            {
                return _loaded
                    ?? (_loaded = new RelayCommand(LoadedMethod));
            }
        }
        private void LoadedMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    SelectedBill = unitOfWork.Bills.GetById(BillID);
                    BillDevices = new ObservableCollection<BillDeviceDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(BillID));
                    BillItems = new ObservableCollection<BillItemDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(BillID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
