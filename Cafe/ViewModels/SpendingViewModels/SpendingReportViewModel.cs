using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using DAL.BindableBaseService;
using DTO.SpendingDataModel;
using BLL.UnitOfWorkService;
using DAL;
using Utilities.Paging;

namespace Cafe.ViewModels.SpendingViewModels
{
    public class SpendingReportViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Spendings.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                Paging.TotalAmount = unitOfWork.Spendings.GetTotalAmount(_key, _dateFrom, _dateTo);
            }
        }

        public SpendingReportViewModel()
        {
            _key = "";
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new SpendingReportDataModel();
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

        private SpendingReportDataModel _paging;
        public SpendingReportDataModel Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ObservableCollection<SpendingDisplayDataModel> _spendings;
        public ObservableCollection<SpendingDisplayDataModel> Spendings
        {
            get { return _spendings; }
            set { SetProperty(ref _spendings, value); }
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
