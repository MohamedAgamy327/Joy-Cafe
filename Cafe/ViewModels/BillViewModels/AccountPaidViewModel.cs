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

namespace Cafe.ViewModels.BillViewModels
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
                    BillDevices = new ObservableCollection<BillDevicesDisplayDataModel>(unitOfWork.BillsDevices.GetBillDevices(BillData.BillID));
                    BillItems = new ObservableCollection<BillItemsDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(BillData.BillID));
                    SelectedClient = unitOfWork.Clients.Get(BillData.ClientID);
                    SelectedBill = unitOfWork.Bills.Get(BillData.BillID);
                    Device device = unitOfWork.Devices.Get(BillData.DeviceID);
                    SelectedBill.PlayedMinutes = BillDevices.Sum(s => s.Duration);
                    SelectedBill.ItemsSum = BillItems.Sum(s => Convert.ToDecimal(s.BillItem.Total));

                    foreach (var item in BillDevices)
                    {
                        if (item.BillDevice.GameType == GamePlayTypeText.Birthday)
                        {
                            IsMembership = Visibility.Collapsed;
                            break;
                        }
                    }

                    var billDevicesCount = BillDevices.Select(k => new { k.DeviceType.Name }).Distinct().Count();
                    var cmm = unitOfWork.ClientMembershipMinutes.FirstOrDefault(f => f.ClientID == BillData.ClientID && f.DeviceTypeID == device.DeviceTypeID);
                    if (billDevicesCount > 1 || cmm == null || cmm.Minutes == 0)
                        IsMembership = Visibility.Collapsed;

                    if (IsMembership != Visibility.Collapsed)
                    {
                        SelectedBill.MembershipMinutes = cmm.Minutes;
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

                    Device device = unitOfWork.Devices.Get(BillData.DeviceID);
                    if (device.Case == CaseText.Busy)
                    {
                        BillDevice billDevice = unitOfWork.BillsDevices.SingleOrDefault(f => f.BillID == BillData.BillID && f.EndDate == null);
                        billDevice.EndDate = BillData.EndDate;
                        billDevice.Duration = Convert.ToInt32((Convert.ToDateTime(billDevice.EndDate) - billDevice.StartDate).TotalMinutes);
                        unitOfWork.BillsDevices.Edit(billDevice);
                    }
                    device.Case = CaseText.Free;
                    device.BillID = null;
                    unitOfWork.Devices.Edit(device);
                    _selectedBill.UserID = UserData.ID;
                    _selectedBill.EndDate = BillData.EndDate;
                    _selectedBill.Date = BillData.EndDate;
                    _selectedBill.ClientID = _selectedClient.ID;

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
                        _selectedBill.Point = 0;
                        _selectedBill.Discount = 0;
                        _selectedBill.Ratio = 0;
                        unitOfWork.Bills.Edit(_selectedBill);
                    }
                    else
                    {
                        _selectedBill.Point = SelectedBill.PlayedMinutes / 5;
                        _selectedBill.Discount = _billPaid.Discount;
                        _selectedBill.Ratio = _billPaid.Ratio;
                        if (IsMembership != Visibility.Collapsed)
                        {
                            var cmm = unitOfWork.ClientMembershipMinutes.FirstOrDefault(f => f.ClientID == BillData.ClientID && f.DeviceTypeID == device.DeviceTypeID);
                            cmm.Minutes = (int)_selectedBill.MembershipMinutesAfterPaid;
                            unitOfWork.ClientMembershipMinutes.Edit(cmm);
                            _selectedBill.MembershipMinutesPaid = _selectedBill.MembershipMinutes - _selectedBill.MembershipMinutesAfterPaid;
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
            if ((BillPaid.Discount == null || BillPaid.Ratio == null) && BillPaid.Minimum == null)
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
                    ?? (_print = new RelayCommand(PrintMethod,CanExecutePrint));
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
                DS ds = new DS();
                ds.Bill.Rows.Clear();
                int i = 0;

                foreach (var item in BillDevices)
                {
                    var hoursPlayed = item.Duration / 60;
                    var minuutesPlayed = item.Duration % 60;
                    ds.Bill.Rows.Add();
                    ds.Bill[i]["BillID"] = "#" + _selectedBill.ID;
                    ds.Bill[i]["Date"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    ds.Bill[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                    ds.Bill[i]["Device"] = item.DeviceType.Name + " ( " + item.Device.Name + " ) : " + item.BillDevice.GameType;
                    ds.Bill[i]["StartDate"] = "Start Time: " + item.BillDevice.StartDate.ToString(" h:mm tt");
                    ds.Bill[i]["EndDate"] = "End Time: " + Convert.ToDateTime(item.EndDate).ToString(" h:mm tt");
                    ds.Bill[i]["TotalTime"] = "Total Time: " + Convert.ToInt32(hoursPlayed).ToString("D2") + ":" + Convert.ToInt32(minuutesPlayed).ToString("D2");
                    ds.Bill[i]["TotalPlayedMoney"] = string.Format("{0:0.00}", Math.Round(Convert.ToDecimal(item.Total), 0));
                    ds.Bill[i]["Discount"] = string.Format("{0:0.00}", _selectedBill.Discount);
                    ds.Bill[i]["BillTotal"] = string.Format("{0:0.00}", _selectedBill.TotalAfterDiscount);
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
                if (IsMembership == Visibility.Collapsed)
                {
                    if (BillItems.Count == 0)
                    {
                        ReportWindow rpt = new ReportWindow();
                        BillReport billReport = new BillReport();
                        billReport.SetDataSource(ds.Tables["Bill"]);
                        rpt.crv.ViewerCore.ReportSource = billReport;
                        Mouse.OverrideCursor = null;
                        rpt.ShowDialog();
                        //billReport.PrintToPrinter(1, false, 0, 15);
                    }
                    else
                    {
                        ReportWindow rpt = new ReportWindow();
                        BillItemsReport billItemsReport = new BillItemsReport();
                        billItemsReport.SetDataSource(ds.Tables["Bill"]);
                        billItemsReport.Subreports[0].SetDataSource(ds.Tables["Items"]);
                        rpt.crv.ViewerCore.ReportSource = billItemsReport;
                        Mouse.OverrideCursor = null;
                        rpt.ShowDialog();

                        //  billItemsReport.PrintToPrinter(1, false, 0, 15);
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
        private bool CanExecutePrint()
        {
            if ((BillPaid.Discount == null || BillPaid.Ratio == null) && BillPaid.Minimum == null)
                return false;
            else
                return true;
        }
    }
}
