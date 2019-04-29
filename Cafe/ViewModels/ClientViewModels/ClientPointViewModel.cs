using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DTO.ClientDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Utilities.Paging;

namespace Cafe.ViewModels.ClientViewModels
{
    public class ClientPointViewModel : ValidatableBindableBase
    {
        void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Clients.GetRecordsNumber(_key);
                Paging.GetFirst();
                Clients = new ObservableCollection<ClientPointDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize, DateFrom, DateTo));
            }
        }

        public ClientPointViewModel()
        {
            _key = "";
            _paging = new PagingWPF();
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
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

        private ObservableCollection<ClientPointDataModel> _clients;
        public ObservableCollection<ClientPointDataModel> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
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
                    Clients = new ObservableCollection<ClientPointDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize, _dateFrom, _dateTo));
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
                    Clients = new ObservableCollection<ClientPointDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize, _dateFrom, _dateTo));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
