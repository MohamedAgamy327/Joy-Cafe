using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using Cafe.Views.ShiftViews;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL;
using Utilities.Paging;
using DTO.ShiftDataModel;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cafe.ViewModels.ShiftViewModels
{
    public class ShiftDisplayViewModel : ValidatableBindableBase
    {
        private readonly MetroWindow currentWindow;
        private readonly ShiftShowDialog shiftShowDialog;
        private readonly ShiftsReportDialog shiftsReportDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Shifts.GetRecordsNumber(_key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ShiftDisplayViewModel()
        {
            _key = "";
            _dateTo = DateTime.Now;
            _dateFrom = DateTime.Now;
            _paging = new PagingWPF();
            _shiftsReport = new ShiftsReportDataModel();
            shiftShowDialog = new ShiftShowDialog();
            shiftsReportDialog = new ShiftsReportDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
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

        private ShiftDisplayDataModel _selectedShift;
        public ShiftDisplayDataModel SelectedShift
        {
            get { return _selectedShift; }
            set { SetProperty(ref _selectedShift, value); }
        }

        private ShiftsReportDataModel _shiftsReport;
        public ShiftsReportDataModel ShiftsReport
        {
            get { return _shiftsReport; }
            set { SetProperty(ref _shiftsReport, value); }
        }

        private ObservableCollection<ShiftDisplayDataModel> _shifts;
        public ObservableCollection<ShiftDisplayDataModel> Shifts
        {
            get { return _shifts; }
            set { SetProperty(ref _shifts, value); }
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
                    Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Shifts = new ObservableCollection<ShiftDisplayDataModel>(unitOfWork.Shifts.Search(_key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
        private async void ShowMethod()
        {
            try
            {
                shiftShowDialog.DataContext = this;

                if (_selectedShift.Shift.Total > _selectedShift.Shift.SafeEnd)
                    Message = $"يوجد عجز مالى قدره {_selectedShift.Shift.Total - _selectedShift.Shift.SafeEnd} جنيه";
                else if (_selectedShift.Shift.Total < _selectedShift.Shift.SafeEnd)
                    Message = $"يوجد فائض مالى قدره {_selectedShift.Shift.SafeEnd - _selectedShift.Shift.Total} جنيه";
                else
                    Message = "";

                await currentWindow.ShowMetroDialogAsync(shiftShowDialog);
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
                if (Shifts.Count == 0)
                    return;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "الشيفتات",
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
                xlWorkSheet.Cells[1, 2] = "وقت بداية الشيفت";
                xlWorkSheet.Cells[1, 3] = "وقت نهاية الشيفت";
                xlWorkSheet.Cells[1, 4] = "بداية الشفت";
                xlWorkSheet.Cells[1, 5] = "إجمالى الحد الأدنى";
                xlWorkSheet.Cells[1, 6] = "إجمالى الأجهزه";
                xlWorkSheet.Cells[1, 7] = "إجمالى الطلبات";
                xlWorkSheet.Cells[1, 8] = "إجمالى الخصومات";
                xlWorkSheet.Cells[1, 9] = "الدخل";
                xlWorkSheet.Cells[1, 10] = "المصاريف";
                xlWorkSheet.Cells[1, 11] = "الإجمالى";
                xlWorkSheet.Cells[1, 12] = "الدرج";
                xlWorkSheet.Cells[1, 13] = "الحالة";

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var shifts = unitOfWork.Shifts.Search(_key,_dateFrom,_dateTo);
                    foreach (var item in shifts)
                    {
                        xlWorkSheet.Cells[i, 2].NumberFormat = "@";
                        xlWorkSheet.Cells[i, 3].NumberFormat = "@";
                        xlWorkSheet.Cells[i, 1] = item.User.Name;
                        xlWorkSheet.Cells[i, 2] = item.StartDate;
                        xlWorkSheet.Cells[i, 3] = item.EndDate;
                        xlWorkSheet.Cells[i, 4] = item.SafeStart;
                        xlWorkSheet.Cells[i, 5] = item.TotalMinimum;
                        xlWorkSheet.Cells[i, 6] = item.TotalDevices;
                        xlWorkSheet.Cells[i, 7] = item.TotalItems;
                        xlWorkSheet.Cells[i, 8] = item.TotalDiscount;
                        xlWorkSheet.Cells[i, 9] = item.Income;
                        xlWorkSheet.Cells[i, 10] = item.Spending;
                        xlWorkSheet.Cells[i, 11] = item.Total;
                        xlWorkSheet.Cells[i, 12] = item.SafeEnd;
                        xlWorkSheet.Cells[i, 13] = item.Total > item.SafeEnd ? $"يوجد عجز مالى قدره {item.Total - item.SafeEnd} جنيه" :
                           item.Total > item.SafeEnd ? $"يوجد فائض مالى قدره {item.SafeEnd - item.Total} جنيه" : "";
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
            if (Shifts == null || Shifts.Count == 0)
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

        private RelayCommand _showReport;
        public RelayCommand ShowReport
        {
            get
            {
                return _showReport
                    ?? (_showReport = new RelayCommand(ShowReportMethodAsync, CanExecuteShowReport));
            }
        }
        private async void ShowReportMethodAsync()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    ShiftsReport.TotalMinimum = unitOfWork.Shifts.GetTotalMinimum(_dateFrom,_dateTo);
                    ShiftsReport.TotalDevices = unitOfWork.Shifts.GetTotalDevices(_dateFrom, _dateTo);
                    ShiftsReport.TotalItems = unitOfWork.Shifts.GetTotalItems(_dateFrom, _dateTo);
                    ShiftsReport.TotalDiscount = unitOfWork.Shifts.GetTotalDiscount(_dateFrom, _dateTo);
                    ShiftsReport.TotalSpending = unitOfWork.Shifts.GetTotalSpending(_dateFrom, _dateTo);
                    ShiftsReport.TotalIncome = unitOfWork.Shifts.GetTotalIncome(_dateFrom, _dateTo);
                    ShiftsReport.TotalNet = ShiftsReport.TotalIncome - ShiftsReport.TotalSpending;
                    shiftsReportDialog.DataContext = this;
                    await currentWindow.ShowMetroDialogAsync(shiftsReportDialog);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteShowReport()
        {
            if (Shifts == null || Shifts.Count == 0)
                return false;
            else
                return true;
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
                    case "show":
                        await currentWindow.HideMetroDialogAsync(shiftShowDialog);
                        break;
                    case "report":
                        await currentWindow.HideMetroDialogAsync(shiftsReportDialog);
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
