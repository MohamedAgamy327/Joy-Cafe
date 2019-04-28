using BLL.UnitOfWorkService;
using Cafe.Reports;
using Cafe.Views.CashierViews.AccountPaidViews;
using Cafe.Views.CashierViews.BillClientViews;
using Cafe.Views.CashierViews.BillItemsViews;
using Cafe.Views.CashierViews.CancelBillViews;
using Cafe.Views.CashierViews.FinishShiftViews;
using Cafe.Views.CashierViews.ShiftItemsViews;
using Cafe.Views.CashierViews.ShiftSpendingViews;
using DAL;
using DAL.BindableBaseService;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDataModel;
using DTO.BillDeviceDataModel;
using DTO.BillItemDataModel;
using DTO.ClientDataModel;
using DTO.DeviceDataModel;
using DTO.MainDataModel;
using DTO.ShiftDataModel;
using DTO.UserDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Cafe.ViewModels.CashierViewModels
{
    public class DevicesViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        private readonly ClientAddDialog clientAddDialog;
        private readonly ClientCheckDialog clientCheckDialog;
        private readonly FinishShiftDialog finishShiftDialog;
        private readonly FinishShiftConfirmDialog finishShiftConfirmDialog;
        private readonly CancelReasonDialog cancelReasonDialog;

        public DevicesViewModel()
        {
            UserData.newShift = false;
            _isFocused = true;
            _accountVisibility = Visibility.Collapsed;
            _freeDevicesVisibility = Visibility.Collapsed;
            _availableDevicesVisibility = Visibility.Visible;
            _printVisibility = Visibility.Visible;
            clientAddDialog = new ClientAddDialog();
            clientCheckDialog = new ClientCheckDialog();
            finishShiftDialog = new FinishShiftDialog();
            finishShiftConfirmDialog = new FinishShiftConfirmDialog();
            cancelReasonDialog = new CancelReasonDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
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

        private int _busyDevices;
        public int BusyDevices
        {
            get { return _busyDevices; }
            set { SetProperty(ref _busyDevices, value); }
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

        private string _currentTime;
        public DispatcherTimer _timer;
        public string CurrentTime
        {
            get
            {
                return this._currentTime;
            }
            set
            {
                if (_currentTime == value)
                    return;
                _currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        private Visibility _accountVisibility;
        public Visibility AccountVisibility
        {
            get { return _accountVisibility; }
            set { SetProperty(ref _accountVisibility, value); }
        }

        private Visibility _freeDevicesVisibility;
        public Visibility FreeDevicesVisibility
        {
            get { return _freeDevicesVisibility; }
            set { SetProperty(ref _freeDevicesVisibility, value); }
        }

        private Visibility _availableDevicesVisibility;
        public Visibility AvailableDevicesVisibility
        {
            get { return _availableDevicesVisibility; }
            set { SetProperty(ref _availableDevicesVisibility, value); }
        }

        private Visibility _startVisibility;
        public Visibility StartVisibility
        {
            get { return _startVisibility; }
            set { SetProperty(ref _startVisibility, value); }
        }

        private Visibility _stopVisibility;
        public Visibility StopVisibility
        {
            get { return _stopVisibility; }
            set { SetProperty(ref _stopVisibility, value); }
        }

        private Visibility _birthdayVisibility;
        public Visibility BirthdayVisibility
        {
            get { return _birthdayVisibility; }
            set { SetProperty(ref _birthdayVisibility, value); }
        }

        private Visibility _singleVisibility;
        public Visibility SingleVisibility
        {
            get { return _singleVisibility; }
            set { SetProperty(ref _singleVisibility, value); }
        }

        private Visibility _multiVisibility;
        public Visibility MultiVisibility
        {
            get { return _multiVisibility; }
            set { SetProperty(ref _multiVisibility, value); }
        }

        private Visibility _temporaryVisibility;
        public Visibility TemporaryVisibility
        {
            get { return _temporaryVisibility; }
            set { SetProperty(ref _temporaryVisibility, value); }
        }

        private Visibility _resumeVisibility;
        public Visibility ResumeVisibility
        {
            get { return _resumeVisibility; }
            set { SetProperty(ref _resumeVisibility, value); }
        }

        private Visibility _printVisibility;
        public Visibility PrintVisibility
        {
            get { return _printVisibility; }
            set { SetProperty(ref _printVisibility, value); }
        }

        private Visibility _cancelVisibility;
        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set { SetProperty(ref _cancelVisibility, value); }
        }

        private ClientBillAddDataModel _newClient;
        public ClientBillAddDataModel NewClient
        {
            get { return _newClient; }
            set { SetProperty(ref _newClient, value); }
        }

        private DevicePlayDataModel _selectedDevice;
        public DevicePlayDataModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { SetProperty(ref _selectedDevice, value); }
        }

        private ClientCheckDataModel _clientCheck;
        public ClientCheckDataModel ClientCheck
        {
            get { return _clientCheck; }
            set { SetProperty(ref _clientCheck, value); }
        }

        private LoginDataModel _loginModel;
        public LoginDataModel LoginModel
        {
            get { return _loginModel; }
            set { SetProperty(ref _loginModel, value); }
        }

        private FinishShiftDataModel _shift;
        public FinishShiftDataModel Shift
        {
            get { return _shift; }
            set { SetProperty(ref _shift, value); }
        }

        private BillCancelDataModel _billCancel;
        public BillCancelDataModel BillCancel
        {
            get { return _billCancel; }
            set { SetProperty(ref _billCancel, value); }
        }

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

        // Device Show

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
                _timer = new DispatcherTimer(DispatcherPriority.Render)
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                _timer.Tick += (sender, args) =>
                {
                    CurrentTime = DateTime.Now.ToLongTimeString();
                };
                _timer.Start();

                UpdateData();
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
                if (SelectedDevice == null)
                {
                    SingleVisibility = Visibility.Collapsed;
                    MultiVisibility = Visibility.Collapsed;
                    BirthdayVisibility = Visibility.Collapsed;
                    ResumeVisibility = Visibility.Collapsed;
                    TemporaryVisibility = Visibility.Collapsed;
                    StartVisibility = Visibility.Collapsed;
                    StopVisibility = Visibility.Collapsed;
                    PrintVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Collapsed;
                    return;
                }


                SingleVisibility = Visibility.Visible;
                MultiVisibility = Visibility.Visible;
                BirthdayVisibility = Visibility.Visible;
                PrintVisibility = Visibility.Visible;

                if (_selectedDevice.Device.Case != DeviceCaseText.Free)
                {
                    StartVisibility = Visibility.Collapsed;
                    StopVisibility = Visibility.Visible;
                    CancelVisibility = Visibility.Visible;
                }
                else
                {
                    StartVisibility = Visibility.Visible;
                    StopVisibility = Visibility.Collapsed;
                    CancelVisibility = Visibility.Collapsed;
                }
                if (_selectedDevice.DeviceType.Birthday && _selectedDevice.GameType != GamePlayTypeText.Birthday)
                    BirthdayVisibility = Visibility.Visible;
                else
                    BirthdayVisibility = Visibility.Collapsed;

                if (_selectedDevice.GameType == GamePlayTypeText.Single)
                    SingleVisibility = Visibility.Collapsed;
                if (_selectedDevice.GameType == GamePlayTypeText.Multi)
                    MultiVisibility = Visibility.Collapsed;

                if (_selectedDevice.Device.Case == DeviceCaseText.Free)
                {
                    ResumeVisibility = Visibility.Collapsed;
                    TemporaryVisibility = Visibility.Collapsed;
                }
                else if (_selectedDevice.Device.Case == DeviceCaseText.Paused)
                {
                    ResumeVisibility = Visibility.Visible;
                    TemporaryVisibility = Visibility.Collapsed;
                }
                else
                {
                    ResumeVisibility = Visibility.Collapsed;
                    TemporaryVisibility = Visibility.Visible;
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
                    Bill bill = new Bill
                    {
                        StartDate = DateTime.Now,
                        Type = BillTypeText.Devices
                    };
                    unitOfWork.Bills.Add(bill);

                    _selectedDevice.Device.Case = DeviceCaseText.Busy;
                    _selectedDevice.Device.BillID = bill.ID;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);
                    BillDevice newBillDevice = null;
                    switch (parameter)
                    {
                        case GamePlayTypeText.Single:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Multi:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multi,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Birthday:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Birthday,
                                MinutePrice = _selectedDevice.DeviceType.BirthdayMinutePrice,
                                BillID = bill.ID,
                                DeviceID = _selectedDevice.Device.ID
                            };
                            break;

                        default:
                            break;
                    }

                    unitOfWork.BillsDevices.Add(newBillDevice);
                    unitOfWork.Complete();
                    UpdateData();
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
                    if (_selectedDevice.Device.Case == DeviceCaseText.Busy)
                    {
                        BillDevice selectedBillDevice = unitOfWork.BillsDevices.FirstOrDefault(w => w.BillID == _selectedDevice.Device.BillID && w.EndDate == null);
                        selectedBillDevice.EndDate = DateTime.Now;
                        selectedBillDevice.Duration = Convert.ToInt32((Convert.ToDateTime(selectedBillDevice.EndDate) - selectedBillDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(selectedBillDevice);
                    }
                    else
                    {
                        _selectedDevice.Device.Case = DeviceCaseText.Busy;
                        unitOfWork.Devices.Edit(_selectedDevice.Device);
                    }
                    BillDevice newBillDevice = null;
                    switch (parameter)
                    {
                        case GamePlayTypeText.Single:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Multi:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multi,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Birthday:

                            newBillDevice = new BillDevice
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

                    unitOfWork.BillsDevices.Add(newBillDevice);
                    unitOfWork.Complete();
                    UpdateData();
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
                    _selectedDevice.Device.Case = DeviceCaseText.Paused;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);
                    BillDevice selectedBillDevice = unitOfWork.BillsDevices.FirstOrDefault(s => s.BillID == _selectedDevice.Device.BillID && s.EndDate == null);
                    selectedBillDevice.EndDate = DateTime.Now;
                    selectedBillDevice.Duration = Convert.ToInt32((Convert.ToDateTime(selectedBillDevice.EndDate) - selectedBillDevice.StartDate).TotalMinutes);
                    unitOfWork.BillsDevices.Edit(selectedBillDevice);
                    unitOfWork.Complete();

                    UpdateData();
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
                    _selectedDevice.Device.Case = DeviceCaseText.Busy;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);

                    BillDevice selectedBillDevice = unitOfWork.BillsDevices.GetLast((int)_selectedDevice.Device.BillID);
                    BillDevice newBillDevice = null;

                    switch (selectedBillDevice.GameType)
                    {
                        case GamePlayTypeText.Single:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = _selectedDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Multi:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multi,
                                MinutePrice = _selectedDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(_selectedDevice.Device.BillID),
                                DeviceID = _selectedDevice.Device.ID
                            };

                            break;

                        case GamePlayTypeText.Birthday:

                            newBillDevice = new BillDevice
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
                    unitOfWork.BillsDevices.Add(newBillDevice);
                    unitOfWork.Complete();
                    UpdateData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _print;
        public RelayCommand Print
        {
            get
            {
                return _print
                    ?? (_print = new RelayCommand(PrintMethodAsync));
            }
        }
        private async void PrintMethodAsync()
        {
            try
            {
                // Account Print
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var bill = unitOfWork.Bills.GetLastBill(_selectedDevice.Device.ID);
                    if (bill == null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الطبع", "عفواً لا يوجد فاتورة سابقة", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        return;
                    }
                    var billDevices = new ObservableCollection<BillDevicesDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(bill.ID));
                    var billItems = new ObservableCollection<BillItemsDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(bill.ID));

                    Mouse.OverrideCursor = Cursors.Wait;
                    DS ds = new DS();
                    ds.Bill.Rows.Clear();
                    int i = 0;

                    foreach (var item in billDevices)
                    {
                        var hoursPlayed = item.Duration / 60;
                        var minuutesPlayed = item.Duration % 60;
                        ds.Bill.Rows.Add();
                        ds.Bill[i]["BillID"] = "#" + bill.ID;
                        ds.Bill[i]["Date"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                        ds.Bill[i]["Device"] = item.DeviceType.Name + " ( " + item.Device.Name + " ) : " + item.BillDevice.GameType;
                        ds.Bill[i]["StartDate"] = "Start Time: " + item.BillDevice.StartDate.ToString(" h:mm tt");
                        ds.Bill[i]["EndDate"] = "End Time: " + Convert.ToDateTime(item.EndDate).ToString(" h:mm tt");
                        ds.Bill[i]["TotalTime"] = "Total Time: " + Convert.ToInt32(hoursPlayed).ToString("D2") + ":" + Convert.ToInt32(minuutesPlayed).ToString("D2");
                        ds.Bill[i]["TotalPlayedMoney"] = bill.MembershipMinutes == null ? string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(item.Total), 0)) : "";
                        ds.Bill[i]["Discount"] = string.Format("{0:0.00}", bill.Discount);
                        ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(bill.TotalAfterDiscount), 0));
                        i++;
                    }
                    i = 0;
                    foreach (var item in billItems)
                    {
                        ds.Items.Rows.Add();
                        ds.Items[i]["Qty"] = item.BillItem.Qty;
                        ds.Items[i]["Item"] = item.Item.Name;
                        ds.Items[i]["Total"] = string.Format("{0:0.00}", item.BillItem.Total); ;
                        i++;
                    }

                    if (billItems.Count == 0)
                    {
                        //ReportWindow rpt = new ReportWindow();
                        BillReport billReport = new BillReport();
                        billReport.SetDataSource(ds.Tables["Bill"]);
                        //rpt.crv.ViewerCore.ReportSource = billReport;
                        Mouse.OverrideCursor = null;
                        //rpt.ShowDialog();
                        billReport.PrintToPrinter(1, false, 0, 15);
                    }
                    else
                    {
                        //ReportWindow rpt = new ReportWindow();
                        BillItemsReport billItemsReport = new BillItemsReport();
                        billItemsReport.SetDataSource(ds.Tables["Bill"]);
                        billItemsReport.Subreports[0].SetDataSource(ds.Tables["Items"]);
                        //rpt.crv.ViewerCore.ReportSource = billItemsReport;
                        Mouse.OverrideCursor = null;
                        billItemsReport.PrintToPrinter(1, false, 0, 15);
                        //rpt.ShowDialog();
                        billItemsReport.PrintToPrinter(1, false, 0, 15);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        // Stop and Pay

        private RelayCommand _stop;
        public RelayCommand Stop
        {
            get
            {
                return _stop
                    ?? (_stop = new RelayCommand(StopMethod));
            }
        }
        private async void StopMethod()
        {
            try
            {
                if (UserData.Role == RoleText.Admin)
                {
                    await currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    return;
                }
                BillData.EndDate = DateTime.Now;
                if (SelectedDevice == null)
                    return;
                ClientCheck = new ClientCheckDataModel();

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    TelephoneSuggestions = unitOfWork.Clients.GetTelephoneSuggetions();
                    BillDevices = new ObservableCollection<BillDevicesDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(Convert.ToInt32(_selectedDevice.Device.BillID)));
                    BillItems = new ObservableCollection<BillItemsDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(Convert.ToInt32(_selectedDevice.Device.BillID)));
                }

                clientCheckDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(clientCheckDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _checkClient;
        public RelayCommand CheckClient
        {
            get
            {
                return _checkClient ?? (_checkClient = new RelayCommand(
                    ExecuteCheckClient,
                    CanExecuteCheckClient));
            }
        }
        private async void ExecuteCheckClient()
        {
            try
            {
                if (ClientCheck.Telephone == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var client = unitOfWork.Clients.SingleOrDefault(s => s.Telephone == _clientCheck.Telephone);
                    await currentWindow.HideMetroDialogAsync(clientCheckDialog);
                    if (client == null)
                    {
                        NewClient = new ClientBillAddDataModel
                        {
                            Telephone = _clientCheck.Telephone
                        };
                        clientAddDialog.DataContext = this;
                        await currentWindow.ShowMetroDialogAsync(clientAddDialog);
                    }
                    else
                    {
                        BillData.BillID = (int)_selectedDevice.Device.BillID;
                        BillData.ClientID = client.ID;
                        BillData.DeviceID = _selectedDevice.Device.ID;
                        new AccountPaidWindow().ShowDialog();
                        UpdateData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteCheckClient()
        {
            try
            {
                return !ClientCheck.HasErrors;
            }
            catch
            {
                return false;
            }
        }

        private RelayCommand _addClient;
        public RelayCommand AddClient
        {
            get
            {
                return _addClient ?? (_addClient = new RelayCommand(
                    ExecuteAddClientAsync,
                    CanExecuteAddClient));
            }
        }
        private async void ExecuteAddClientAsync()
        {
            try
            {
                if (NewClient.Name == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var client = unitOfWork.Clients.Add(new Client
                    {
                        Name = _newClient.Name,
                        Telephone = _newClient.Telephone
                    });
                    unitOfWork.Complete();
                    BillData.DeviceID = _selectedDevice.Device.ID;
                    BillData.ClientID = client.ID;
                    BillData.BillID = (int)_selectedDevice.Device.BillID;
                }
                await currentWindow.HideMetroDialogAsync(clientAddDialog);

                new AccountPaidWindow().ShowDialog();
                UpdateData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteAddClient()
        {
            return !NewClient.HasErrors;
        }

        // Cancel

        private RelayCommand _showCancel;
        public RelayCommand ShowCancel
        {
            get
            {
                return _showCancel
                    ?? (_showCancel = new RelayCommand(ShowCancelMethod));
            }
        }
        private async void ShowCancelMethod()
        {
            try
            {
                BillCancel = new BillCancelDataModel();
                cancelReasonDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(cancelReasonDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _cancel;
        public RelayCommand Cancel
        {
            get
            {
                return _cancel
                    ?? (_cancel = new RelayCommand(CancelMethod, CanExecuteCancel));
            }
        }
        private async void CancelMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    if (SelectedDevice.Device.Case == DeviceCaseText.Busy)
                    {
                        BillDevice selectedBillDevice = unitOfWork.BillsDevices.FirstOrDefault(s => s.BillID == _selectedDevice.Device.BillID && s.EndDate == null);
                        selectedBillDevice.EndDate = DateTime.Now;
                        selectedBillDevice.Duration = Convert.ToInt32((Convert.ToDateTime(selectedBillDevice.EndDate) - selectedBillDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(selectedBillDevice);
                    }
                    var bill = unitOfWork.Bills.Get((int)_selectedDevice.Device.BillID);
                    bill.UserID = UserData.ID;
                    bill.EndDate = DateTime.Now;
                    bill.Date = DateTime.Now;
                    bill.Canceled = true;
                    bill.CancelReason = _billCancel.CancelReason;
                    unitOfWork.Bills.Edit(bill);
                    _selectedDevice.Device.BillID = null;
                    _selectedDevice.Device.Case = DeviceCaseText.Free;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);
                    unitOfWork.Complete();
                    UpdateData();
                }
                await currentWindow.HideMetroDialogAsync(cancelReasonDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteCancel()
        {
            return !BillCancel.HasErrors;
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
                AvailableDevicesVisibility = Visibility.Collapsed;
                FreeDevicesVisibility = Visibility.Visible;
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
                FreeDevicesVisibility = Visibility.Collapsed;
                AvailableDevicesVisibility = Visibility.Visible;
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
                    BillDevice selectedBillDevice;

                    if (_selectedDevice.Device.Case == DeviceCaseText.Busy)
                    {
                        selectedBillDevice = unitOfWork.BillsDevices.FirstOrDefault(s => s.BillID == _selectedDevice.Device.BillID && s.EndDate == null);
                        selectedBillDevice.EndDate = DateTime.Now;
                        selectedBillDevice.Duration = Convert.ToInt32((Convert.ToDateTime(selectedBillDevice.EndDate) - selectedBillDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(selectedBillDevice);
                    }
                    else
                    {
                        selectedBillDevice = unitOfWork.BillsDevices.GetLast((int)_selectedDevice.Device.BillID);
                    }

                    selectedFreeDevice.Device.Case = DeviceCaseText.Busy;
                    selectedFreeDevice.Device.BillID = _selectedDevice.Device.BillID;
                    unitOfWork.Devices.Edit(selectedFreeDevice.Device);
                    _selectedDevice.Device.Case = DeviceCaseText.Free;
                    _selectedDevice.Device.BillID = null;
                    unitOfWork.Devices.Edit(_selectedDevice.Device);

                    BillDevice newBillDevice = null;
                    switch (selectedBillDevice.GameType)
                    {
                        case GamePlayTypeText.Single:

                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Single,
                                MinutePrice = selectedFreeDevice.DeviceType.SingleMinutePrice,
                                BillID = Convert.ToInt32(selectedFreeDevice.Device.BillID),
                                DeviceID = selectedFreeDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Multi:
                            newBillDevice = new BillDevice
                            {
                                StartDate = DateTime.Now,
                                GameType = GamePlayTypeText.Multi,
                                MinutePrice = selectedFreeDevice.DeviceType.MultiMinutePrice,
                                BillID = Convert.ToInt32(selectedFreeDevice.Device.BillID),
                                DeviceID = selectedFreeDevice.Device.ID
                            };
                            break;

                        case GamePlayTypeText.Birthday:
                            newBillDevice = new BillDevice
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
                    unitOfWork.BillsDevices.Add(newBillDevice);
                    unitOfWork.Complete();
                    FreeDevicesVisibility = Visibility.Collapsed;
                    AvailableDevicesVisibility = Visibility.Visible;
                    UpdateData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Bill Account

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
                AccountVisibility = Visibility.Collapsed;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    BillDevices = new ObservableCollection<BillDevicesDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(Convert.ToInt32(billID)));
                    BillItems = new ObservableCollection<BillItemsDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(Convert.ToInt32(billID)));
                }
                AvailableDevicesVisibility = Visibility.Collapsed;
                AccountVisibility = Visibility.Visible;
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
                AccountVisibility = Visibility.Collapsed;
                AvailableDevicesVisibility = Visibility.Visible;
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
                currentWindow.Hide();
                BillItemsViewModel.BillID = (int)SelectedDevice.Device.BillID;
                new BillItemsWindow().ShowDialog();
                currentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Items

        private RelayCommand _showItems;
        public RelayCommand ShowItems
        {
            get
            {
                return _showItems
                    ?? (_showItems = new RelayCommand(ShowItemsMethod));
            }
        }
        private async void ShowItemsMethod()
        {
            try
            {
                if (UserData.Role == RoleText.Admin)
                {
                    await currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
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
                    await currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
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

        // Finish Shift

        private RelayCommand _showFinishShift;
        public RelayCommand ShowFinishShift
        {
            get
            {
                return _showFinishShift ?? (_showFinishShift = new RelayCommand(
                    ExecuteShowFinishShiftAsync));
            }
        }
        private async void ExecuteShowFinishShiftAsync()
        {
            try
            {
                if (UserData.Role == RoleText.Admin)
                {
                    await currentWindow.ShowMessageAsync("تنبيه", "الكاشير فقط من له الصلاحية فى الدخول", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    return;
                }
                LoginModel = new LoginDataModel();
                finishShiftConfirmDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(finishShiftConfirmDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _signIn;
        public RelayCommand SignIn
        {
            get
            {
                return _signIn ?? (_signIn = new RelayCommand(
                    ExecuteSignInAsync,
                    CanExecuteSignIn));
            }
        }
        private async void ExecuteSignInAsync()
        {
            try
            {
                if (LoginModel.Name == null || LoginModel.Password == null)
                    return;
                if (LoginModel.Name == UserData.Name && LoginModel.Password == UserData.Password)
                {
                    using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                    {
                        Shift shift = unitOfWork.Shifts.FirstOrDefault(s => s.EndDate == null);

                        decimal safeIncome = unitOfWork.Safes.Find(f => f.UserID == UserData.ID && f.Type == true && f.RegistrationDate >= shift.StartDate && f.RegistrationDate <= DateTime.Now).Sum(s => s.Amount) ?? 0;

                        decimal itemsBillTotal = unitOfWork.BillsItems.Find(f => f.Bill.Type == BillTypeText.Items && f.Bill.EndDate == null).Sum(s => s.Total) ?? 0;

                        decimal spending = unitOfWork.Safes.Find(f => f.UserID == UserData.ID && f.Type == false && f.RegistrationDate >= shift.StartDate && f.RegistrationDate <= DateTime.Now).Sum(s => s.Amount) ?? 0;

                        decimal totalMinimum = unitOfWork.Bills.Find(f => f.UserID == UserData.ID && f.Minimum != null && f.EndDate >= shift.StartDate && f.EndDate <= DateTime.Now).Sum(s => s.Minimum) ?? 0;

                        decimal totalDevices = unitOfWork.Bills.Find(f => f.UserID == UserData.ID && f.EndDate >= shift.StartDate && f.EndDate <= DateTime.Now).Sum(s => s.DevicesSum) ?? 0;

                        decimal totalItems = (unitOfWork.Bills.Find(f => f.UserID == UserData.ID && f.EndDate >= shift.StartDate && f.EndDate <= DateTime.Now).Sum(s => s.ItemsSum) ?? 0) + itemsBillTotal;

                        decimal totalDiscount = unitOfWork.Bills.Find(f => f.UserID == UserData.ID && f.EndDate >= shift.StartDate && f.EndDate <= DateTime.Now).Sum(s => s.Discount) ?? 0;

                        Shift = new FinishShiftDataModel
                        {
                            NewShift = true,
                            CurrentUserName = shift.User.Name,
                            SafeStart = shift.SafeStart,
                            StartDate = shift.StartDate,
                            TotalDevices = totalDevices,
                            TotalDiscount = totalDiscount,
                            TotalItems = totalItems,
                            TotalMinimum = totalMinimum,
                            Income = safeIncome + itemsBillTotal,
                            Spending = spending
                        };
                        Shift.Total = shift.SafeStart + Shift.Income - spending;
                    }
                    finishShiftDialog.DataContext = this;
                    await currentWindow.HideMetroDialogAsync(finishShiftConfirmDialog);
                    await currentWindow.ShowMetroDialogAsync(finishShiftDialog);
                }
                else
                {
                    await currentWindow.HideMetroDialogAsync(finishShiftConfirmDialog);
                    await currentWindow.ShowMessageAsync("فشل الدخول", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "موافق",
                        DialogMessageFontSize = 25,
                        DialogTitleFontSize = 30
                    });
                    await currentWindow.ShowMetroDialogAsync(finishShiftConfirmDialog);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        private bool CanExecuteSignIn()
        {
            return !LoginModel.HasErrors;
        }

        private RelayCommand _finishShift;
        public RelayCommand FinishShift
        {
            get
            {
                return _finishShift ?? (_finishShift = new RelayCommand(
                    ExecuteFinishShiftAsync,
                    CanExecuteFinishShift));
            }
        }
        private async void ExecuteFinishShiftAsync()
        {
            try
            {
                if (Shift.SafeEnd == null)
                    return;
                if (Shift.NewShift == true && (Shift.UserName == null || Shift.Password == null))
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    string oldUSer = UserData.Name;
                    if (_shift.NewShift)
                    {
                        var user = unitOfWork.Users.SingleOrDefault(s => s.Name == _shift.UserName && s.Password == _shift.Password && s.IsWorked == true);

                        if (user == null)
                        {
                            Mouse.OverrideCursor = null;
                            await currentWindow.ShowMessageAsync("فشل الإنهاء", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                            {
                                AffirmativeButtonText = "موافق",
                                DialogMessageFontSize = 25,
                                DialogTitleFontSize = 30
                            });
                            return;
                        }

                        else if (user.Role.Name == RoleText.Admin)
                        {
                            Mouse.OverrideCursor = null;
                            await currentWindow.ShowMessageAsync("فشل الإنهاء", "لا تملك الصلاحية الكاشير فقط من يستطيع بداية شيفت جديد", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                            {
                                AffirmativeButtonText = "موافق",
                                DialogMessageFontSize = 25,
                                DialogTitleFontSize = 30
                            });
                            return;
                        }
                        else
                        {
                            UserData.ID = user.ID;
                            UserData.Role = user.Role.Name;
                            UserData.Name = user.Name;
                            UserData.Password = user.Password;
                        }
                    }

                    DateTime endDate = DateTime.Now;
                    Bill bill = unitOfWork.Bills.SingleOrDefault(s => s.EndDate == null && s.Type == BillTypeText.Items);
                    if (bill != null)
                    {
                        bill.UserID = UserData.ID;
                        bill.ItemsSum = unitOfWork.BillsItems.Find(f => f.BillID == bill.ID).Sum(s => s.Total) ?? 0;
                        bill.Total = bill.ItemsSum;
                        bill.TotalAfterDiscount = bill.ItemsSum;
                        bill.Discount = 0;
                        bill.Ratio = 0;
                        bill.Date = DateTime.Now;
                        bill.EndDate = endDate;
                        unitOfWork.Bills.Edit(bill);

                        if (bill.TotalAfterDiscount > 0)
                        {
                            Safe safe = new Safe
                            {
                                Amount = bill.TotalAfterDiscount,
                                CanDelete = false,
                                Statement = "طلبات خارجية ",
                                UserID = UserData.ID,
                                RegistrationDate = endDate,
                                Type = true
                            };
                            unitOfWork.Safes.Add(safe);
                        }
                        else
                        {
                            unitOfWork.Bills.Remove(bill);
                        }
                    }

                    var shift = unitOfWork.Shifts.SingleOrDefault(s => s.EndDate == null);
                    shift.EndDate = endDate;
                    shift.Income = _shift.Income;
                    shift.SafeEnd = _shift.SafeEnd;
                    shift.Spending = _shift.Spending;
                    shift.Total = _shift.Total;
                    shift.TotalDevices = _shift.TotalDevices;
                    shift.TotalDiscount = _shift.TotalDiscount;
                    shift.TotalItems = _shift.TotalItems;
                    shift.TotalMinimum = _shift.TotalMinimum;
                    unitOfWork.Shifts.Edit(shift);

                    if (_shift.NewShift)
                    {
                        Shift newShift = new Shift
                        {
                            StartDate = DateTime.Now,
                            UserID = UserData.ID,
                            SafeStart = _shift.SafeEnd
                        };
                        unitOfWork.Shifts.Add(newShift);
                    }
                    else
                    {
                        UserData.Role = "";
                        UserData.Name = "";
                        UserData.Password = "";
                        UserData.ID = 0;
                    }
                    unitOfWork.Complete();

                    string path = "";
                    bool exists;
                    try
                    {
                        path = @"D:\JoyDB";
                        exists = Directory.Exists(path);
                        if (!exists)
                            Directory.CreateDirectory(path);
                    }
                    catch
                    {

                    }
                    try
                    {
                        path = @"E:\JoyDB";
                        exists = Directory.Exists(path);
                        if (!exists)
                            Directory.CreateDirectory(path);
                    }
                    catch
                    {
                        path = @"D:\JoyDB";
                    }
                    using (GeneralDBContext db = new GeneralDBContext())
                    {
                        try
                        {
                            string fileName = path + $"\\JoyDB shift {oldUSer} {DateTime.Now.ToShortDateString().Replace('/', '-')} - {DateTime.Now.ToLongTimeString().Replace(':', '-')}";
                            string dbname = db.Database.Connection.Database;
                            string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                            db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                        }
                        catch
                        {
                        }
                    }
                    Mouse.OverrideCursor = null;
                    await currentWindow.HideMetroDialogAsync(finishShiftDialog);
                    if (!_shift.NewShift)
                    {
                        currentWindow.Close();
                        new MainViewModel().NavigateToViewMethodAsync("SignOut");
                    }
                    else
                    {
                        UserData.newShift = true;
                        currentWindow.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        private bool CanExecuteFinishShift()
        {
            if (Shift.HasErrors)
                return false;
            else if (Shift.NewShift == true && (Shift.UserName == null || Shift.Password == null))
                return false;
            else
                return true;

        }

        // Show Calc 

        private RelayCommand _showCalc;
        public RelayCommand ShowCalc
        {
            get
            {
                return _showCalc ?? (_showCalc = new RelayCommand(
                    ExecuteShowCalc));
            }
        }
        private void ExecuteShowCalc()
        {
            try
            {
                Process.Start("calc.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    case "ClientCheck":
                        await currentWindow.HideMetroDialogAsync(clientCheckDialog);
                        break;
                    case "AddClient":
                        await currentWindow.HideMetroDialogAsync(clientAddDialog);
                        break;
                    case "FinishShift":
                        await currentWindow.HideMetroDialogAsync(finishShiftDialog);
                        break;
                    case "Login":
                        await currentWindow.HideMetroDialogAsync(finishShiftConfirmDialog);
                        break;
                    case "CancelBill":
                        await currentWindow.HideMetroDialogAsync(cancelReasonDialog);
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

        private void UpdateData()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Devices = new ObservableCollection<DevicePlayDataModel>(unitOfWork.Devices.GetAvailable());
                BusyDevices = _devices.Where(w => w.Device.Case == DeviceCaseText.Busy).Count();
                AvailableDevices = _devices.Where(w => w.Device.Case == DeviceCaseText.Free).Count();
                TemporaryDevices = _devices.Where(w => w.Device.Case == DeviceCaseText.Paused).Count();
            }
        }
    }
}
