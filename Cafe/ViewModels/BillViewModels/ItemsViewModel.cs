using Cafe.Helpers;
using System.Collections.ObjectModel;
using Cafe.Views.BillViews;
using MahApps.Metro.Controls;
using System.Windows;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using System;
using DAL.BindableBaseService;

namespace Cafe.ViewModels.BillViewModels
{
    public class ItemsViewModel : ValidatableBindableBase
    {
        //Bill _bill;
        //MetroWindow _currentWindow;
        //BillServices _billServ;
        //ItemServices _itemServ;
        //BillItemServices _billItemServ;

        //private readonly ItemAddDialog _itemAddDialog;

        //private void Load()
        //{
        //    BillItems = new ObservableCollection<BillItem>(_billItemServ.GetItems());
        //}

        //public ItemsViewModel()
        //{
        //    _isFocused = true;
        //    _billServ = new BillServices();
        //    _itemServ = new ItemServices();
        //    _billItemServ = new BillItemServices();
        //    _itemAddDialog = new ItemAddDialog();
        //    _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        //    _items = new ObservableCollection<Item>(_itemServ.GetItems());
        //    Load();
        //}

        //private bool _isFocused;
        //public bool IsFocused
        //{
        //    get { return _isFocused; }
        //    set { SetProperty(ref _isFocused, value); }
        //}
   
        //private decimal _itemsSum;
        //public decimal ItemsSum
        //{
        //    get { return _itemsSum = BillItems.Sum(s => Convert.ToDecimal(s.Total)); }
        //}

        //private int _itemsNumber;
        //public int ItemsNumber
        //{
        //    get { return _itemsNumber = BillItems.Sum(s => (int)s.Qty); }
        //}

        //private Item _selectedItem;
        //public Item SelectedItem
        //{
        //    get { return _selectedItem; }
        //    set { SetProperty(ref _selectedItem, value); }
        //}

        //private BillItem _newBillItem;
        //public BillItem NewBillItem
        //{
        //    get { return _newBillItem; }
        //    set { SetProperty(ref _newBillItem, value); }
        //}

        //private BillItem _selectedBillItem;
        //public BillItem SelectedBillItem
        //{
        //    get { return _selectedBillItem; }
        //    set { SetProperty(ref _selectedBillItem, value); }
        //}

        //private ObservableCollection<Item> _items;
        //public ObservableCollection<Item> Items
        //{
        //    get { return _items; }
        //    set { SetProperty(ref _items, value); }
        //}

        //private ObservableCollection<BillItem> _billItems;
        //public ObservableCollection<BillItem> BillItems
        //{
        //    get { return _billItems; }
        //    set
        //    {
        //        SetProperty(ref _billItems, value);
        //        OnPropertyChanged("ItemsSum");
        //        OnPropertyChanged("ItemsNumber");
        //    }
        //}

        //// Display

        //private RelayCommand _deleteItem;
        //public RelayCommand DeleteItem
        //{
        //    get
        //    {
        //        return _deleteItem
        //            ?? (_deleteItem = new RelayCommand(DeleteItemMethod));
        //    }
        //}
        //private void DeleteItemMethod()
        //{
        //    try
        //    {
        //        _billItemServ.DeleteBillItem(_selectedBillItem);
        //        Load();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }

        //}

        //private RelayCommand _qtyChanged;
        //public RelayCommand QtyChanged
        //{
        //    get
        //    {
        //        return _qtyChanged
        //            ?? (_qtyChanged = new RelayCommand(QtyChangedMethod));
        //    }
        //}
        //private void QtyChangedMethod()
        //{
        //    try
        //    {
        //        if (SelectedBillItem == null || SelectedBillItem.Qty == null)
        //            return;

        //        _billItemServ.UpdateBillItem(_selectedBillItem);
        //        OnPropertyChanged("ItemsSum");
        //        OnPropertyChanged("ItemsNumber");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //// Add

        //private RelayCommand _showAdd;
        //public RelayCommand ShowAdd
        //{
        //    get
        //    {
        //        return _showAdd
        //            ?? (_showAdd = new RelayCommand(ShowAddMethod));
        //    }
        //}
        //private async void ShowAddMethod()
        //{
        //    try
        //    {
        //        NewBillItem = new BillItem();
        //        _itemAddDialog.DataContext = this;
        //        await _currentWindow.ShowMetroDialogAsync(_itemAddDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _save;
        //public RelayCommand Save
        //{
        //    get
        //    {
        //        return _save ?? (_save = new RelayCommand(
        //            ExecuteSave,
        //            CanExecuteSave));
        //    }
        //}
        //private void ExecuteSave()
        //{
        //    try
        //    {
        //        if (NewBillItem.Qty == null || SelectedItem == null)
        //            return;
        //        _bill = _billServ.GetBill();
        //        if (_bill == null)
        //        {
        //            _bill = new Bill
        //            {
        //                StartDate = DateTime.Now,
        //                Type = "Items"
        //            };
        //            _billServ.AddBill(_bill);
        //            _bill.ID = _billServ.GetLastBill();
        //        }
        //        _newBillItem.BillID = _bill.ID;
        //        _newBillItem.Price = SelectedItem.Price;
        //        _newBillItem.RegistrationDate = DateTime.Now;
        //        _billItemServ.AddBillItem(_newBillItem);
        //        NewBillItem = new BillItem();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteSave()
        //{
        //    try
        //    {
        //        if (NewBillItem.HasErrors)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch
        //    { return false; }
        //}

        //private RelayCommand<string> _closeDialog;
        //public RelayCommand<string> CloseDialog
        //{
        //    get
        //    {
        //        return _closeDialog
        //            ?? (_closeDialog = new RelayCommand<string>(ExecuteCloseDialogAsync));
        //    }
        //}
        //private async void ExecuteCloseDialogAsync(string parameter)
        //{
        //    try
        //    {
        //        switch (parameter)
        //        {
        //            case "Add":
        //                await _currentWindow.HideMetroDialogAsync(_itemAddDialog);
        //                Load();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
    }
}
