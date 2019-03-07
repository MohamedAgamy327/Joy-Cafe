using System.Collections.ObjectModel;
using Cafe.Views.BillViews;
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

namespace Cafe.ViewModels.BillViewModels
{
    public class BillItemsViewModel : ValidatableBindableBase
    {
        public static int BillID { get; set; }

        MetroWindow _currentWindow;

        private readonly BillItemAddDialog _billItemAddDialog;

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
            _billItemAddDialog = new BillItemAddDialog();
            _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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

        private int _itemsNumber;
        public int ItemsNumber
        {
            get
            {
                if (BillItems != null && BillItems.Count > 0)
                    return _itemsNumber = BillItems.Sum(s => (int)s.BillItem.Qty);
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
                OnPropertyChanged("ItemsNumber");
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
                    Items = new ObservableCollection<Item>(unitOfWork.Items.Find(f => f.IsAvailable == true));
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
                    ?? (_deleteItem = new RelayCommand(DeleteItemMethod));
            }
        }
        private void DeleteItemMethod()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.BillsItems.Remove(_selectedBillItem.BillItem);
                    unitOfWork.Complete();
                }
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand _qtyChanged;
        public RelayCommand QtyChanged
        {
            get
            {
                return _qtyChanged
                    ?? (_qtyChanged = new RelayCommand(QtyChangedMethod));
            }
        }
        private void QtyChangedMethod()
        {
            try
            {
                if (SelectedBillItem == null || SelectedBillItem.BillItem.Qty == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _selectedBillItem.BillItem.Total = _selectedBillItem.BillItem.Qty * _selectedBillItem.BillItem.Price;
                    unitOfWork.BillsItems.Edit(_selectedBillItem.BillItem);
                    unitOfWork.Complete();
                    //Load();
                }
                OnPropertyChanged("ItemsSum");
                OnPropertyChanged("ItemsNumber");

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
                _billItemAddDialog.DataContext = this;
                await _currentWindow.ShowMetroDialogAsync(_billItemAddDialog);
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
                if (NewBillItem.Qty == null || SelectedItem == null)
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
                        await _currentWindow.HideMetroDialogAsync(_billItemAddDialog);
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
