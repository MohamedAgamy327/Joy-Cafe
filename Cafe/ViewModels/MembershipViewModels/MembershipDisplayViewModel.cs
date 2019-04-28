using BLL.UnitOfWorkService;
using Cafe.Views.MembershipViews;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.MembershipDataModel;
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
    public class MembershipDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly MembershipAddDialog membershipAddDialog;
        private readonly MembershipUpdateDialog membershipUpdateDialog;

        private void Load(bool isNew)
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Memberships.GetRecordsNumber(_key);
                Paging.GetFirst();
                Memberships = new ObservableCollection<MembershipDisplayDataModel>(unitOfWork.Memberships.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public MembershipDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            membershipAddDialog = new MembershipAddDialog();
            membershipUpdateDialog = new MembershipUpdateDialog();
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
            set { SetProperty(ref _key, value); }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private MembershipDisplayDataModel _selectedMembership;
        public MembershipDisplayDataModel SelectedMembership
        {
            get { return _selectedMembership; }
            set { SetProperty(ref _selectedMembership, value); }
        }

        private MembershipUpdateDataModel _membershipUpdate;
        public MembershipUpdateDataModel MembershipUpdate
        {
            get { return _membershipUpdate; }
            set { SetProperty(ref _membershipUpdate, value); }
        }

        private MembershipAddDataModel _newMembership;
        public MembershipAddDataModel NewMembership
        {
            get { return _newMembership; }
            set { SetProperty(ref _newMembership, value); }
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

        private ObservableCollection<MembershipDisplayDataModel> _memberships;
        public ObservableCollection<MembershipDisplayDataModel> Memberships
        {
            get { return _memberships; }
            set { SetProperty(ref _memberships, value); }
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
                    _devicesTypes = new ObservableCollection<DeviceType>(unitOfWork.DevicesTypes.GetAll());
                }
                Load(true);
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
                Load(true);
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
                    Memberships = new ObservableCollection<MembershipDisplayDataModel>(unitOfWork.Memberships.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Memberships = new ObservableCollection<MembershipDisplayDataModel>(unitOfWork.Memberships.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Memberships.Remove(_selectedMembership.Membership);
                    unitOfWork.Complete();
                }
                Load(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Add Membership

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
                NewMembership = new MembershipAddDataModel();
                membershipAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(membershipAddDialog);
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
                    ExecuteSaveAsync,
                    CanExecuteSave));
            }
        }
        private async void ExecuteSaveAsync()
        {
            try
            {
                if (NewMembership.Minutes == null || NewMembership.Name == null || NewMembership.Price == null || SelectedDeviceType.Name == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var membership = unitOfWork.Memberships.SingleOrDefault(s => s.Name == _newMembership.Name && s.DeviceTypeID == _newMembership.DeviceTypeID);
                    if (membership != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذه العضوية موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        unitOfWork.Memberships.Add(new Membership
                        {
                            IsAvailable = true,
                            Name = _newMembership.Name,
                            DeviceTypeID = _newMembership.DeviceTypeID,
                            Minutes = _newMembership.Minutes,
                            Price = _newMembership.Price
                        });
                        unitOfWork.Complete();
                        NewMembership = new MembershipAddDataModel();
                        Load(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSave()
        {
            return !NewMembership.HasErrors && SelectedDeviceType != null;
        }

        // Update Account

        private RelayCommand _showUpdate;
        public RelayCommand ShowUpdate
        {
            get
            {
                return _showUpdate
                    ?? (_showUpdate = new RelayCommand(ShowUpdateMethod));
            }
        }
        private async void ShowUpdateMethod()
        {
            try
            {
                membershipUpdateDialog.DataContext = this;
                MembershipUpdate = new MembershipUpdateDataModel
                {
                    ID = _selectedMembership.Membership.ID,
                    Name = _selectedMembership.Membership.Name,
                    DeviceTypeID = _selectedMembership.Membership.DeviceTypeID,
                    Price = _selectedMembership.Membership.Price,
                    Minutes = _selectedMembership.Membership.Minutes,
                    DeviceType = _selectedMembership.DeviceType,
                    IsAvailable = _selectedMembership.Membership.IsAvailable
                };
                await currentWindow.ShowMetroDialogAsync(membershipUpdateDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _update;
        public RelayCommand Update
        {
            get
            {
                return _update ?? (_update = new RelayCommand(
                    ExecuteUpdateAsync,
                    CanExecuteUpdate));
            }
        }
        private async void ExecuteUpdateAsync()
        {
            if (SelectedMembership.Membership.Name == null || SelectedMembership.Membership.Price == null)
                return;
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var membership = unitOfWork.Devices.SingleOrDefault(s => s.Name == _membershipUpdate.Name && s.DeviceTypeID == _membershipUpdate.DeviceTypeID && s.ID != _membershipUpdate.ID);
                    if (membership != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذه العضوية موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        SelectedMembership.Membership.Name = _membershipUpdate.Name;
                        SelectedMembership.Membership.IsAvailable = _membershipUpdate.IsAvailable;
                        SelectedMembership.Membership.Price = _membershipUpdate.Price;
                        unitOfWork.Memberships.Edit(SelectedMembership.Membership);
                        unitOfWork.Complete();
                        await currentWindow.HideMetroDialogAsync(membershipUpdateDialog);
                        Load(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteUpdate()
        {
            try
            {
                return !MembershipUpdate.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(membershipAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(membershipUpdateDialog);
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
