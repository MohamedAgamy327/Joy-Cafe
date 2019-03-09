using BLL.UnitOfWorkService;
using Cafe.Views.BillViews;
using Cafe.Views.BillViews.BillItemsViews;
using Cafe.Views.BillViews.ShiftItemsViews;
using Cafe.Views.BillViews.ShiftSpendingViews;
using DAL;
using DAL.BindableBaseService;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using DTO.BillItemDataModel;
using DTO.DeviceDataModel;
using DTO.UserDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cafe.ViewModels.BillViewModels
{
    public class DevicesViewModel : ValidatableBindableBase
    {
        Bill _bill;
        Bill _itemsBill;
        DateTime _endDate;
        BillDevice _billDevice;
        DateTime _finishShiftDate;
        MetroWindow _currentWindow;

        private readonly ClientAddDialog _clientAddDialog;
        private readonly ClientCheckDialog _clientCheckDialog;
        private readonly AccountPaidDialog _accountPaidDialog;
        private readonly FinishShiftDialog _finishShiftDialog;

        public DevicesViewModel()
        {
            _isFocused = true;
            _accountVisibility = VisibilityText.Collapsed;
            _freeDevicesVisibility = VisibilityText.Collapsed;
            _availableDevicesVisibility = VisibilityText.Visible;
            _clientAddDialog = new ClientAddDialog();
            _clientCheckDialog = new ClientCheckDialog();
            _accountPaidDialog = new AccountPaidDialog();
            _finishShiftDialog = new FinishShiftDialog();
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private string _accountVisibility;
        public string AccountVisibility
        {
            get { return _accountVisibility; }
            set { SetProperty(ref _accountVisibility, value); }
        }

        private string _freeDevicesVisibility;
        public string FreeDevicesVisibility
        {
            get { return _freeDevicesVisibility; }
            set { SetProperty(ref _freeDevicesVisibility, value); }
        }

        private string _availableDevicesVisibility;
        public string AvailableDevicesVisibility
        {
            get { return _availableDevicesVisibility; }
            set { SetProperty(ref _availableDevicesVisibility, value); }
        }

        private string _isMembership;
        public string IsMembership
        {
            get { return _isMembership; }
            set { SetProperty(ref _isMembership, value); }
        }

        private string _isStart;
        public string IsStart
        {
            get { return _isStart; }
            set { SetProperty(ref _isStart, value); }
        }

        private string _isStop;
        public string IsStop
        {
            get { return _isStop; }
            set { SetProperty(ref _isStop, value); }
        }

        private string _isBirthday;
        public string IsBirthday
        {
            get { return _isBirthday; }
            set { SetProperty(ref _isBirthday, value); }
        }

        private string _isSingle;
        public string IsSingle
        {
            get { return _isSingle; }
            set { SetProperty(ref _isSingle, value); }
        }

        private string _isMulti;
        public string IsMulti
        {
            get { return _isMulti; }
            set { SetProperty(ref _isMulti, value); }
        }

        private string _isTemporary;
        public string IsTemporary
        {
            get { return _isTemporary; }
            set { SetProperty(ref _isTemporary, value); }
        }

        private string _isResume;
        public string IsResume
        {
            get { return _isResume; }
            set { SetProperty(ref _isResume, value); }
        }

        private int _busyDevices;
        public int BusyDevices
        {
            get { return _busyDevices; }
            set { SetProperty(ref _busyDevices, value); }
        }

        private int _temporaryDevices;
        public int TemporaryDevices
        {
            get { return _temporaryDevices; }
            set { SetProperty(ref _temporaryDevices, value); }
        }

        private int _availableDevices;
        public int AvailableDevices
        {
            get { return _availableDevices; }
            set { SetProperty(ref _availableDevices, value); }
        }

        private decimal _devicesSum;
        public decimal DevicesSum
        {
            get
            {
                if (BillDevices != null)
                    return _devicesSum = BillDevices.Sum(s => Convert.ToDecimal(s.Total));
                else
                    return 0;
            }
        }

        private decimal _itemsSum;
        public decimal ItemsSum
        {
            get
            {
                if (BillItems != null)
                    return _itemsSum = BillItems.Sum(s => Convert.ToDecimal(s.BillItem.Total));
                else
                    return 0;
            }
        }

        private decimal _totalSum;
        public decimal TotalSum
        {
            get { return _totalSum = ItemsSum + DevicesSum; }
        }

        private Shift _newShift;
        public Shift NewShift
        {
            get { return _newShift; }
            set { SetProperty(ref _newShift, value); }
        }

        private Client _newClient;
        public Client NewClient
        {
            get { return _newClient; }
            set { SetProperty(ref _newClient, value); }
        }

        private Bill _selectBill;
        public Bill SelectBill
        {
            get { return _selectBill; }
            set { SetProperty(ref _selectBill, value); }
        }

        private DevicePlayDataModel _selectedDevice;
        public DevicePlayDataModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { SetProperty(ref _selectedDevice, value); }
        }

        private BillDevice _newBillDevice;
        public BillDevice NewBillDevice
        {
            get { return _newBillDevice; }
            set { SetProperty(ref _newBillDevice, value); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        //private FinishShiftVM _finishShiftVM;
        //public FinishShiftVM FinishShiftVM
        //{
        //    get { return _finishShiftVM; }
        //    set { SetProperty(ref _finishShiftVM, value); }
        //}

        private List<string> _telephoneSuggestions;
        public List<string> TelephoneSuggestions
        {
            get { return _telephoneSuggestions; }
            set { SetProperty(ref _telephoneSuggestions, value); }
        }

        private ObservableCollection<DeviceFreeDataModel> _freeDevices;
        public ObservableCollection<DeviceFreeDataModel> FreeDevices
        {
            get { return _freeDevices; }
            set { SetProperty(ref _freeDevices, value); }
        }

        private ObservableCollection<DevicePlayDataModel> _devices;
        public ObservableCollection<DevicePlayDataModel> Devices
        {
            get { return _devices; }
            set { SetProperty(ref _devices, value); }
        }

        private ObservableCollection<BillItemsDisplayDataModel> _billItems;
        public ObservableCollection<BillItemsDisplayDataModel> BillItems
        {
            get { return _billItems; }
            set
            {
                SetProperty(ref _billItems, value);
                OnPropertyChanged("ItemsSum");
                OnPropertyChanged("TotalSum");
            }
        }

        private ObservableCollection<BillDevicesDisplayDataModel> _billDevices;
        public ObservableCollection<BillDevicesDisplayDataModel> BillDevices
        {
            get { return _billDevices; }
            set
            {
                SetProperty(ref _billDevices, value);
                OnPropertyChanged("DevicesSum");
                OnPropertyChanged("TotalSum");
            }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        // Devices

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
                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                    BusyDevices = _devices.Where(w => w.Device.Case == CaseText.Busy).Count();
                    AvailableDevices = _devices.Where(w => w.Device.Case == CaseText.Free).Count();
                    TemporaryDevices = _devices.Where(w => w.Device.Case == CaseText.Temporary).Count();

                    //_clients = new ObservableCollection<Client>(_clientServ.GetClients());
                    //_freeDevices = new ObservableCollection<DeviceVM>(_deviceServ.GetFreeDevices());
                    //_billItems = new ObservableCollection<BillItem>();
                    //_billDevices = new ObservableCollection<BillDevicesVM>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _check;
        public RelayCommand Check
        {
            get
            {
                return _check
                    ?? (_check = new RelayCommand(CheckMethod));
            }
        }
        private void CheckMethod()
        {
            try
            {
                AccountVisibility = "Collapsed";

                if (SelectedDevice == null)
                    return;

                IsSingle = "Visible";
                IsMulti = "Visible";
                IsBirthday = "Visible";
                if (_selectedDevice.Device.Case != CaseText.Free)
                {
                    IsStart = "Collapsed";
                    IsStop = "Visible";
                }
                else
                {
                    IsStart = "Visible";
                    IsStop = "Collapsed";
                }
                if (_selectedDevice.DeviceType.Birthday && _selectedDevice.GameType != GamePlayTypeText.Birthday)
                    IsBirthday = "Visible";
                else
                    IsBirthday = "Collapsed";

                if (_selectedDevice.GameType == GamePlayTypeText.Single)
                    IsSingle = "Collapsed";
                if (_selectedDevice.GameType == GamePlayTypeText.Multiplayer)
                    IsMulti = "Collapsed";

                if (_selectedDevice.Device.Case == CaseText.Free)
                {
                    IsResume = "Collapsed";
                    IsTemporary = "Collapsed";
                }
                else if (_selectedDevice.Device.Case == CaseText.Temporary)
                {
                    IsResume = "Visible";
                    IsTemporary = "Collapsed";
                }
                else
                {
                    IsResume = "Collapsed";
                    IsTemporary = "Visible";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<string> _start;
        public RelayCommand<string> Start
        {
            get
            {
                return _start
                    ?? (_start = new RelayCommand<string>(ExecuteStart));
            }
        }
        private void ExecuteStart(string parameter)
        {
            try
            {
                if (SelectedDevice == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _bill = new Bill
                    {
                        StartDate = DateTime.Now,
                        Type = GeneralText.Devices
                    };
                    unitOfWork.Bills.Add(_bill);

                    _selectedDevice.Device.Case = CaseText.Busy;
                    _selectedDevice.Device.BillID = _bill.ID;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);

                    switch (parameter)
                    {
                        case GamePlayTypeText.Single:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = _bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Multiplayer:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multiplayer,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = _bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Birthday:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Birthday,
                                MinutePrice = _selectedDevice.DeviceType.BirthdayMinutePrice,
                                BillID = _bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        default:
                            break;
                    }

                    unitOfWork.BillsDevices.Add(_billDevice);
                    unitOfWork.Complete();
                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                    BusyDevices = _devices.Where(w => w.Device.Case == CaseText.Busy).Count();
                    AvailableDevices = _devices.Where(w => w.Device.Case == CaseText.Free).Count();
                    TemporaryDevices = _devices.Where(w => w.Device.Case == CaseText.Temporary).Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<string> _change;
        public RelayCommand<string> Change
        {
            get
            {
                return _change
                    ?? (_change = new RelayCommand<string>(ExecuteChange));
            }
        }
        private void ExecuteChange(string parameter)
        {
            try
            {
                if (SelectedDevice == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    if (_selectedDevice.Device.Case == CaseText.Busy)
                    {
                        _billDevice = unitOfWork.BillsDevices.FirstOrDefault(w => w.BillID == _selectedDevice.Device.BillID && w.EndDate == null);
                        _billDevice.EndDate = DateTime.Now;
                        _billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(_billDevice.EndDate) - _billDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(_billDevice);
                    }
                    else
                    {
                        _selectedDevice.Device.Case = CaseText.Busy;
                        unitOfWork.Devices.Edit(_selectedDevice.Device);
                    }

                    switch (parameter)
                    {
                        case GamePlayTypeText.Single:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Multiplayer:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multiplayer,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Birthday:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Birthday,
                                MinutePrice = _selectedDevice.DeviceType.BirthdayMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        default:
                            break;
                    }

                    unitOfWork.BillsDevices.Add(_billDevice);
                    unitOfWork.Complete();
                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                    BusyDevices = _devices.Where(w => w.Device.Case == CaseText.Busy).Count();
                    AvailableDevices = _devices.Where(w => w.Device.Case == CaseText.Free).Count();
                    TemporaryDevices = _devices.Where(w => w.Device.Case == CaseText.Temporary).Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _temporaryStop;
        public RelayCommand TemporaryStop
        {
            get
            {
                return _temporaryStop
                    ?? (_temporaryStop = new RelayCommand(TemporaryStopMethod));
            }
        }
        private void TemporaryStopMethod()
        {
            try
            {
                if (SelectedDevice == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _selectedDevice.Device.Case = CaseText.Temporary;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);
                    _billDevice = unitOfWork.BillsDevices.FirstOrDefault(s => s.BillID == _selectedDevice.Device.BillID && s.EndDate == null);
                    _billDevice.EndDate = DateTime.Now;
                    _billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(_billDevice.EndDate) - _billDevice.StartDate).TotalMinutes);
                    unitOfWork.BillsDevices.Edit(_billDevice);
                    unitOfWork.Complete();

                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                    BusyDevices = _devices.Where(w => w.Device.Case == CaseText.Busy).Count();
                    AvailableDevices = _devices.Where(w => w.Device.Case == CaseText.Free).Count();
                    TemporaryDevices = _devices.Where(w => w.Device.Case == CaseText.Temporary).Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _resume;
        public RelayCommand Resume
        {
            get
            {
                return _resume
                    ?? (_resume = new RelayCommand(ResumeMethod));
            }
        }
        private void ResumeMethod()
        {
            try
            {
                if (SelectedDevice == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _selectedDevice.Device.Case = CaseText.Busy;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);

                    _billDevice = unitOfWork.BillsDevices.GetLast((int)_selectedDevice.Device.BillID);

                    switch (_billDevice.GameType)
                    {
                        case GamePlayTypeText.Single:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Multiplayer:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multiplayer,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Birthday:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Birthday,
                                MinutePrice = _selectedDevice.DeviceType.BirthdayMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        default:
                            break;
                    }
                    unitOfWork.BillsDevices.Add(_billDevice);
                    unitOfWork.Complete();
                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                    BusyDevices = _devices.Where(w => w.Device.Case == CaseText.Busy).Count();
                    AvailableDevices = _devices.Where(w => w.Device.Case == CaseText.Free).Count();
                    TemporaryDevices = _devices.Where(w => w.Device.Case == CaseText.Temporary).Count();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Move

        private RelayCommand _moveFrom;
        public RelayCommand MoveFrom
        {
            get
            {
                return _moveFrom
                    ?? (_moveFrom = new RelayCommand(MoveFromMethod));
            }
        }
        private void MoveFromMethod()
        {
            try
            {
                if (SelectedDevice == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    FreeDevices = new ObservableCollection<DeviceFreeDataModel>(unitOfWork.Devices.GetFree(_selectedDevice.GameType));
                }
                AvailableDevicesVisibility = VisibilityText.Collapsed;
                FreeDevicesVisibility = VisibilityText.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _cancelMove;
        public RelayCommand CancelMove
        {
            get
            {
                return _cancelMove
                    ?? (_cancelMove = new RelayCommand(CancelMoveMethod));
            }
        }
        private void CancelMoveMethod()
        {
            try
            {
                FreeDevicesVisibility = VisibilityText.Collapsed;
                AvailableDevicesVisibility = VisibilityText.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<DeviceFreeDataModel> _moveTo;
        public RelayCommand<DeviceFreeDataModel> MoveTo
        {
            get
            {
                return _moveTo
                    ?? (_moveTo = new RelayCommand<DeviceFreeDataModel>(ExecuteMoveTo));
            }
        }
        private void ExecuteMoveTo(DeviceFreeDataModel selectedFreeDevice)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    if (_selectedDevice.Device.Case == CaseText.Busy)
                    {
                        _billDevice = unitOfWork.BillsDevices.FirstOrDefault(s => s.BillID == _selectedDevice.Device.BillID && s.EndDate == null);
                        _billDevice.EndDate = DateTime.Now;
                        _billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(_billDevice.EndDate) - _billDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(_billDevice);
                    }
                    else
                    {
                        _billDevice = unitOfWork.BillsDevices.GetLast((int)_selectedDevice.Device.BillID);
                    }

                    selectedFreeDevice.Device.Case = CaseText.Busy;
                    selectedFreeDevice.Device.BillID = _selectedDevice.Device.BillID;
                    unitOfWork.Devices.Edit(selectedFreeDevice.Device);
                    _selectedDevice.Device.Case = CaseText.Free;
                    _selectedDevice.Device.BillID = null;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);

                    switch (_billDevice.GameType)
                    {
                        case GamePlayTypeText.Single:

                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = selectedFreeDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(selectedFreeDevice.Device.BillID),
                                DeviceID = selectedFreeDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Multiplayer:
                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multiplayer,
                                MinutePrice = selectedFreeDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(selectedFreeDevice.Device.BillID),
                                DeviceID = selectedFreeDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Birthday:
                            _billDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Birthday,
                                MinutePrice = selectedFreeDevice.DeviceType.BirthdayMinutePrice,
                                BillID = Convert.ToInt32(selectedFreeDevice.Device.BillID),
                                DeviceID = selectedFreeDevice.Device.ID
                            };
                            break;

                        default:
                            break;
                    }
                    unitOfWork.BillsDevices.Add(_billDevice);
                    unitOfWork.Complete();
                    FreeDevicesVisibility = VisibilityText.Collapsed;
                    AvailableDevicesVisibility = VisibilityText.Visible;
                    Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Bill Items

        private RelayCommand _showBillItems;
        public RelayCommand ShowBillItems
        {
            get
            {
                return _showBillItems
                    ?? (_showBillItems = new RelayCommand(ShowBillItemsMethod));
            }
        }
        private void ShowBillItemsMethod()
        {
            try
            {
                if (SelectedDevice == null)
                    return;
                _currentWindow.Hide();
                BillItemsViewModel.BillID = (int)SelectedDevice.Device.BillID;
                new BillItemsWindow().ShowDialog();
                _currentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand<string> _showAccount;
        public RelayCommand<string> ShowAccount
        {
            get
            {
                return _showAccount
                    ?? (_showAccount = new RelayCommand<string>(ShowAccountMethod));
            }
        }
        private void ShowAccountMethod(string billID)
        {
            try
            {
                if (billID == null)
                    return;
                AccountVisibility = VisibilityText.Collapsed;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    BillDevices = new ObservableCollection<BillDevicesDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(Convert.ToInt32(billID)));
                    BillItems = new ObservableCollection<BillItemsDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(Convert.ToInt32(billID)));
                }
                AvailableDevicesVisibility = VisibilityText.Collapsed;
                AccountVisibility = VisibilityText.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _cancelAccount;
        public RelayCommand CancelAccount
        {
            get
            {
                return _cancelAccount
                    ?? (_cancelAccount = new RelayCommand(CancelAccountMethod));
            }
        }
        private void CancelAccountMethod()
        {
            try
            {
                AccountVisibility = VisibilityText.Collapsed;
                AvailableDevicesVisibility = VisibilityText.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Items

        private RelayCommand _showAddItems;
        public RelayCommand ShowAddItems
        {
            get
            {
                return _showAddItems
                    ?? (_showAddItems = new RelayCommand(ShowAddItemsMethod));
            }
        }
        private async void ShowAddItemsMethod()
        {
            try
            {
                if (UserData.Role == RoleText.Admin)
                {
                    await _currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    return;
                }
                new ShiftItemsWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Spending

        private RelayCommand _showSpending;
        public RelayCommand ShowSpending
        {
            get
            {
                return _showSpending
                    ?? (_showSpending = new RelayCommand(ShowSpendingMethod));
            }
        }
        private async void ShowSpendingMethod()
        {
            try
            {
               if (UserData.Role == RoleText.Admin)
                {
                    await _currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    return;
                }
                new ShiftSpendingWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //private RelayCommand _stop;
        //public RelayCommand Stop
        //{
        //    get
        //    {
        //        return _stop
        //            ?? (_stop = new RelayCommand(StopMethod));
        //    }
        //}
        //private async void StopMethod()
        //{
        //    try
        //    {
        //        _endDate = DateTime.Now;
        //        if (SelectedDevice == null)
        //            return;
        //        BillDevices = new ObservableCollection<BillDevicesVM>(_billDeviceServ.GetBillDevices(Convert.ToInt32(_selectedDevice.Device.BillID)));
        //        BillItems = new ObservableCollection<BillItem>(_billItemServ.GetBillItems(Convert.ToInt32(_selectedDevice.Device.BillID)));
        //        SelectedClient = new Client();
        //        TelephoneSuggestions = _clientServ.GetTelephoneSuggetions();
        //        _clientCheckDialog.DataContext = this;
        //        await _currentWindow.ShowMetroDialogAsync(_clientCheckDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}





        //// Client

        //private RelayCommand _checkClient;
        //public RelayCommand CheckClient
        //{
        //    get
        //    {
        //        return _checkClient ?? (_checkClient = new RelayCommand(
        //            ExecuteCheckClient,
        //            CanExecuteCheckClient));
        //    }
        //}
        //private async void ExecuteCheckClient()
        //{
        //    try
        //    {
        //        if (SelectedClient.Telephone == null)
        //            return;
        //        var client = _clientServ.GetClient(_selectedClient.Telephone);
        //        await _currentWindow.HideMetroDialogAsync(_clientCheckDialog);
        //        if (client != null)
        //        {
        //            SelectedClient = client;
        //            OpenPaidAccount();
        //        }
        //        else
        //        {
        //            NewClient = new Client();
        //            NewClient.Telephone = SelectedClient.Telephone;
        //            _clientAddDialog.DataContext = this;
        //            await _currentWindow.ShowMetroDialogAsync(_clientAddDialog);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteCheckClient()
        //{
        //    try
        //    {
        //        return !SelectedClient.HasErrors && !HasErrors;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //private RelayCommand _addClient;
        //public RelayCommand AddClient
        //{
        //    get
        //    {
        //        return _addClient ?? (_addClient = new RelayCommand(
        //            ExecuteAddClientAsync,
        //            CanExecuteAddClient));
        //    }
        //}
        //private async void ExecuteAddClientAsync()
        //{
        //    try
        //    {
        //        if (NewClient.Name == null)
        //            return;
        //        _clientServ.AddClient(_newClient);
        //        await _currentWindow.HideMetroDialogAsync(_clientAddDialog);
        //        var client = _clientServ.GetClient(_selectedClient.Telephone);
        //        SelectedClient = client;
        //        OpenPaidAccount();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteAddClient()
        //{
        //    return !NewClient.HasErrors && !HasErrors;
        //}

        //// Pay Account

        //private async void OpenPaidAccount()
        //{
        //    IsMembership = "Visible";
        //    BillDevices = new ObservableCollection<BillDevicesVM>(_billDeviceServ.GetBillDevices(Convert.ToInt32(_selectedDevice.Device.BillID)));

        //    foreach (var item in BillDevices)
        //    {
        //        if (item.BillDevice.Type == GameType.Birthday.GetDescription())
        //        {
        //            IsMembership = "Collapsed";
        //            break;
        //        }
        //    }
        //    var billDevicesCount = BillDevices.Select(k => new { k.DeviceType.Name }).Distinct().Count();


        //    if (_selectedDevice.Device.Case == Case.Busy.GetDescription())
        //    {
        //        _billDevice = _billDeviceServ.GetBillDevice(Convert.ToInt32(_selectedDevice.Device.BillID));
        //        _billDevice.EndDate = _endDate;
        //        _billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(_billDevice.EndDate) - _billDevice.StartDate).TotalMinutes);
        //    }
        //    BillItems = new ObservableCollection<BillItem>(_billItemServ.GetBillItems(Convert.ToInt32(_selectedDevice.Device.BillID)));
        //    SelectBill = _billServ.GetBill(Convert.ToInt32(_selectedDevice.Device.BillID));
        //    SelectBill.ItemsSum = BillItems.Sum(s => Convert.ToDecimal(s.Total));
        //    SelectBill.PlayedMinutes = BillDevices.Sum(s => s.Duration);

        //    var cmm = _clientMembershipMinuteServ.GetClientMembershipMinute(SelectedClient.ID, BillDevices[0].DeviceType.ID);
        //    if (billDevicesCount > 1 || cmm == null || cmm.Minutes == 0)
        //        IsMembership = "Collapsed";
        //    if (IsMembership != "Collapsed")
        //    {
        //        SelectBill.MembershipMinutes = cmm.Minutes;
        //        SelectBill.MembershipMinutesAfterPaid = cmm.Minutes > SelectBill.PlayedMinutes ? cmm.Minutes - SelectBill.PlayedMinutes : 0;
        //        SelectBill.RemainderMinutes = SelectBill.MembershipMinutesAfterPaid > 0 ? 0 : SelectBill.PlayedMinutes - cmm.Minutes;

        //        if (SelectBill.RemainderMinutes > 0)
        //        {
        //            var avg = (BillDevices.Sum(s => s.BillDevice.MinutePrice)) / BillDevices.Count();
        //            SelectBill.DevicesSum = avg * SelectBill.RemainderMinutes;
        //        }
        //        else
        //        {
        //            SelectBill.DevicesSum = 0;
        //        }
        //    }
        //    else
        //        SelectBill.DevicesSum = BillDevices.Sum(s => s.Total);
        //    SelectBill.Total = SelectBill.DevicesSum + SelectBill.ItemsSum;
        //    _accountPaidDialog.DataContext = this;
        //    await _currentWindow.ShowMetroDialogAsync(_accountPaidDialog);

        //}

        //private RelayCommand _ratioChanged;
        //public RelayCommand RatioChanged
        //{
        //    get
        //    {
        //        return _ratioChanged
        //            ?? (_ratioChanged = new RelayCommand(RatioChangedMethod));
        //    }
        //}
        //private void RatioChangedMethod()
        //{
        //    try
        //    {
        //        SelectBill.Discount = (SelectBill.Ratio * SelectBill.Total) / 100;
        //        SelectBill.TotalAfterDiscount = Math.Round(Convert.ToDecimal(SelectBill.Total - SelectBill.Discount), 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _discountChanged;
        //public RelayCommand DiscountChanged
        //{
        //    get
        //    {
        //        return _discountChanged
        //            ?? (_discountChanged = new RelayCommand(DiscountChangedMethod));
        //    }
        //}
        //private void DiscountChangedMethod()
        //{
        //    try
        //    {
        //        if (SelectBill.Total != 0)
        //            SelectBill.Ratio = (SelectBill.Discount * 100) / SelectBill.Total;
        //        else
        //            SelectBill.Ratio = 0;
        //        SelectBill.TotalAfterDiscount = Math.Round(Convert.ToDecimal(SelectBill.Total - SelectBill.Discount), 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _save;
        //public RelayCommand Save
        //{
        //    get
        //    {
        //        return _save
        //            ?? (_save = new RelayCommand(SaveMethod));
        //    }
        //}
        //private async void SaveMethod()
        //{
        //    try
        //    {
        //        if (SelectBill.Minimum != null && SelectBill.Minimum > 0)
        //        {
        //            Safe _safe = new Safe
        //            {
        //                Amount = _selectBill.Minimum,
        //                CanDelete = false,
        //                Statement = "فاتورة للجهاز  " + _selectedDevice.Device.Name,
        //                UserID = MainViewModel.UserID,
        //                RegistrationDate = _endDate
        //            };
        //            _safeServ.AddSafe(_safe);
        //            if (_selectedDevice.Device.Case == Case.Busy.GetDescription())
        //            {
        //                _billDeviceServ.UpdateBillDevice(_billDevice);
        //            }
        //            _selectedDevice.Device.Case = Case.Free.GetDescription();
        //            _selectedDevice.Device.BillID = null;
        //            _deviceServ.UpdateDevice(_selectedDevice.Device);

        //            _selectBill.UserID = MainViewModel.UserID;
        //            _selectBill.EndDate = _endDate;
        //            _selectBill.Date = _endDate;
        //            _selectBill.ClientID = _selectedClient.ID;
        //            _selectBill.Total = _selectBill.Minimum;
        //            _selectBill.TotalAfterDiscount = _selectBill.Minimum;
        //            _billServ.UpdateBill(_selectBill);

        //            Devices = new ObservableCollection<DeviceVM>(_deviceServ.GetDevices());
        //            BusyDevices = _devices.Where(w => w.Device.Case == Case.Busy.GetDescription()).Count();
        //            AvailableDevices = _devices.Where(w => w.Device.Case == Case.Free.GetDescription()).Count();
        //            TemporaryDevices = _devices.Where(w => w.Device.Case == Case.Temporary.GetDescription()).Count();
        //            await _currentWindow.HideMetroDialogAsync(_accountPaidDialog);

        //            return;
        //        }

        //        if (SelectBill.TotalAfterDiscount == null || SelectBill.Discount > SelectBill.Total)
        //            return;

        //        if (_selectedDevice.Device.Case == Case.Busy.GetDescription())
        //        {
        //            _billDeviceServ.UpdateBillDevice(_billDevice);
        //        }
        //        _selectedDevice.Device.Case = Case.Free.GetDescription();
        //        _selectedDevice.Device.BillID = null;
        //        _deviceServ.UpdateDevice(_selectedDevice.Device);

        //        _selectBill.UserID = MainViewModel.UserID;
        //        _selectBill.EndDate = _endDate;
        //        _selectBill.Date = _endDate;
        //        _selectBill.ClientID = _selectedClient.ID;
        //        _billServ.UpdateBill(_selectBill);

        //        if (_selectBill.TotalAfterDiscount > 0)
        //        {
        //            Safe _safe = new Safe
        //            {
        //                Amount = _selectBill.TotalAfterDiscount,
        //                CanDelete = false,
        //                Statement = "فاتورة للجهاز  " + _selectedDevice.Device.Name,
        //                UserID = MainViewModel.UserID,
        //                RegistrationDate = _endDate
        //            };
        //            _safeServ.AddSafe(_safe);
        //        }

        //        if (IsMembership != "Collapsed")
        //        {
        //            var cmm = _clientMembershipMinuteServ.GetClientMembershipMinute(SelectedClient.ID, BillDevices[0].DeviceType.ID);
        //            cmm.Minutes = (int)_selectBill.MembershipMinutesAfterPaid;
        //            _clientMembershipMinuteServ.UpdateClientMembershipMinute(cmm);
        //        }
        //        Devices = new ObservableCollection<DeviceVM>(_deviceServ.GetDevices());
        //        BusyDevices = _devices.Where(w => w.Device.Case == Case.Busy.GetDescription()).Count();
        //        AvailableDevices = _devices.Where(w => w.Device.Case == Case.Free.GetDescription()).Count();
        //        TemporaryDevices = _devices.Where(w => w.Device.Case == Case.Temporary.GetDescription()).Count();
        //        await _currentWindow.HideMetroDialogAsync(_accountPaidDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _print;
        //public RelayCommand Print
        //{
        //    get
        //    {
        //        return _print
        //            ?? (_print = new RelayCommand(PrintMethod));
        //    }
        //}
        //private void PrintMethod()
        //{
        //    try
        //    {
        //        SaveMethod();
        //        // Account Print

        //        Mouse.OverrideCursor = Cursors.Wait;
        //        DS ds = new DS();
        //        ds.Bill.Rows.Clear();
        //        int i = 0;

        //        foreach (var item in BillDevices)
        //        {
        //            var hoursPlayed = item.Duration / 60;
        //            var minuutesPlayed = item.Duration % 60;
        //            ds.Bill.Rows.Add();
        //            ds.Bill[i]["BillID"] = "#" + _selectBill.ID;
        //            ds.Bill[i]["Date"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //            ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
        //            ds.Bill[i]["Device"] = item.DeviceType.Name + " ( " + item.Device.Name + " ) : " + item.BillDevice.Type;
        //            ds.Bill[i]["StartDate"] = "Start Time: " + item.BillDevice.StartDate.ToString(" h:mm tt");
        //            ds.Bill[i]["EndDate"] = "End Time: " + Convert.ToDateTime(item.EndDate).ToString(" h:mm tt");
        //            ds.Bill[i]["TotalTime"] = "Total Time: " + Convert.ToInt32(hoursPlayed).ToString("D2") + ":" + Convert.ToInt32(minuutesPlayed).ToString("D2");
        //            ds.Bill[i]["TotalPlayedMoney"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(item.Total), 0));
        //            ds.Bill[i]["Discount"] = string.Format("{0:0.00}", _selectBill.Discount);
        //            ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", _selectBill.TotalAfterDiscount);
        //            i++;
        //        }
        //        i = 0;
        //        foreach (var item in BillItems)
        //        {
        //            ds.Items.Rows.Add();
        //            ds.Items[i]["Qty"] = item.Qty;
        //            ds.Items[i]["Item"] = item.Item.Name;
        //            ds.Items[i]["Total"] = string.Format("{0:0.00}", item.Total); ;
        //            i++;
        //        }
        //        if (IsMembership == "Collapsed")
        //        {
        //            if (BillItems.Count == 0)
        //            {
        //                ReportWindow rpt = new ReportWindow();
        //                BillReport billReport = new BillReport();
        //                billReport.SetDataSource(ds.Tables["Bill"]);
        //                rpt.crv.ViewerCore.ReportSource = billReport;
        //                Mouse.OverrideCursor = null;
        //                rpt.ShowDialog();
        //            }
        //            else
        //            {
        //                ReportWindow rpt = new ReportWindow();
        //                BillItemsReport billItemsReport = new BillItemsReport();
        //                billItemsReport.SetDataSource(ds.Tables["Bill"]);
        //                billItemsReport.Subreports[0].SetDataSource(ds.Tables["Items"]);
        //                rpt.crv.ViewerCore.ReportSource = billItemsReport;
        //                Mouse.OverrideCursor = null;
        //                rpt.ShowDialog();
        //            }
        //        }
        //        Mouse.OverrideCursor = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}



        //// Finish Shift

        //private RelayCommand _showFinishShift;
        //public RelayCommand ShowFinishShift
        //{
        //    get
        //    {
        //        return _showFinishShift ?? (_showFinishShift = new RelayCommand(
        //            ExecuteShowFinishShiftAsync));
        //    }
        //}
        //private async void ExecuteShowFinishShiftAsync()
        //{
        //    try
        //    {
        //        if (MainViewModel.UserRole == "ادمن")
        //        {
        //            await _currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
        //            {
        //                AffirmativeButtonText = "موافق",
        //                DialogMessageFontSize = 25,
        //                DialogTitleFontSize = 30
        //            });
        //            return;
        //        }
        //        _finishShiftDate = DateTime.Now;

        //        _itemsBill = _billServ.GetBill();
        //        _itemsBill.UserID = MainViewModel.UserID;
        //        _itemsBill.ItemsSum = _billItemServ.GetItems().Sum(s => s.Total);
        //        _itemsBill.Total = _itemsBill.ItemsSum;
        //        _itemsBill.TotalAfterDiscount = _itemsBill.Total;
        //        _itemsBill.Discount = 0;
        //        _itemsBill.Ratio = 0;
        //        _itemsBill.Date = DateTime.Now;
        //        _itemsBill.EndDate = _finishShiftDate;

        //        FinishShiftVM = new FinishShiftVM();
        //        NewShift = _shiftServ.GetShift(MainViewModel.UserID);
        //        NewShift.EndDate = _finishShiftDate;
        //        NewShift.Income = _safeServ.GetTotalIncome(_newShift.UserID, _newShift.StartDate, Convert.ToDateTime(_newShift.EndDate)) + _itemsBill.Total;
        //        NewShift.Spending = Math.Abs(Convert.ToDecimal(_safeServ.GetTotalOutgoings(_newShift.UserID, _newShift.StartDate, Convert.ToDateTime(_newShift.EndDate))));
        //        _finishShiftDialog.DataContext = this;
        //        await _currentWindow.ShowMetroDialogAsync(_finishShiftDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _finishShift;
        //public RelayCommand FinishShift
        //{
        //    get
        //    {
        //        return _finishShift ?? (_finishShift = new RelayCommand(
        //            ExecuteFinishShiftAsync,
        //            CanExecuteFinishShift));
        //    }
        //}
        //private async void ExecuteFinishShiftAsync()
        //{
        //    try
        //    {
        //        if (FinishShiftVM.SafeEnd == null || FinishShiftVM.Name == null || FinishShiftVM.Password == null)
        //            return;

        //        var user = _userServ.GetUser(_finishShiftVM.Name, _finishShiftVM.Password);

        //        if (user == null)
        //        {
        //            await _currentWindow.ShowMessageAsync("فشل الإنهاء", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
        //            {
        //                AffirmativeButtonText = "موافق",
        //                DialogMessageFontSize = 25,
        //                DialogTitleFontSize = 30
        //            });
        //            return;
        //        }

        //        if (user.Role.Name != "كاشير")
        //        {
        //            await _currentWindow.ShowMessageAsync("فشل الإنهاء", "لا تملك الصلاحية", MessageDialogStyle.Affirmative, new MetroDialogSettings()
        //            {
        //                AffirmativeButtonText = "موافق",
        //                DialogMessageFontSize = 25,
        //                DialogTitleFontSize = 30
        //            });
        //            return;
        //        }
        //        _billServ.UpdateBill(_itemsBill);
        //        if (_itemsBill.TotalAfterDiscount > 0)
        //        {
        //            Safe _safe = new Safe
        //            {
        //                Amount = _itemsBill.TotalAfterDiscount,
        //                CanDelete = false,
        //                Statement = "طلبات خارجية ",
        //                UserID = MainViewModel.UserID,
        //                RegistrationDate = _finishShiftDate
        //            };
        //            _safeServ.AddSafe(_safe);
        //        }

        //        _newShift.SafeEnd = _finishShiftVM.SafeEnd;
        //        _shiftServ.UpdateShift(_newShift);


        //        MainViewModel.UserID = user.ID;
        //        MainViewModel.UserRole = user.Role.Name;
        //        MainViewModel.UserName = user.Name;

        //        _newShift = new Shift();
        //        _newShift.StartDate = DateTime.Now;
        //        _newShift.UserID = MainViewModel.UserID;
        //        _newShift.SafeStart = _finishShiftVM.SafeEnd;
        //        _shiftServ.AddShift(_newShift);

        //        await _currentWindow.HideMetroDialogAsync(_finishShiftDialog);

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteFinishShift()
        //{
        //    return !FinishShiftVM.HasErrors;
        //}

        //private RelayCommand<string> _closeDialog;
        //public RelayCommand<string> CloseDialog
        //{
        //    get
        //    {
        //        return _closeDialog
        //            ?? (_closeDialog = new RelayCommand<string>(ExecuteCloseDialogAsync));
        //    }
        //}
        //private async void ExecuteCloseDialogAsync(string parameter)
        //{
        //    try
        //    {
        //        switch (parameter)
        //        {
        //            case "ClientCheck":
        //                await _currentWindow.HideMetroDialogAsync(_clientCheckDialog);
        //                break;
        //            case "AddClient":
        //                await _currentWindow.HideMetroDialogAsync(_clientAddDialog);
        //                break;
        //            case "AccountPaid":
        //                await _currentWindow.HideMetroDialogAsync(_accountPaidDialog);
        //                break;
        //            case "FinishShift":
        //                await _currentWindow.HideMetroDialogAsync(_finishShiftDialog);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

    }
}
