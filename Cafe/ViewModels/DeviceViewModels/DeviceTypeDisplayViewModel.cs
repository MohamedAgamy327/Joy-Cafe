using BLL.UnitOfWorkService;
using Cafe.Views.DeviceViews;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.DeviceTypeDataModel;
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
    public class DeviceTypeDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly DeviceTypeAddDialog deviceTypeAddDialog;
        private readonly DeviceTypeUpdateDialog deviceTypeUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.DevicesTypes.GetRecordsNumber(_key);
                Paging.GetFirst();
                DevicesTypes = new ObservableCollection<DeviceTypeDisplayDataModel>(unitOfWork.DevicesTypes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public DeviceTypeDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            deviceTypeAddDialog = new DeviceTypeAddDialog();
            deviceTypeUpdateDialog = new DeviceTypeUpdateDialog();
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

        private DeviceTypeDisplayDataModel _selectedDeviceType;
        public DeviceTypeDisplayDataModel SelectedDeviceType
        {
            get { return _selectedDeviceType; }
            set { SetProperty(ref _selectedDeviceType, value); }
        }

        private DeviceTypeAddDataModel _newDeviceType;
        public DeviceTypeAddDataModel NewDeviceType
        {
            get { return _newDeviceType; }
            set { SetProperty(ref _newDeviceType, value); }
        }

        private DeviceTypeUpdateDataModel _deviceTypeUpdate;
        public DeviceTypeUpdateDataModel DeviceTypeUpdate
        {
            get { return _deviceTypeUpdate; }
            set { SetProperty(ref _deviceTypeUpdate, value); }
        }

        private ObservableCollection<DeviceTypeDisplayDataModel> _devicesTypes;
        public ObservableCollection<DeviceTypeDisplayDataModel> DevicesTypes
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
                    DevicesTypes = new ObservableCollection<DeviceTypeDisplayDataModel>(unitOfWork.DevicesTypes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    DevicesTypes = new ObservableCollection<DeviceTypeDisplayDataModel>(unitOfWork.DevicesTypes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.DevicesTypes.Remove(_selectedDeviceType.DeviceType);
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
                NewDeviceType = new DeviceTypeAddDataModel();
                deviceTypeAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(deviceTypeAddDialog);
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
                if (NewDeviceType.Name == null || NewDeviceType.SingleHourPrice == null || NewDeviceType.MultiHourPrice == null || (NewDeviceType.Birthday == true && NewDeviceType.BirthdayHourPrice == null))
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var deviceType = unitOfWork.DevicesTypes.SingleOrDefault(s => s.Name == _newDeviceType.Name);

                    if (deviceType != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالنوع موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        if (NewDeviceType.Birthday == false)
                        {
                            NewDeviceType.BirthdayHourPrice = null;
                            NewDeviceType.BirthdayMinutePrice = null;
                        }
                        unitOfWork.DevicesTypes.Add(new DeviceType
                        {
                            Birthday = _newDeviceType.Birthday,
                            BirthdayHourPrice = _newDeviceType.BirthdayHourPrice,
                            BirthdayMinutePrice = _newDeviceType.BirthdayMinutePrice,
                            MultiHourPrice = _newDeviceType.MultiHourPrice,
                            MultiMinutePrice = _newDeviceType.MultiMinutePrice,
                            Name = _newDeviceType.Name,
                            SingleHourPrice = _newDeviceType.SingleHourPrice,
                            SingleMinutePrice = _newDeviceType.SingleMinutePrice
                        });
                        unitOfWork.Complete();
                        NewDeviceType = new DeviceTypeAddDataModel();
                        Load();
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
            if (NewDeviceType.HasErrors || (NewDeviceType.Birthday && NewDeviceType.BirthdayHourPrice == null))
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
                deviceTypeUpdateDialog.DataContext = this;
                DeviceTypeUpdate = new DeviceTypeUpdateDataModel
                {
                    ID = _selectedDeviceType.DeviceType.ID,
                    Birthday = _selectedDeviceType.DeviceType.Birthday,
                    BirthdayHourPrice = _selectedDeviceType.DeviceType.BirthdayHourPrice,
                    BirthdayMinutePrice = _selectedDeviceType.DeviceType.BirthdayMinutePrice,
                    MultiHourPrice = _selectedDeviceType.DeviceType.MultiHourPrice,
                    MultiMinutePrice = _selectedDeviceType.DeviceType.MultiMinutePrice,
                    Name = _selectedDeviceType.DeviceType.Name,
                    SingleHourPrice = _selectedDeviceType.DeviceType.SingleHourPrice,
                    SingleMinutePrice = _selectedDeviceType.DeviceType.SingleMinutePrice
                };
                await currentWindow.ShowMetroDialogAsync(deviceTypeUpdateDialog);
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
                if (DeviceTypeUpdate.Name == null || DeviceTypeUpdate.SingleHourPrice == null || DeviceTypeUpdate.MultiHourPrice == null || (DeviceTypeUpdate.Birthday == true && DeviceTypeUpdate.BirthdayHourPrice == null))
                    return;
                if (DeviceTypeUpdate.Birthday == false)
                {
                    DeviceTypeUpdate.BirthdayHourPrice = null;
                    DeviceTypeUpdate.BirthdayMinutePrice = null;
                }
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var deviceType = unitOfWork.DevicesTypes.SingleOrDefault(s => s.Name == _deviceTypeUpdate.Name && s.ID != _deviceTypeUpdate.ID);

                    if (deviceType != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالنوع موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {

                        SelectedDeviceType.DeviceType.Birthday = _deviceTypeUpdate.Birthday;
                        SelectedDeviceType.DeviceType.BirthdayHourPrice = _deviceTypeUpdate.BirthdayHourPrice;
                        SelectedDeviceType.DeviceType.BirthdayMinutePrice = _deviceTypeUpdate.BirthdayMinutePrice;
                        SelectedDeviceType.DeviceType.MultiHourPrice = _deviceTypeUpdate.MultiHourPrice;
                        SelectedDeviceType.DeviceType.MultiMinutePrice = _deviceTypeUpdate.MultiMinutePrice;
                        SelectedDeviceType.DeviceType.Name = _deviceTypeUpdate.Name;
                        SelectedDeviceType.DeviceType.SingleHourPrice = _deviceTypeUpdate.SingleHourPrice;
                        SelectedDeviceType.DeviceType.SingleMinutePrice = _deviceTypeUpdate.SingleMinutePrice;
                        unitOfWork.DevicesTypes.Edit(SelectedDeviceType.DeviceType);
                        unitOfWork.Complete();
                        await currentWindow.HideMetroDialogAsync(deviceTypeUpdateDialog);
                        Load();
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
                if (DeviceTypeUpdate.HasErrors || (DeviceTypeUpdate.Birthday && DeviceTypeUpdate.BirthdayHourPrice == null))
                    return false;
                else
                    return true;
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
                        await currentWindow.HideMetroDialogAsync(deviceTypeAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(deviceTypeUpdateDialog);
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
