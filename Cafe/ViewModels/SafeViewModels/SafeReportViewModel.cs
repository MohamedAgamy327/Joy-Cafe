using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using DAL.BindableBaseService;
using Utilities.Paging;
using BLL.UnitOfWorkService;
using DAL;
using DTO.SafeDataModel;

namespace Cafe.ViewModels.SafeViewModels
{
    public class SafeReportViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Safes.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                TotalIncome = unitOfWork.Safes.GetTotalIncome(_key, _dateFrom, _dateTo);
                TotalOutgoings = unitOfWork.Safes.GetTotalOutgoings(_key, _dateFrom, _dateTo);
            }
        }

        public SafeReportViewModel()
        {
            _key = "";
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new PagingWPF();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private decimal? _currentAccount;
        public decimal? CurrentAccount
        {
            get { return _currentAccount; }
            set { SetProperty(ref _currentAccount, value); }
        }

        private decimal? _totalIncome;
        public decimal? TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                SetProperty(ref _totalIncome, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _totalOutgoings;
        public decimal? TotalOutgoings
        {
            get { return _totalOutgoings; }
            set
            {
                SetProperty(ref _totalOutgoings, value);
                OnPropertyChanged("Difference");
            }
        }

        private decimal? _difference;
        public decimal? Difference
        {
            get { return _difference = _totalIncome + _totalOutgoings; }
            set { SetProperty(ref _difference, value); }
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

        private ObservableCollection<SafeDisplayDataModel> _safes;
        public ObservableCollection<SafeDisplayDataModel> Safes
        {
            get { return _safes; }
            set { SetProperty(ref _safes, value); }
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
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    CurrentAccount = unitOfWork.Safes.GetCurrentAccount();
                }
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
                    Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
