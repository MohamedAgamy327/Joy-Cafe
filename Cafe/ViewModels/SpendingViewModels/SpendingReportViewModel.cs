using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using DAL.BindableBaseService;
using DTO.SpendingDataModel;
using BLL.UnitOfWorkService;
using DAL;
using Utilities.Paging;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;

namespace Cafe.ViewModels.SpendingViewModels
{
    public class SpendingReportViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Spendings.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                TotalAmount = unitOfWork.Spendings.GetTotalAmount(_key, _dateFrom, _dateTo);
            }
        }

        public SpendingReportViewModel()
        {
            _key = "";
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new PagingWPF();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
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

        private ObservableCollection<SpendingDisplayDataModel> _spendings;
        public ObservableCollection<SpendingDisplayDataModel> Spendings
        {
            get { return _spendings; }
            set { SetProperty(ref _spendings, value); }
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                if (Spendings.Count == 0)
                    return;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "المصاريف",
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
                xlWorkSheet.Cells[1, 1] = "البيان";
                xlWorkSheet.Cells[1, 2] = "التاريخ";
                xlWorkSheet.Cells[1, 3] = "المستخدم";
                xlWorkSheet.Cells[1, 4] = "المبلغ";
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var spendings = unitOfWork.Spendings.Find(w => (w.Statement + w.User.Name).Contains(_key) && w.RegistrationDate >= _dateFrom && w.RegistrationDate <= _dateTo).OrderByDescending(o => o.RegistrationDate);
                    foreach (var item in spendings)
                    {
                        xlWorkSheet.Cells[i, 2].NumberFormat = "@";
                        xlWorkSheet.Cells[i, 1] = item.Statement;
                        xlWorkSheet.Cells[i, 2] = item.RegistrationDate;
                        xlWorkSheet.Cells[i, 3] = item.User.Name;
                        xlWorkSheet.Cells[i, 4] = item.Amount;
                        i++;
                    }
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
            if (Spendings == null || Spendings.Count == 0)
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
