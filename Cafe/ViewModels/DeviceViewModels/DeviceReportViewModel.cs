using BLL.UnitOfWorkService;
using DAL;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillDeviceDataModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Cafe.ViewModels.DeviceViewModels
{
    public class DeviceReportViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Types = new ObservableCollection<DeviceReportDataModel>(unitOfWork.BillsDevices.Search(_selectedDevice.ID, _dateFrom, _dateTo));
                TotalAmount = _types.Sum(s => s.Amount);
                var totalMinuts = _types.Sum(s => s.Minutes);
                TotalHours = ((int)totalMinuts / 60).ToString("D2") + ":" + ((int)totalMinuts % 60).ToString("D2");
            }
        }

        public DeviceReportViewModel()
        {
            _dateFrom = DateTime.Now;
            _dateTo = DateTime.Now;
        }

        private string _totalHours;
        public string TotalHours
        {
            get { return _totalHours; }
            set { SetProperty(ref _totalHours, value); }
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

        private ObservableCollection<DeviceReportDataModel> _types;
        public ObservableCollection<DeviceReportDataModel> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
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
                if (Types.Count == 0)
                    return;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "الاجهزه",
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
                xlWorkSheet.Cells[1, 1] = "نوع الجهاز";
                xlWorkSheet.Cells[1, 2] = "النوع";
                xlWorkSheet.Cells[1, 3] = "عدد الساعات";
                xlWorkSheet.Cells[1, 4] = "اجمالى الفلوس";

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {

                    foreach (var type in _types)
                    {
                        xlWorkSheet.Cells[i, 1] = type.DeviceType.Name;
                        xlWorkSheet.Cells[i, 2] = type.Type;
                        xlWorkSheet.Cells[i, 3] = type.Hours;
                        xlWorkSheet.Cells[i, 4] = type.Amount;
                        i++;
                    }
                    xlWorkSheet.Cells[i + 1, 3] = "إجمالى الساعات";
                    xlWorkSheet.Cells[i + 2, 3] = _totalHours;
                    xlWorkSheet.Cells[i + 1, 4] = "مجموع إجمالى المبلغ";
                    xlWorkSheet.Cells[i + 2, 4] = _totalAmount;
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
            if (Types == null || Types.Count == 0)
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
