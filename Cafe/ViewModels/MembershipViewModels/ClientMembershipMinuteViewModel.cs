using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DTO.ClientMembershipMinutesDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Utilities.Paging;

namespace Cafe.ViewModels.MembershipViewModels
{
    public class ClientMembershipMinuteViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.ClientMembershipMinutes.GetRecordsNumber(_key);
                Paging.GetFirst();
                ClientMembershipMinutes = new ObservableCollection<ClientMembershipMinutesDisplayDataModel>(unitOfWork.ClientMembershipMinutes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ClientMembershipMinuteViewModel()
        {
            _key = "";
            _paging = new PagingWPF();
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

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ObservableCollection<ClientMembershipMinutesDisplayDataModel> _clientMembershipMinutes;
        public ObservableCollection<ClientMembershipMinutesDisplayDataModel> ClientMembershipMinutes
        {
            get { return _clientMembershipMinutes; }
            set { SetProperty(ref _clientMembershipMinutes, value); }
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
                    ClientMembershipMinutes = new ObservableCollection<ClientMembershipMinutesDisplayDataModel>(unitOfWork.ClientMembershipMinutes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    ClientMembershipMinutes = new ObservableCollection<ClientMembershipMinutesDisplayDataModel>(unitOfWork.ClientMembershipMinutes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
