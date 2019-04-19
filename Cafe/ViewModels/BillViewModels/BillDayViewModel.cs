using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DTO.BillDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillDayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Bills = new ObservableCollection<BillDayDataModel>(unitOfWork.Bills.Search(Date));
                SelectedBills = Bills.Where(w => w.Bill.IsDeleted == true).Count();
            }
        }

        public BillDayViewModel()
        {
            _date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private decimal _selectedBills;
        public decimal SelectedBills
        {
            get { return _selectedBills; }
            set { SetProperty(ref _selectedBills, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private ObservableCollection<BillDayDataModel> _bills;
        public ObservableCollection<BillDayDataModel> Bills
        {
            get { return _bills; }
            set { SetProperty(ref _bills, value); }
        }

        // Display

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
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _search;
        public RelayCommand Search
        {
            get
            {
                return _search
                    ?? (_search = new RelayCommand(SearchMethod));
            }
        }
        private void SearchMethod()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _check;
        public RelayCommand Check
        {
            get
            {
                return _check
                    ?? (_check = new RelayCommand(CheckMethod));
            }
        }
        private void CheckMethod()
        {
            try
            {
                 SelectedBills = Bills.Where(w => w.Bill.IsDeleted == true).Count();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _delete;
        public RelayCommand Delete
        {
            get
            {
                return _delete
                    ?? (_delete = new RelayCommand(DeleteMethodAsync, CanExecuteDelete));
            }
        }
        private async void DeleteMethodAsync()
        {
            try
            {
                if (Bills.Where(w => w.Bill.IsDeleted == true).Count() == 0)
                    return;

                MessageDialogResult result = await currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هذه الفواتير؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    NegativeButtonText = "الغاء",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
                if (result == MessageDialogResult.Affirmative)
                {
                    using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                    {
                        foreach (var item in Bills.Where(w => w.Bill.IsDeleted == true))
                        {
                            item.Bill.IsDeleted = true;
                            unitOfWork.Bills.Edit(item.Bill);
                        }
                        unitOfWork.Complete();
                    }

                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteDelete()
        {
            if (Bills == null || Bills.Where(w => w.Bill.IsDeleted == true).Count() == 0)
                return false;
            else
                return true;
        }

    }
}
