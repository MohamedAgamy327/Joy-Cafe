using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.ItemDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Utilities.Paging;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cafe.ViewModels.ItemViewModels
{
    public class DevicesItemsReportViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.BillsItems.GetRecordsNumber(_selectedDevice.ID, _dateFrom, _dateTo);
                TotalAmount = unitOfWork.BillsItems.TotalAmount(_selectedDevice.ID, _dateFrom, _dateTo);
                TotalQty = unitOfWork.BillsItems.TotalQty(_selectedDevice.ID, _dateFrom, _dateTo);
                Paging.GetFirst();
                Items = new ObservableCollection<ItemReportDataModel>(unitOfWork.BillsItems.Search(_selectedDevice.ID, Paging.CurrentPage, PagingWPF.PageSize, _dateFrom, _dateTo));
            }
        }

        public DevicesItemsReportViewModel()
        {
            _paging = new PagingWPF();
            _dateFrom = DateTime.Now;
            _dateTo = DateTime.Now;
        }

        private decimal? _totalQty;
        public decimal? TotalQty
        {
            get { return _totalQty; }
            set { SetProperty(ref _totalQty, value); }
        }

        private decimal? _totalAmount;
        public decimal? TotalAmount
        {
            get { return _totalAmount; }
            set { SetProperty(ref _totalAmount, value); }
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

        private Device _selectedDevice;
        public Device SelectedDevice
        {
            get { return _selectedDevice; }
            set { SetProperty(ref _selectedDevice, value); }
        }

        private ObservableCollection<Device> _devices;
        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
            set { SetProperty(ref _devices, value); }
        }

        private ObservableCollection<ItemReportDataModel> _items;
        public ObservableCollection<ItemReportDataModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
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
                    Devices = new ObservableCollection<Device>(unitOfWork.Devices.GetAll());
                    _devices.Add(new Device { ID = 0, Name = "طلبات خارجية" });
                    SelectedDevice = Devices.FirstOrDefault();
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
                    Items = new ObservableCollection<ItemReportDataModel>(unitOfWork.BillsItems.Search(_selectedDevice.ID, Paging.CurrentPage, PagingWPF.PageSize, _dateFrom, _dateTo));
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
                    Items = new ObservableCollection<ItemReportDataModel>(unitOfWork.BillsItems.Search(_selectedDevice.ID, Paging.CurrentPage, PagingWPF.PageSize, _dateFrom, _dateTo));
                }
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
                if (Items.Count == 0)
                    return;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "الطلبات",
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
                xlWorkSheet.Cells[1, 1] = "الصنف";
                xlWorkSheet.Cells[1, 2] = "إجمالى الكمية";
                xlWorkSheet.Cells[1, 3] = "إجمالى المبلغ";

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var items = unitOfWork.BillsItems.Search(_selectedDevice.ID, _dateFrom, _dateTo);

                    foreach (var item in items)
                    {
                        xlWorkSheet.Cells[i, 1] = item.Name;
                        xlWorkSheet.Cells[i, 2] = item.Qty;
                        xlWorkSheet.Cells[i, 3] = item.Amount;
                        i++;
                    }
                    xlWorkSheet.Cells[i + 1, 2] = "مجموع إجمالى الكمية";
                    xlWorkSheet.Cells[i + 2, 2] = _totalQty;
                    xlWorkSheet.Cells[i + 1, 3] = "مجموع إجمالى المبلغ";
                    xlWorkSheet.Cells[i + 2, 3] = _totalAmount;
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
            if (Items == null || Items.Count == 0)
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
    }
}
