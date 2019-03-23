using BLL.UnitOfWorkService;
using Cafe.Views.MembershipViews;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.ClientMembershipDataModel;
using DTO.UserDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Utilities.Paging;

namespace Cafe.ViewModels.MembershipViewModels
{
    public class ClientMembershipViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly ClientMembershipAddDialog clientMembershipAddDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.ClientsMemberships.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                ClientsMemberships = new ObservableCollection<ClientMembershipDisplayDataModel>(unitOfWork.ClientsMemberships.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ClientMembershipViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            clientMembershipAddDialog = new ClientMembershipAddDialog();
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());                   
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
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

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private Membership _selectedMembership;
        public Membership SelectedMembership
        {
            get { return _selectedMembership; }
            set { SetProperty(ref _selectedMembership, value); }
        }

        private ClientMembershipAddDataModel _newClientMembership;
        public ClientMembershipAddDataModel NewClientMembership
        {
            get { return _newClientMembership; }
            set { SetProperty(ref _newClientMembership, value); }
        }

        private ClientMembershipDisplayDataModel _selectedClientMembership;
        public ClientMembershipDisplayDataModel SelectedClientMembership
        {
            get { return _selectedClientMembership; }
            set { SetProperty(ref _selectedClientMembership, value); }
        }

        private DeviceType _selectedDeviceType;
        public DeviceType SelectedDeviceType
        {
            get { return _selectedDeviceType; }
            set { SetProperty(ref _selectedDeviceType, value); }
        }

        private ObservableCollection<DeviceType> _devicesTypes;
        public ObservableCollection<DeviceType> DevicesTypes
        {
            get { return _devicesTypes; }
            set { SetProperty(ref _devicesTypes, value); }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        private ObservableCollection<Membership> _memberships;
        public ObservableCollection<Membership> Memberships
        {
            get { return _memberships; }
            set { SetProperty(ref _memberships, value); }
        }

        private ObservableCollection<ClientMembershipDisplayDataModel> _clientsMemberships;
        public ObservableCollection<ClientMembershipDisplayDataModel> ClientsMemberships
        {
            get { return _clientsMemberships; }
            set { SetProperty(ref _clientsMemberships, value); }
        }

        //Display


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
                    _clients = new ObservableCollection<Client>(unitOfWork.Clients.Find(f => f.Code != null).OrderBy(o => o.Name));
                    _devicesTypes = new ObservableCollection<DeviceType>(unitOfWork.DevicesTypes.GetAll().OrderBy(o => o.Name));
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
                    ClientsMemberships = new ObservableCollection<ClientMembershipDisplayDataModel>(unitOfWork.ClientsMemberships.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    ClientsMemberships = new ObservableCollection<ClientMembershipDisplayDataModel>(unitOfWork.ClientsMemberships.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
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
                    ?? (_delete = new RelayCommand(DeleteMethod));
            }
        }
        private void DeleteMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.ClientsMemberships.Remove(_selectedClientMembership.ClientMembership);
                    unitOfWork.Safes.Remove(unitOfWork.Safes.SingleOrDefault(w => w.RegistrationDate == _selectedClientMembership.ClientMembership.RegistrationDate));
                    var clientMembershipMinute = unitOfWork.ClientMembershipMinutes.SingleOrDefault(s => s.DeviceTypeID == _selectedClientMembership.Membership.DeviceTypeID && s.ClientID == _selectedClientMembership.Client.ID);
                    clientMembershipMinute.Minutes -= (int)_selectedClientMembership.Membership.Minutes;
                    unitOfWork.ClientMembershipMinutes.Edit(clientMembershipMinute);
                    unitOfWork.Complete();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Add

        private RelayCommand _showAdd;
        public RelayCommand ShowAdd
        {
            get
            {
                return _showAdd
                    ?? (_showAdd = new RelayCommand(ShowAddMethod));
            }
        }
        private async void ShowAddMethod()
        {
            try
            {
                NewClientMembership = new ClientMembershipAddDataModel();
                clientMembershipAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(clientMembershipAddDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _getMemberships;
        public RelayCommand GetMemberships
        {
            get
            {
                return _getMemberships
                    ?? (_getMemberships = new RelayCommand(GetMembershipsMethod));
            }
        }
        private void GetMembershipsMethod()
        {
            try
            {
                try
                {
                    using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                    {
                        Memberships = new ObservableCollection<Membership>(unitOfWork.Memberships.Find(f => f.DeviceTypeID == _selectedDeviceType.ID && f.IsAvailable == true));
                    }

                }
                catch (Exception)
                {
                    Memberships = new ObservableCollection<Membership>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ?? (_save = new RelayCommand(
                    ExecuteSave,
                    CanExecuteSave));
            }
        }
        private void ExecuteSave()
        {
            try
            {
                if (SelectedClient == null || SelectedMembership == null || SelectedDeviceType == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    DateTime dt = DateTime.Now;
                    var clientMembershipMinute = unitOfWork.ClientMembershipMinutes.SingleOrDefault(s => s.DeviceTypeID == _selectedDeviceType.ID && s.ClientID == _selectedClient.ID);
                    if (clientMembershipMinute == null)
                    {
                        ClientMembershipMinute cmm = new ClientMembershipMinute
                        {
                            ClientID = _selectedClient.ID,
                            DeviceTypeID = _selectedMembership.DeviceTypeID,
                            Minutes = (int)_selectedMembership.Minutes
                        };
                        unitOfWork.ClientMembershipMinutes.Add(cmm);
                    }
                    else
                    {
                        clientMembershipMinute.Minutes += (int)_selectedMembership.Minutes;
                        unitOfWork.ClientMembershipMinutes.Edit(clientMembershipMinute);
                    }

                    Safe safe = new Safe
                    {
                        Amount = _selectedMembership.Price,
                        CanDelete = false,
                        RegistrationDate = dt,
                        UserID = UserData.ID,
                        Type = true,
                        Statement = "اشتراك فى العضوية " + _selectedMembership.Name + " ل " + _selectedDeviceType.Name + " للعميل " + _selectedClient.Name
                    };
                    unitOfWork.Safes.Add(safe);

                    ClientMembership cm = new ClientMembership
                    {
                        Date = dt,
                        RegistrationDate = dt,
                        Price = _selectedMembership.Price,
                        UserID = UserData.ID,
                        MembershipID = _newClientMembership.MembershipID,
                        ClientID = _newClientMembership.ClientID
                    };
                    unitOfWork.ClientsMemberships.Add(cm);
                    unitOfWork.Complete();
                    NewClientMembership = new ClientMembershipAddDataModel();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSave()
        {
            try
            {
                if (SelectedClient != null && SelectedMembership != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
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
                    case "Add":
                        await currentWindow.HideMetroDialogAsync(clientMembershipAddDialog);
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
