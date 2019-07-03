using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using System;
using DAL.BindableBaseService;
using DAL.Entities;
using Cafe.Views.CashierViews.ShiftItemsViews;
using DTO.BillItemDataModel;
using BLL.UnitOfWorkService;
using DAL;
using DAL.ConstString;
using System.Windows.Input;
using Cafe.Reports;
using DTO.UserDataModel;
using System.Collections.Generic;
using DTO.ItemDataModel;

namespace Cafe.ViewModels.CashierViewModels
{
    public class ShiftItemsViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;

        private readonly ShiftItemAddDialog _shiftItemAddDialog;
        private List<Item> items;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                ShiftItems = new ObservableCollection<ShiftItemDisplayDataModel>(unitOfWork.BillsItems.GetShiftItems());
                CheckedSum = ShiftItems.Where(w => w.Checked == true).Sum(s => Convert.ToDecimal(s.BillItem.Total));
            }
        }

        public ShiftItemsViewModel()
        {
            _isFocused = true;
            _shiftItemAddDialog = new ShiftItemAddDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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
            set { SetProperty(ref _key, value); }
        }

        public decimal ItemsSum
        {
            get
            {
                if (ShiftItems != null && ShiftItems.Count > 0)
                    return  ShiftItems.Sum(s => Convert.ToDecimal(s.BillItem.Total));
                else
                    return 0;
            }
        }

        private decimal _checkedSum;
        public decimal CheckedSum
        {
            get { return _checkedSum; }
            set { SetProperty(ref _checkedSum, value); }
        }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private List<ItemPrintDataModel> _newItems;
        public List<ItemPrintDataModel> NewItems
        {
            get { return _newItems; }
            set { SetProperty(ref _newItems, value); }
        }

        private ShiftItemAddDataModel _newShiftItem;
        public ShiftItemAddDataModel NewShiftItem
        {
            get { return _newShiftItem; }
            set { SetProperty(ref _newShiftItem, value); }
        }

        private ShiftItemDisplayDataModel _selectedShiftItem;
        public ShiftItemDisplayDataModel SelectedShiftItem
        {
            get { return _selectedShiftItem; }
            set { SetProperty(ref _selectedShiftItem, value); }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<ShiftItemDisplayDataModel> _shiftItems;
        public ObservableCollection<ShiftItemDisplayDataModel> ShiftItems
        {
            get { return _shiftItems; }
            set
            {
                SetProperty(ref _shiftItems, value);
                OnPropertyChanged("ItemsSum");
            }
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
                    items = unitOfWork.Items.Find(f => f.IsAvailable == true).OrderByDescending(o => o.BillsItems.Count).ThenBy(o => o.Name).ToList();
                }
                Load();
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
                CheckedSum = ShiftItems.Where(w => w.Checked == true).Sum(s => Convert.ToDecimal(s.BillItem.Total));
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
                        unitOfWork.BillsItems.Remove(_selectedShiftItem.BillItem);
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

        private RelayCommand<ShiftItemDisplayDataModel> _qtyChanged;
        public RelayCommand<ShiftItemDisplayDataModel> QtyChanged
        {
            get
            {
                return _qtyChanged
                    ?? (_qtyChanged = new RelayCommand<ShiftItemDisplayDataModel>(QtyChangedMethodAsync));
            }
        }
        private async void QtyChangedMethodAsync(ShiftItemDisplayDataModel selectedShiftItem)
        {
            try
            {
                if (selectedShiftItem.BillItem.Qty == null)
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
                        selectedShiftItem.BillItem.Total = selectedShiftItem.BillItem.Qty * selectedShiftItem.BillItem.Price;
                        unitOfWork.BillsItems.Edit(selectedShiftItem.BillItem);
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
                if (_key == string.Empty)
                {
                    Items = new ObservableCollection<Item>();
                }
                else
                {
                    Items = new ObservableCollection<Item>(items.Where(w => w.Name.Contains(_key)));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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
                Key = string.Empty;
                NewShiftItem = new ShiftItemAddDataModel();
                _newItems = new List<ItemPrintDataModel>();
                _shiftItemAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(_shiftItemAddDialog);
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
                if (NewShiftItem.Qty == null || SelectedItem.Name == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var bill = unitOfWork.Bills.FirstOrDefault(f => f.Type == BillTypeText.Items && f.EndDate == null);
                    if (bill == null)
                    {
                        bill = new Bill
                        {
                            StartDate = DateTime.Now,
                            Type = BillTypeText.Items
                        };
                        unitOfWork.Bills.Add(bill);
                    }

                    unitOfWork.BillsItems.Add(new BillItem
                    {
                        BillID = bill.ID,
                        ItemID = _newShiftItem.ItemID,
                        Price = _selectedItem.Price,
                        Qty = _newShiftItem.Qty,
                        RegistrationDate = DateTime.Now,
                        Total = _newShiftItem.Qty * _selectedItem.Price
                    });

                    unitOfWork.Complete();
                }
                _newItems.Add(new ItemPrintDataModel { Qty = _newShiftItem.Qty, Name = _selectedItem.Name });
                Key = string.Empty;
                Load();
                NewShiftItem = new ShiftItemAddDataModel();
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
                if (NewShiftItem.HasErrors)
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
                var itemsPrint = _shiftItems.Where(w => w.Checked == true).Select(s => new ItemPrintDataModel { Name = s.Item.Name, Qty = s.BillItem.Qty }).ToList();
                if (itemsPrint.Count != 0)
                    PrintItems(itemsPrint);
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
                        await currentWindow.HideMetroDialogAsync(_shiftItemAddDialog);
                        if (_newItems.Count != 0)
                            PrintItems(_newItems);
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

        private void PrintItems(List<ItemPrintDataModel> items)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                DS ds = new DS();
                ds.BillItems.Rows.Clear();
                int i = 0;

                foreach (var item in items)
                {
                    ds.BillItems.Rows.Add();
                    ds.BillItems[i]["Cashier"] = UserData.Name;
                    ds.BillItems[i]["Time"] = DateTime.Now.ToString(" h:mm tt");
                    ds.BillItems[i]["Device"] = "طلبات خارجية";
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
