using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Cafe.Views.ItemViews;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DAL.BindableBaseService;
using Utilities.Paging;
using BLL.UnitOfWorkService;
using DAL;
using DTO.ItemDataModel;

namespace Cafe.ViewModels.ItemViewModels
{
    public class ItemDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly ItemAddDialog itemAddDialog;
        private readonly ItemUpdateDialog itemUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Items.GetRecordsNumber( _key);
                Paging.GetFirst();
                Items = new ObservableCollection<ItemDisplayDataModel>(unitOfWork.Items.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public ItemDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            itemAddDialog = new ItemAddDialog();
            itemUpdateDialog = new ItemUpdateDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set { SetProperty(ref _canEdit, value); }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private ItemDisplayDataModel _selectedItem;
        public ItemDisplayDataModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private ItemAddDataModel _newItem;
        public ItemAddDataModel NewItem
        {
            get { return _newItem; }
            set { SetProperty(ref _newItem, value); }
        }

        private ItemUpdateDataModel _itemUpdate;
        public ItemUpdateDataModel ItemUpdate
        {
            get { return _itemUpdate; }
            set { SetProperty(ref _itemUpdate, value); }
        }

        private ObservableCollection<ItemDisplayDataModel> _items;
        public ObservableCollection<ItemDisplayDataModel> Items
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
                    Items = new ObservableCollection<ItemDisplayDataModel>(unitOfWork.Items.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Items = new ObservableCollection<ItemDisplayDataModel>(unitOfWork.Items.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Items.Remove(_selectedItem.Item);
                    unitOfWork.Complete();
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
                NewItem = new ItemAddDataModel();
                itemAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(itemAddDialog);
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
                if (NewItem.Name == null || NewItem.Price == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var item = unitOfWork.Items.SingleOrDefault(s => s.Name == _newItem.Name);

                    if (item != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالصنف موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        unitOfWork.Items.Add(new DAL.Entities.Item
                        {
                            IsAvailable = true,
                            Name = _newItem.Name,
                            Price = _newItem.Price
                        });
                        unitOfWork.Complete();
                        NewItem = new ItemAddDataModel();
                        Load();
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
            if (NewItem.HasErrors)
                return false;
            else
                return true;
        }

        // Update

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
                itemUpdateDialog.DataContext = this;
                ItemUpdate = new ItemUpdateDataModel();
                ItemUpdate.Name = _selectedItem.Item.Name;
                ItemUpdate.IsAvailable = _selectedItem.Item.IsAvailable;
                ItemUpdate.Price = _selectedItem.Item.Price;
                ItemUpdate.ID = _selectedItem.Item.ID;

                await currentWindow.ShowMetroDialogAsync(itemUpdateDialog);
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
                if (ItemUpdate.Name == null || ItemUpdate.Price == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var item = unitOfWork.Items.SingleOrDefault(s => s.Name == ItemUpdate.Name && s.ID != ItemUpdate.ID);
                    if (item != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذاالصنف موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        SelectedItem.Item.Name = ItemUpdate.Name;
                        SelectedItem.Item.Price = ItemUpdate.Price;
                        SelectedItem.Item.IsAvailable = ItemUpdate.IsAvailable;
                        unitOfWork.Items.Edit(_selectedItem.Item);
                        unitOfWork.Complete();
                        await currentWindow.HideMetroDialogAsync(itemUpdateDialog);
                        Load();
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
                return !ItemUpdate.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(itemAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(itemUpdateDialog);
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
