using BLL.UnitOfWorkService;
using Cafe.Reports;
using DAL;
using DAL.BindableBaseService;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDataModel;
using DTO.BillDeviceDataModel;
using DTO.BillItemDataModel;
using DTO.UserDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cafe.ViewModels.CashierViewModels
{
    public class AccountPaidViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        public AccountPaidViewModel()
        {
            _billPaid = new BillPaidDataModel();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private Visibility _isMembership;
        public Visibility IsMembership
        {
            get { return _isMembership; }
            set { SetProperty(ref _isMembership, value); }
        }

        private Bill _selectedBill;
        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set { SetProperty(ref _selectedBill, value); }
        }

        private BillPaidDataModel _billPaid;
        public BillPaidDataModel BillPaid
        {
            get { return _billPaid; }
            set { SetProperty(ref _billPaid, value); }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private ObservableCollection<BillItemDisplayDataModel> _billItems;
        public ObservableCollection<BillItemDisplayDataModel> BillItems
        {
            get { return _billItems; }
            set
            {
                SetProperty(ref _billItems, value);
                OnPropertyChanged("ItemsSum");
                OnPropertyChanged("TotalSum");
            }
        }

        private ObservableCollection<BillDeviceDisplayDataModel> _billDevices;
        public ObservableCollection<BillDeviceDisplayDataModel> BillDevices
        {
            get { return _billDevices; }
            set
            {
                SetProperty(ref _billDevices, value);
                OnPropertyChanged("DevicesSum");
                OnPropertyChanged("TotalSum");
            }
        }

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
                    IsMembership = Visibility.Visible;
                    BillDevices = new ObservableCollection<BillDeviceDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(BillData.BillID));
                    BillItems = new ObservableCollection<BillItemDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(BillData.BillID));
                    SelectedClient = unitOfWork.Clients.GetById(BillData.ClientID);
                    SelectedBill = unitOfWork.Bills.GetById(BillData.BillID);
                    Device device = unitOfWork.Devices.GetById(BillData.DeviceID);

                    SelectedBill.PlayedMinutes = BillDevices.Sum(s => s.Duration);
                    SelectedBill.ItemsSum = BillItems.Sum(s => Convert.ToDecimal(s.BillItem.Total));
                    SelectedBill.CurrentPoints = SelectedClient.Points ?? 0;
                    SelectedBill.PointsAfterUsed = SelectedBill.CurrentPoints - BillPaid.UsedPoints;

                    foreach (var item in BillDevices)
                    {
                        if (item.BillDevice.GameType == GamePlayTypeText.Birthday)
                        {
                            IsMembership = Visibility.Collapsed;
                            break;
                        }
                    }

                    var billDevicesCount = BillDevices.Select(k => new { k.DeviceType.Name }).Distinct().Count();
                    var cmm = unitOfWork.ClientMembershipMinutes.GetByDeviceTypeClient(device.DeviceTypeID,BillData.ClientID);
                    if (billDevicesCount > 1 || cmm == null || cmm.Minutes == 0)
                        IsMembership = Visibility.Collapsed;

                    if (IsMembership != Visibility.Collapsed)
                    {
                        SelectedBill.CurrentMembershipMinutes = cmm.Minutes;
                        SelectedBill.MembershipMinutesAfterPaid = cmm.Minutes > SelectedBill.PlayedMinutes ? cmm.Minutes - SelectedBill.PlayedMinutes : 0;
                        SelectedBill.RemainderMinutes = SelectedBill.MembershipMinutesAfterPaid > 0 ? 0 : SelectedBill.PlayedMinutes - cmm.Minutes;

                        if (SelectedBill.RemainderMinutes > 0)
                        {
                            var avg = (BillDevices.Sum(s => s.BillDevice.MinutePrice)) / BillDevices.Count();
                            SelectedBill.DevicesSum = avg * SelectedBill.RemainderMinutes;
                        }
                        else
                        {
                            SelectedBill.DevicesSum = 0;
                        }
                    }
                    else
                        SelectedBill.DevicesSum = BillDevices.Sum(s => Convert.ToDecimal(s.Total));

                    SelectedBill.Total = _selectedBill.DevicesSum + _selectedBill.ItemsSum;
                    SelectedBill.TotalAfterDiscount = SelectedBill.Total;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _ratioChanged;
        public RelayCommand RatioChanged
        {
            get
            {
                return _ratioChanged
                    ?? (_ratioChanged = new RelayCommand(RatioChangedMethod));
            }
        }
        private void RatioChangedMethod()
        {
            try
            {
                BillPaid.Discount = (BillPaid.Ratio * SelectedBill.Total) / 100;
                SelectedBill.TotalAfterDiscount = Math.Round(Convert.ToDecimal(SelectedBill.Total - BillPaid.Discount), 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _discountChanged;
        public RelayCommand DiscountChanged
        {
            get
            {
                return _discountChanged
                    ?? (_discountChanged = new RelayCommand(DiscountChangedMethod));
            }
        }
        private void DiscountChangedMethod()
        {
            try
            {
                if (SelectedBill.Total != 0)
                    BillPaid.Ratio = (BillPaid.Discount * 100) / SelectedBill.Total;
                else
                    BillPaid.Ratio = 0;
                SelectedBill.TotalAfterDiscount = Math.Round(Convert.ToDecimal(SelectedBill.Total - BillPaid.Discount), 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _usedPointsChanged;
        public RelayCommand UsedPointsChanged
        {
            get
            {
                return _usedPointsChanged
                    ?? (_usedPointsChanged = new RelayCommand(UsedPointsChangedMethod));
            }
        }
        private void UsedPointsChangedMethod()
        {
            try
            {
                if (BillPaid.UsedPoints != null)
                    SelectedBill.PointsAfterUsed = SelectedBill.CurrentPoints - BillPaid.UsedPoints;
                else
                    SelectedBill.PointsAfterUsed = 0;
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
                return _save
                    ?? (_save = new RelayCommand(SaveMethod, CanExecuteSave));
            }
        }
        private void SaveMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    if (BillPaid.Minimum == null && (SelectedBill.TotalAfterDiscount == null || BillPaid.Discount > SelectedBill.Total))
                        return;

                    Device device = unitOfWork.Devices.GetById(BillData.DeviceID);
                    if (device.Case == DeviceCaseText.Busy)
                    {
                        BillDevice billDevice = unitOfWork.BillsDevices.GetByBill(BillData.BillID);
                        billDevice.EndDate = BillData.EndDate;
                        billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(billDevice.EndDate) - billDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(billDevice);
                    }
                    device.Case = DeviceCaseText.Free;
                    device.BillID = null;
                    unitOfWork.Devices.Edit(device);

                    _selectedBill.UserID = UserData.ID;
                    _selectedBill.EndDate = BillData.EndDate;
                    _selectedBill.Date = BillData.EndDate;
                    _selectedBill.ClientID = _selectedClient.ID;
                    _selectedBill.UsedPoints = _billPaid.UsedPoints;

                    if (BillPaid.Minimum != null && BillPaid.Minimum > 0)
                    {
                        Safe safe = new Safe
                        {
                            Amount = _billPaid.Minimum,
                            CanDelete = false,
                            Statement = "فاتورة للجهاز  " + device.Name,
                            UserID = UserData.ID,
                            RegistrationDate = BillData.EndDate,
                            Type = true
                        };
                        unitOfWork.Safes.Add(safe);
                        _selectedBill.Minimum = _billPaid.Minimum;
                        _selectedBill.Total = _billPaid.Minimum;
                        _selectedBill.TotalAfterDiscount = _billPaid.Minimum;
                        _selectedBill.EarnedPoints = 0;
                        _selectedBill.Discount = 0;
                        _selectedBill.Ratio = 0;
                        unitOfWork.Bills.Edit(_selectedBill);
                    }
                    else
                    {
                        _selectedBill.EarnedPoints = Convert.ToInt32(Math.Round(Convert.ToDecimal(SelectedBill.DevicesSum), 0));
                        _selectedBill.Discount = _billPaid.Discount;
                        _selectedBill.Ratio = _billPaid.Ratio;
                        if (IsMembership != Visibility.Collapsed)
                        {
                            var cmm = unitOfWork.ClientMembershipMinutes.GetByDeviceTypeClient(device.DeviceTypeID,BillData.ClientID);
                            cmm.Minutes = (int)_selectedBill.MembershipMinutesAfterPaid;
                            unitOfWork.ClientMembershipMinutes.Edit(cmm);
                            _selectedBill.MembershipMinutesPaid = _selectedBill.CurrentMembershipMinutes - _selectedBill.MembershipMinutesAfterPaid;
                        }
                        unitOfWork.Bills.Edit(_selectedBill);

                        if (_selectedBill.TotalAfterDiscount > 0)
                        {
                            Safe safe = new Safe
                            {
                                Amount = _selectedBill.TotalAfterDiscount,
                                CanDelete = false,
                                Statement = "فاتورة للجهاز  " + device.Name,
                                UserID = UserData.ID,
                                RegistrationDate = BillData.EndDate,
                                Type = true
                            };
                            unitOfWork.Safes.Add(safe);
                        }
                    }

                    _selectedClient.Points = _selectedBill.PointsAfterUsed + _selectedBill.EarnedPoints;
                    unitOfWork.Clients.Edit(_selectedClient);

                    unitOfWork.Complete();
                    currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSave()
        {
            if (BillPaid.HasErrors || ((BillPaid.Discount == null || BillPaid.Ratio == null) && BillPaid.Minimum == null))
                return false;
            else
                return true;
        }

        private RelayCommand _print;
        public RelayCommand Print
        {
            get
            {
                return _print
                    ?? (_print = new RelayCommand(PrintMethod, CanExecutePrint));
            }
        }
        private void PrintMethod()
        {
            try
            {
                if (BillPaid.Minimum == null && (SelectedBill.TotalAfterDiscount == null || BillPaid.Discount > SelectedBill.Total))
                    return;

                SaveMethod();

                // Account Print

                Mouse.OverrideCursor = Cursors.Wait;
                int rnd = new Random().Next(1000, 9999);
                DS ds = new DS();
                ds.Bill.Rows.Clear();
                int i = 0;

                foreach (var item in BillDevices)
                {
                    var hoursPlayed = item.Duration / 60;
                    var minuutesPlayed = item.Duration % 60;
                    ds.Bill.Rows.Add();
                    ds.Bill[i]["BillID"] = $"#{rnd}#{_selectedBill.ID}#";
                    ds.Bill[i]["Date"] = DateTime.Now.ToShortDateString();
                    ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                    ds.Bill[i]["Device"] = item.DeviceType.Name + " ( " + item.Device.Name + " ) : " + item.BillDevice.GameType;
                    ds.Bill[i]["StartDate"] = "Start Time: " + item.BillDevice.StartDate.ToString(" h:mm tt");
                    ds.Bill[i]["EndDate"] = "End Time: " + Convert.ToDateTime(item.EndDate).ToString(" h:mm tt");
                    ds.Bill[i]["TotalTime"] = "Total Time: " + Convert.ToInt32(hoursPlayed).ToString("D2") + ":" + Convert.ToInt32(minuutesPlayed).ToString("D2");
                    ds.Bill[i]["TotalPlayedMoney"] = IsMembership == Visibility.Collapsed ? string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(item.Total), 0)) : "";
                    ds.Bill[i]["Discount"] = string.Format("{0:0.00}", _selectedBill.Discount);
                    ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(_selectedBill.TotalAfterDiscount), 0));
                    i++;
                }
                i = 0;
                foreach (var item in BillItems)
                {
                    ds.Items.Rows.Add();
                    ds.Items[i]["Qty"] = item.BillItem.Qty;
                    ds.Items[i]["Item"] = item.Item.Name;
                    ds.Items[i]["Total"] = string.Format("{0:0.00}", item.BillItem.Total); ;
                    i++;
                }

                if (BillItems.Count == 0)
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
                    //rpt.ShowDialog();
                    billItemsReport.PrintToPrinter(1, false, 0, 15);
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
        private bool CanExecutePrint()
        {
            if (BillPaid.HasErrors || BillPaid.Minimum != null)
                return false;
            if (BillPaid.Discount != null && BillPaid.Ratio != null)
                return true;
            else
                return false;
        }
    }
}
