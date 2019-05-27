using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using System;
using DAL.BindableBaseService;
using DAL.Entities;
using DTO.BillItemDataModel;
using BLL.UnitOfWorkService;
using DAL;
using Cafe.Views.CashierViews.BillItemsViews;
using System.Windows.Input;
using Cafe.Reports;
using DTO.UserDataModel;
using System.Collections.Generic;
using DTO.ItemDataModel;

namespace Cafe.ViewModels.CashierViewModels
{
    public class BillItemsViewModel : ValidatableBindableBase
    {
        public static int BillID { get; set; }

        MetroWindow currentWindow;

        private readonly BillItemAddDialog billItemAddDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                BillItems = new ObservableCollection<BillItemDisplayDataModel>(unitOfWork.BillsItems.GetBillItems(BillID));
            }
        }

        public BillItemsViewModel()
        {
            _isFocused = true;
            billItemAddDialog = new BillItemAddDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private decimal _itemsSum;
        public decimal ItemsSum
        {
            get
            {
                if (BillItems != null && BillItems.Count > 0)
                    return _itemsSum = BillItems.Sum(s => Convert.ToDecimal(s.BillItem.Total));
                else
                    return 0;
            }
        }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private BillItemAddDataModel _newBillItem;
        public BillItemAddDataModel NewBillItem
        {
            get { return _newBillItem; }
            set { SetProperty(ref _newBillItem, value); }
        }

        private BillItemDisplayDataModel _selectedBillItem;
        public BillItemDisplayDataModel SelectedBillItem
        {
            get { return _selectedBillItem; }
            set { SetProperty(ref _selectedBillItem, value); }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<BillItemDisplayDataModel> _billItems;
        public ObservableCollection<BillItemDisplayDataModel> BillItems
        {
            get { return _billItems; }
            set
            {
                SetProperty(ref _billItems, value);
                OnPropertyChanged("ItemsSum");
            }
        }

        private List<ItemPrintDataModel> _newItems;
        public List<ItemPrintDataModel> NewItems
        {
            get { return _newItems; }
            set { SetProperty(ref _newItems, value); }
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
                    Items = new ObservableCollection<Item>(unitOfWork.Items.Find(f => f.IsAvailable == true).OrderByDescending(o => o.BillsItems.Count).ThenBy(o => o.Name));
                }
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _deleteItem;
        public RelayCommand DeleteItem
        {
            get
            {
                return _deleteItem
                    ?? (_deleteItem = new RelayCommand(DeleteItemMethodAsync));
            }
        }
        private async void DeleteItemMethodAsync()
        {
            try
            {
                MessageDialogResult result = await currentWindow.ShowMessageAsync("تأكيد الحذف", "هل تـريــد حــذف هـذا الطلب؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    NegativeButtonText = "الغاء",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
                if (result == MessageDialogResult.Affirmative)
                {
                    using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                    {
                        unitOfWork.BillsItems.Remove(_selectedBillItem.BillItem);
                        unitOfWork.Complete();
                    }
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand<BillItemDisplayDataModel> _qtyChanged;
        public RelayCommand<BillItemDisplayDataModel> QtyChanged
        {
            get
            {
                return _qtyChanged
                    ?? (_qtyChanged = new RelayCommand<BillItemDisplayDataModel>(QtyChangedMethodAsync));
            }
        }
        private async void QtyChangedMethodAsync(BillItemDisplayDataModel selectedBillItem)
        {
            try
            {
                if (selectedBillItem.BillItem.Qty == null)
                    return;

                MessageDialogResult result = await currentWindow.ShowMessageAsync("تأكيد العملية", "هل تـريــد تغير هـذه الكمية؟", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "موافق",
                    NegativeButtonText = "الغاء",
                    DialogMessageFontSize = 25,
                    DialogTitleFontSize = 30
                });
                if (result == MessageDialogResult.Affirmative)
                {
                    using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                    {
                        selectedBillItem.BillItem.Total = selectedBillItem.BillItem.Qty * selectedBillItem.BillItem.Price;
                        unitOfWork.BillsItems.Edit(selectedBillItem.BillItem);
                        unitOfWork.Complete();
                    }
                    OnPropertyChanged("ItemsSum");
                    OnPropertyChanged("ItemsNumber");
                }
                else
                {
                    Load();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Add

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
                NewBillItem = new BillItemAddDataModel();
                _newItems = new List<ItemPrintDataModel>();
                billItemAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(billItemAddDialog);
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
                    ExecuteSave,
                    CanExecuteSave));
            }
        }
        private void ExecuteSave()
        {
            try
            {
                if (NewBillItem.Qty == null || SelectedItem.Name == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.BillsItems.Add(new BillItem
                    {
                        BillID = BillID,
                        ItemID = _newBillItem.ItemID,
                        Price = _selectedItem.Price,
                        Qty = _newBillItem.Qty,
                        RegistrationDate = DateTime.Now,
                        Total = _newBillItem.Qty * _selectedItem.Price
                    });
                    unitOfWork.Complete();
                }
                _newItems.Add(new ItemPrintDataModel { Qty = _newBillItem.Qty, Name = _selectedItem.Name });
                Load();
                NewBillItem = new BillItemAddDataModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSave()
        {
            try
            {
                if (NewBillItem.HasErrors)
                    return false;
                else
                    return true;
            }
            catch
            { return false; }
        }

        private RelayCommand _print;
        public RelayCommand Print
        {
            get
            {
                return _print
                    ?? (_print = new RelayCommand(PrintMethod));
            }
        }
        private void PrintMethod()
        {
            try
            {
                // Account Print
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    DS ds = new DS();
                    ds.BillItems.Rows.Clear();
                    int i = 0;
                    Device device = unitOfWork.Devices.FirstOrDefault(s => s.BillID == BillID);

                    foreach (var item in _billItems)
                    {
                        ds.BillItems.Rows.Add();
                        ds.BillItems[i]["Cashier"] = UserData.Name;
                        ds.BillItems[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                        ds.BillItems[i]["Device"] = device.DeviceType.Name + " ( " + device.Name + " )";
                        ds.BillItems[i]["Qty"] = item.BillItem.Qty;
                        ds.BillItems[i]["Item"] = item.Item.Name;
                        i++;
                    }

                    //ReportWindow rpt = new ReportWindow();
                    ItemsOnlyReport itemsOnlyReport = new ItemsOnlyReport();
                    itemsOnlyReport.SetDataSource(ds.Tables["BillItems"]);
                    //rpt.crv.ViewerCore.ReportSource = itemsOnlyReport;
                    Mouse.OverrideCursor = null;
                    //rpt.ShowDialog();
                    itemsOnlyReport.PrintToPrinter(1, false, 0, 15);
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
                        await currentWindow.HideMetroDialogAsync(billItemAddDialog);
                        PrintNewItems();
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

        private void PrintNewItems()
        {
            try
            {
                if (_newItems.Count == 0)
                    return;
                // Account Print
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    DS ds = new DS();
                    ds.BillItems.Rows.Clear();
                    int i = 0;
                    Device device = unitOfWork.Devices.FirstOrDefault(s => s.BillID == BillID);

                    foreach (var item in _newItems)
                    {
                        ds.BillItems.Rows.Add();
                        ds.BillItems[i]["Cashier"] = UserData.Name;
                        ds.BillItems[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                        ds.BillItems[i]["Device"] = device.DeviceType.Name + " ( " + device.Name + " )";
                        ds.BillItems[i]["Qty"] = item.Qty;
                        ds.BillItems[i]["Item"] = item.Name;
                        i++;
                    }

                    //ReportWindow rpt = new ReportWindow();
                    ItemsOnlyReport itemsOnlyReport = new ItemsOnlyReport();
                    itemsOnlyReport.SetDataSource(ds.Tables["BillItems"]);
                    //rpt.crv.ViewerCore.ReportSource = itemsOnlyReport;
                    Mouse.OverrideCursor = null;
                    //rpt.ShowDialog();
                    itemsOnlyReport.PrintToPrinter(1, false, 0, 15);
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
    }
}
