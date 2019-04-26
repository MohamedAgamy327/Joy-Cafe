using BLL.UnitOfWorkService;
using Cafe.Views.DeviceViews;
using DAL;
using DAL.BindableBaseService;
using DAL.ConstString;
using DAL.Entities;
using DTO.DeviceDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Utilities.Paging;

namespace Cafe.ViewModels.DeviceViewModels
{
    public class DeviceDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly DeviceAddDialog deviceAddDialog;
        private readonly DeviceUpdateDialog deviceUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Devices.GetRecordsNumber(_key);
                Paging.GetFirst();
                Devices = new ObservableCollection<DeviceDisplayDataModel>(unitOfWork.Devices.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public DeviceDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            deviceAddDialog = new DeviceAddDialog();
            deviceUpdateDialog = new DeviceUpdateDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private bool _notBusy;
        public bool NotBusy
        {
            get { return _notBusy; }
            set { SetProperty(ref _notBusy, value); }
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

        private DeviceDisplayDataModel _selectedDevice;
        public DeviceDisplayDataModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { SetProperty(ref _selectedDevice, value); }
        }

        private DeviceType _selectedDeviceType;
        public DeviceType SelectedDeviceType
        {
            get { return _selectedDeviceType; }
            set { SetProperty(ref _selectedDeviceType, value); }
        }

        private DeviceAddDataModel _newDevice;
        public DeviceAddDataModel NewDevice
        {
            get { return _newDevice; }
            set { SetProperty(ref _newDevice, value); }
        }

        private DeviceUpdateDataModel _deviceUpdate;
        public DeviceUpdateDataModel DeviceUpdate
        {
            get { return _deviceUpdate; }
            set { SetProperty(ref _deviceUpdate, value); }
        }

        private ObservableCollection<DeviceDisplayDataModel> _devices;
        public ObservableCollection<DeviceDisplayDataModel> Devices
        {
            get { return _devices; }
            set { SetProperty(ref _devices, value); }
        }

        private ObservableCollection<DeviceType> _devicesTypes;
        public ObservableCollection<DeviceType> DevicesTypes
        {
            get { return _devicesTypes; }
            set { SetProperty(ref _devicesTypes, value); }
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
                    Devices = new ObservableCollection<DeviceDisplayDataModel>(unitOfWork.Devices.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Devices = new ObservableCollection<DeviceDisplayDataModel>(unitOfWork.Devices.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Devices.Remove(_selectedDevice.Device);
                    unitOfWork.Complete();
                }
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Add

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
                NewDevice = new DeviceAddDataModel();
                deviceAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(deviceAddDialog);
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
                if (NewDevice.Name == null || SelectedDeviceType == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var device = unitOfWork.Devices.SingleOrDefault(s => s.Name == _newDevice.Name && s.DeviceTypeID == _newDevice.DeviceTypeID);
                    if (device != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالجهاز موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        return;
                    }
                    device = unitOfWork.Devices.SingleOrDefault(s => s.Order == _newDevice.Order);
                    if (device != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذا الترتيب موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        return;
                    }
                    unitOfWork.Devices.Add(new Device
                    {
                        IsAvailable = true,
                        Name = _newDevice.Name,
                        DeviceTypeID = _newDevice.DeviceTypeID,
                        Order = _newDevice.Order,
                        Case = DeviceCaseText.Free
                    });
                    unitOfWork.Complete();
                    NewDevice = new DeviceAddDataModel();
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
            if (NewDevice.HasErrors || SelectedDeviceType == null)
                return false;
            else
                return true;
        }

        // Update

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
                DeviceUpdate = new DeviceUpdateDataModel();
                DeviceUpdate.ID = _selectedDevice.Device.ID;
                DeviceUpdate.Name = _selectedDevice.Device.Name;
                DeviceUpdate.Order = _selectedDevice.Device.Order;
                DeviceUpdate.DeviceTypeID = _selectedDevice.Device.DeviceTypeID;
                DeviceUpdate.DeviceType = _selectedDevice.DeviceType;
                DeviceUpdate.IsAvailable = _selectedDevice.Device.IsAvailable;
                NotBusy = SelectedDevice.Device.Case == DeviceCaseText.Free ? true : false;
                deviceUpdateDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(deviceUpdateDialog);
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
            try
            {
                if (DeviceUpdate.Name == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var device = unitOfWork.Devices.SingleOrDefault(s => s.Name == _deviceUpdate.Name && s.DeviceTypeID == _deviceUpdate.DeviceTypeID && s.ID != _deviceUpdate.ID);
                    if (device != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالجهاز موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        return;
                    }
                     device = unitOfWork.Devices.SingleOrDefault(s => s.Order == _deviceUpdate.Order && s.ID != _deviceUpdate.ID);
                    if (device != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذا الترتيب موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        return;
                    }

                    SelectedDevice.Device.Name = _deviceUpdate.Name;
                    SelectedDevice.Device.IsAvailable = _deviceUpdate.IsAvailable;
                    SelectedDevice.Device.Order = _deviceUpdate.Order;
                    unitOfWork.Devices.Edit(SelectedDevice.Device);
                    unitOfWork.Complete();
                    await currentWindow.HideMetroDialogAsync(deviceUpdateDialog);
                    Load();

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
                return !DeviceUpdate.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(deviceAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(deviceUpdateDialog);
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
