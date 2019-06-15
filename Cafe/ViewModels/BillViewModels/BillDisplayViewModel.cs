using DAL.BindableBaseService;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using Utilities.Paging;
using System.Collections.ObjectModel;
using DTO.BillDataModel;
using BLL.UnitOfWorkService;
using DAL;
using DAL.ConstString;
using System.Windows;
using System.Linq;
using DAL.Entities;
using Excel = Microsoft.Office.Interop.Excel;
using Cafe.Views.BillViews;
using MahApps.Metro.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                if (_selectedBillCase != null)
                {
                    Paging.TotalRecords = unitOfWork.Bills.GetRecordsNumber(_selectedBillCase.Key, _key, _dateFrom, _dateTo);
                    Paging.GetFirst();
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));

                    if (_selectedBillCase.Key == BillCaseText.All)
                    {
                        AvailableCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == false && w.Deleted == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                        DeletedCount = unitOfWork.Bills.GetRecordsNumber(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                        CanceledCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                    }
                    else if (_selectedBillCase.Key == BillCaseText.Available)
                    {
                        AvailableCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == false && w.Deleted == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                        DeletedCount = 0;
                        CanceledCount = 0;
                    }
                    else if (_selectedBillCase.Key == BillCaseText.Canceled)
                    {
                        AvailableCount = 0;
                        DeletedCount = 0;
                        CanceledCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                    }
                    else if (_selectedBillCase.Key == BillCaseText.Deleted)
                    {
                        AvailableCount = 0;
                        DeletedCount = unitOfWork.Bills.GetRecordsNumber(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                        CanceledCount = 0;
                    }
                    GetSum();
                }
                else
                {
                    Paging.TotalRecords = 0;
                    Paging.GetFirst();
                    Bills = new ObservableCollection<BillDisplayDataModel>();
                    AvailableCount = 0;
                    DeletedCount = 0;
                    CanceledCount = 0;
                    DevicesSum = 0;
                    ItemsSum = 0;
                    Discount = 0;
                    TotalAfterDiscount = 0;
                }
            }
        }

        public BillDisplayViewModel()
        {
            _key = "";
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _paging = new PagingWPF();
            _billCases = new ObservableCollection<BillCaseDataModel>()
            { new BillCaseDataModel { Value= "الكل",Key= BillCaseText.All },
           new BillCaseDataModel { Value= "المتاح",Key= BillCaseText.Available },
           new BillCaseDataModel { Value=  "الملغى",Key= BillCaseText.Canceled },
           new BillCaseDataModel{  Value="المحذوف",Key= BillCaseText.Deleted } };
            _selectedBillCase = _billCases.SingleOrDefault(w => w.Key == BillCaseText.All);
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private int? _availableCount;
        public int? AvailableCount
        {
            get { return _availableCount; }
            set { SetProperty(ref _availableCount, value); }
        }

        private int? _deletedCount;
        public int? DeletedCount
        {
            get { return _deletedCount; }
            set { SetProperty(ref _deletedCount, value); }
        }

        private int? _canceledCount;
        public int? CanceledCount
        {
            get { return _canceledCount; }
            set { SetProperty(ref _canceledCount, value); }
        }

        private decimal? _devicesSum;
        public decimal? DevicesSum
        {
            get { return _devicesSum; }
            set { SetProperty(ref _devicesSum, value); }
        }

        private decimal? _itemsSum;
        public decimal? ItemsSum
        {
            get { return _itemsSum; }
            set { SetProperty(ref _itemsSum, value); }
        }

        private decimal? _discount;
        public decimal? Discount
        {
            get { return _discount; }
            set { SetProperty(ref _discount, value); }
        }

        private decimal? _totalAfterDiscount;
        public decimal? TotalAfterDiscount
        {
            get { return _totalAfterDiscount; }
            set { SetProperty(ref _totalAfterDiscount, value); }
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

        private BillCaseDataModel _selectedBillCase;
        public BillCaseDataModel SelectedBillCase
        {
            get { return _selectedBillCase; }
            set { SetProperty(ref _selectedBillCase, value); }
        }

        private BillDisplayDataModel _selectedBill;
        public BillDisplayDataModel SelectedBill
        {
            get { return _selectedBill; }
            set { SetProperty(ref _selectedBill, value); }
        }

        private ObservableCollection<BillCaseDataModel> _billCases;
        public ObservableCollection<BillCaseDataModel> BillCases
        {
            get { return _billCases; }
            set { SetProperty(ref _billCases, value); }
        }

        private ObservableCollection<BillDisplayDataModel> _bills;
        public ObservableCollection<BillDisplayDataModel> Bills
        {
            get { return _bills; }
            set { SetProperty(ref _bills, value); }
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
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _show;
        public RelayCommand Show
        {
            get
            {
                return _show
                    ?? (_show = new RelayCommand(ShowMethod));
            }
        }
        private void ShowMethod()
        {
            try
            {
                BillShowViewModel.BillID = _selectedBill.Bill.ID;
                currentWindow.Hide();
                new BillShowWindow().ShowDialog();
                currentWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _export;
        public RelayCommand Export
        {
            get
            {
                return _export
                    ?? (_export = new RelayCommand(ExportMethod, CanExecuteExport));
            }
        }
        private void ExportMethod()
        {
            try
            {
                if (Bills.Count == 0)
                    return;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "الفواتير",
                    DefaultExt = ".xls",
                    Filter = "Text documents (.xls)|*.xls"
                };
                bool? result = dlg.ShowDialog();

                if (result != true)
                {
                    return;
                }

                Mouse.OverrideCursor = Cursors.Wait;
                int i = 2;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "الكاشير";
                xlWorkSheet.Cells[1, 2] = "كود الفاتورة";
                xlWorkSheet.Cells[1, 3] = "تاريخ الفاتورة";
                xlWorkSheet.Cells[1, 4] = "العميل";
                xlWorkSheet.Cells[1, 5] = "إجمالى الأجهزه";
                xlWorkSheet.Cells[1, 6] = "إجمالى الطلبات";
                xlWorkSheet.Cells[1, 7] = "الإجمالى";
                xlWorkSheet.Cells[1, 8] = "خصم نسبة";
                xlWorkSheet.Cells[1, 9] = "خصم مبلغ";
                xlWorkSheet.Cells[1, 10] = "الإجمالى بعد الخصم";
                xlWorkSheet.Cells[1, 11] = "الحد الأدنى";
                xlWorkSheet.Cells[1, 12] = "ملغى";
                xlWorkSheet.Cells[1, 13] = "سبب الإلغاء";
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    List<Bill> bills = null;
                    switch (_selectedBillCase.Key)
                    {
                        case BillCaseText.All:
                            bills = unitOfWork.Bills.Find(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                            break;

                        case BillCaseText.Available:
                            bills = unitOfWork.Bills.Find(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                            break;

                        case BillCaseText.Canceled:
                            bills = unitOfWork.Bills.Find(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                            break;

                        case BillCaseText.Deleted:
                            bills = unitOfWork.Bills.Find(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                            break;

                        default:
                            break;
                    }

                    foreach (var item in bills)
                    {
                        xlWorkSheet.Cells[i, 3].NumberFormat = "@";
                        xlWorkSheet.Cells[i, 1] = item.User.Name;
                        xlWorkSheet.Cells[i, 2] = item.ID;
                        xlWorkSheet.Cells[i, 3] = item.Date;
                        xlWorkSheet.Cells[i, 4] = !item.Canceled ? item.Client.Name : "";
                        xlWorkSheet.Cells[i, 5] = item.DevicesSum;
                        xlWorkSheet.Cells[i, 6] = item.ItemsSum;
                        xlWorkSheet.Cells[i, 7] = item.Total;
                        xlWorkSheet.Cells[i, 8] = item.Ratio;
                        xlWorkSheet.Cells[i, 9] = item.Discount;
                        xlWorkSheet.Cells[i, 10] = item.TotalAfterDiscount;
                        xlWorkSheet.Cells[i, 11] = item.Minimum;
                        xlWorkSheet.Cells[i, 12] = item.Canceled ? "نعم" : "لا";
                        xlWorkSheet.Cells[i, 13] = item.CancelReason;
                        i++;
                    }
                    xlWorkSheet.Cells[i + 1, 5] = "إجمالى الأجهزة";
                    xlWorkSheet.Cells[i + 2, 5] = bills.Where(w => w.DevicesSum != null).Sum(s => Convert.ToDecimal(s.DevicesSum));
                    xlWorkSheet.Cells[i + 1, 6] = "إجمالى طلبات الاجهزة";
                    xlWorkSheet.Cells[i + 2, 6] = bills.Where(w => w.ItemsSum != null).Sum(s => Convert.ToDecimal(s.ItemsSum));
                    xlWorkSheet.Cells[i + 1, 9] = "إجمالى الخصومات";
                    xlWorkSheet.Cells[i + 2, 9] = bills.Where(w => w.Discount != null).Sum(s => Convert.ToDecimal(s.Discount));
                    xlWorkSheet.Cells[i + 1, 10] = "إجمالى الفواتير بعد الخصومات";
                    xlWorkSheet.Cells[i + 2, 10] = bills.Where(w => w.TotalAfterDiscount != null).Sum(s => Convert.ToDecimal(s.TotalAfterDiscount));
                }
                xlWorkBook.SaveAs(dlg.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteExport()
        {
            if (Bills == null || Bills.Count == 0)
                return false;
            else
                return true;
        }
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


        private void GetSum()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                List<Bill> bills = null;
                switch (_selectedBillCase.Key)
                {
                    case BillCaseText.All:
                        bills = unitOfWork.Bills.Find(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                        break;

                    case BillCaseText.Available:
                        bills = unitOfWork.Bills.Find(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                        break;

                    case BillCaseText.Canceled:
                        bills = unitOfWork.Bills.Find(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                        break;

                    case BillCaseText.Deleted:
                        bills = unitOfWork.Bills.Find(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo).OrderBy(o => o.ID).ToList();
                        break;

                    default:
                        break;
                }
                DevicesSum = bills.Where(w => w.DevicesSum != null).Sum(s => Convert.ToDecimal(s.DevicesSum));
                ItemsSum = bills.Where(w => w.ItemsSum != null).Sum(s => Convert.ToDecimal(s.ItemsSum));
                Discount = bills.Where(w => w.Discount != null).Sum(s => Convert.ToDecimal(s.Discount));
                TotalAfterDiscount = bills.Where(w => w.TotalAfterDiscount != null).Sum(s => Convert.ToDecimal(s.TotalAfterDiscount));
            }
        }
    }
}
