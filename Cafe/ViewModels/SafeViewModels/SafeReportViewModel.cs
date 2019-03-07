using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
                Paging.TotalIncome = unitOfWork.Safes.GetTotalIncome(_key, _dateFrom, _dateTo);
                Paging.TotalOutgoings = unitOfWork.Safes.GetTotalOutgoings(_key, _dateFrom, _dateTo);
            }
        }

        public SafeReportViewModel()
        {
            _key = "";        
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new SafeReportDataModel();    
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

        private SafeReportDataModel _paging;
        public SafeReportDataModel Paging
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
                    Paging.CurrentAccount = unitOfWork.Safes.GetCurrentAccount();
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
