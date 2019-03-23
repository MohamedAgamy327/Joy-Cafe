using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using Cafe.Views.ShiftViews;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL.Entities;
using DAL;
using Utilities.Paging;
using DTO.ShiftDataModel;

namespace Cafe.ViewModels.ShiftViewModels
{
    public class ShiftDisplayViewModel : ValidatableBindableBase
    {
        private MetroWindow currentWindow;
        private readonly ShiftShowDialog shiftShowDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Shifts.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ShiftDisplayViewModel()
        {
            _key = "";
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new PagingWPF();
            shiftShowDialog = new ShiftShowDialog();          
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                SetProperty(ref _key, value);
            }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ShiftDisplayDataModel _selectedShift;
        public ShiftDisplayDataModel SelectedShift
        {
            get { return _selectedShift; }
            set { SetProperty(ref _selectedShift, value); }
        }

        private ObservableCollection<ShiftDisplayDataModel> _shifts;
        public ObservableCollection<ShiftDisplayDataModel> Shifts
        {
            get { return _shifts; }
            set { SetProperty(ref _shifts, value); }
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

        private RelayCommand _next;
        public RelayCommand Next
        {
            get
            {
                return _next
                    ?? (_next = new RelayCommand(NextMethod));
            }
        }
        private void NextMethod()
        {
            try
            {
                Paging.Next();
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _previous;
        public RelayCommand Previous
        {
            get
            {
                return _previous
                    ?? (_previous = new RelayCommand(PreviousMethod));
            }
        }
        private void PreviousMethod()
        {
            try
            {
                Paging.Previous();
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _show;
        public RelayCommand Show
        {
            get
            {
                return _show
                    ?? (_show = new RelayCommand(ShowMethod));
            }
        }
        private async void ShowMethod()
        {
            try
            {
                shiftShowDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(shiftShowDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<string> _closeDialog;
        public RelayCommand<string> CloseDialog
        {
            get
            {
                return _closeDialog
                    ?? (_closeDialog = new RelayCommand<string>(ExecuteCloseDialogAsync));
            }
        }
        private async void ExecuteCloseDialogAsync(string parameter)
        {
            try
            {
                switch (parameter)
                {
                    case "show":
                        await currentWindow.HideMetroDialogAsync(shiftShowDialog);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
