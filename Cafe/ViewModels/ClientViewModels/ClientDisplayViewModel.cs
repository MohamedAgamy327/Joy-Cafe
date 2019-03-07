﻿using MahApps.Metro.Controls;
using Cafe.Views.ClientViews;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using Excel = Microsoft.Office.Interop.Excel;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL;
using Utilities.Paging;
using DTO.ClientDataModel;
using DAL.Entities;
using System.Windows.Input;

namespace Cafe.ViewModels.ClientViewModels
{
    public class ClientDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly ClientAddDialog _clientAddDialog;
        private readonly ClientUpdateDialog _clientUpdateDialog;

        private void Load(bool isNew)
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Clients.GetRecordsNumber(isNew, _key);
                Paging.GetFirst();
                Clients = new ObservableCollection<ClientDisplayDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ClientDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            _clientAddDialog = new ClientAddDialog();
            _clientUpdateDialog = new ClientUpdateDialog();       
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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
            set
            {
                SetProperty(ref _key, value);
            }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ClientDisplayDataModel _selectedClient;
        public ClientDisplayDataModel SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        private ClientAddDataModel _newClient;
        public ClientAddDataModel NewClient
        {
            get { return _newClient; }
            set { SetProperty(ref _newClient, value); }
        }

        private ClientUpdateDataModel _clientUpdate;
        public ClientUpdateDataModel ClientUpdate
        {
            get { return _clientUpdate; }
            set { SetProperty(ref _clientUpdate, value); }
        }

        private ObservableCollection<ClientDisplayDataModel> _clients;
        public ObservableCollection<ClientDisplayDataModel> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        //Display

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
                Load(true);
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
                Load(false);
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
                    Clients = new ObservableCollection<ClientDisplayDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Clients = new ObservableCollection<ClientDisplayDataModel>(unitOfWork.Clients.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Clients.Remove(_selectedClient.Client);
                    unitOfWork.Complete();
                }
                Load(true);
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
                    ?? (_export = new RelayCommand(ExportMethod));
            }
        }
        private void ExportMethod()
        {
            try
            {
                if (Clients.Count == 0)
                    return;

                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.ShowDialog();
                if (fbd.SelectedPath == "")
                    return;

                Mouse.OverrideCursor = Cursors.Wait;
                int i = 2;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "العميل";
                xlWorkSheet.Cells[1, 2] = "التليفون";
                foreach (var item in Clients)
                {
                    xlWorkSheet.Cells[i, 2].NumberFormat = "@";
                    xlWorkSheet.Cells[i, 1] = item.Client.Name;
                    xlWorkSheet.Cells[i, 2] = item.Client.Telephone;
                    i++;
                }
                xlWorkBook.SaveAs(fbd.SelectedPath + @"\العملاء.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(ex.ToString());
            }
        }

        private void releaseObject(object obj)
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

        // Add Client

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
                NewClient = new ClientAddDataModel();
                _clientAddDialog.DataContext = this;
                await _currentWindow.ShowMetroDialogAsync(_clientAddDialog);
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
                if (NewClient.Telephone == null || NewClient.Name == null || NewClient.Code == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var client = unitOfWork.Clients.SingleOrDefault(s => s.Code == _newClient.Code || s.Telephone == _newClient.Telephone);
                    if (client != null)
                    {
                        await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالعميل موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        unitOfWork.Clients.Add(new Client
                        {
                            Code = _newClient.Code,
                            Name = _newClient.Name,
                            Telephone = _newClient.Telephone
                        });
                        unitOfWork.Complete();
                        NewClient = new ClientAddDataModel();
                        Load(true);
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
            return !NewClient.HasErrors && !HasErrors;
        }

        // Update Account

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
                _clientUpdateDialog.DataContext = this;
                ClientUpdate = new ClientUpdateDataModel();
                ClientUpdate.Name = _selectedClient.Client.Name;
                ClientUpdate.Code = _selectedClient.Client.Code;
                ClientUpdate.Telephone = _selectedClient.Client.Telephone;
                ClientUpdate.ID = _selectedClient.Client.ID;
                await _currentWindow.ShowMetroDialogAsync(_clientUpdateDialog);
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
                if (_selectedClient.Client.Telephone == null || _selectedClient.Client.Name == null || _selectedClient.Client.Code == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var client = unitOfWork.Clients.SingleOrDefault(s => (s.Telephone == _clientUpdate.Telephone || s.Code == _clientUpdate.Code) && s.ID != _clientUpdate.ID);
                    if (client != null)
                    {
                        await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالعميل موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        SelectedClient.Client.Name = ClientUpdate.Name;
                        SelectedClient.Client.Code = ClientUpdate.Code;
                        SelectedClient.Client.Telephone = ClientUpdate.Telephone;
                        unitOfWork.Clients.Edit(_selectedClient.Client);
                        unitOfWork.Complete();
                        await _currentWindow.HideMetroDialogAsync(_clientUpdateDialog);
                        Load(true);
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
                return !ClientUpdate.HasErrors;
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
                        await _currentWindow.HideMetroDialogAsync(_clientAddDialog);
                        break;
                    case "Update":
                        await _currentWindow.HideMetroDialogAsync(_clientUpdateDialog);
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
