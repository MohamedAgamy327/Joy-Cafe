using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAL.BindableBaseService;
using Cafe.Views.BillViews.ShiftSpendingViews;
using BLL.UnitOfWorkService;
using DAL;
using DTO.SpendingDataModel;
using DTO.UserDataModel;
using DAL.Entities;

namespace Cafe.ViewModels.BillViewModels
{
    public class ShiftSpendingViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly ShiftSpendingAddDialog shiftSpendingAddDialog;
        private readonly ShiftSpendingUpdateDialog shiftSpendingUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, UserData.ID));
            }
        }

        public ShiftSpendingViewModel()
        {
            _key = "";
            _isFocused = true;
            shiftSpendingAddDialog = new ShiftSpendingAddDialog();
            shiftSpendingUpdateDialog = new ShiftSpendingUpdateDialog();
            currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get
            {
                if (Spendings != null)
                    return _totalRecords = Spendings.Count;
                else
                    return 0;
            }
        }

        private decimal? _sum;
        public decimal? Sum
        {
            get
            {
                if (Spendings != null)
                    return _sum = Spendings.Sum(s => s.Spending.Amount);
                else
                    return 0;
            }
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

        private SpendingDisplayDataModel _selectedSpending;
        public SpendingDisplayDataModel SelectedSpending
        {
            get { return _selectedSpending; }
            set { SetProperty(ref _selectedSpending, value); }
        }

        private SpendingAddDataModel _newSpending;
        public SpendingAddDataModel NewSpending
        {
            get { return _newSpending; }
            set { SetProperty(ref _newSpending, value); }
        }

        private SpendingUpdateDataModel _spendingUpdate;
        public SpendingUpdateDataModel SpendingUpdate
        {
            get { return _spendingUpdate; }
            set { SetProperty(ref _spendingUpdate, value); }
        }

        private List<string> _statementSuggestions;
        public List<string> StatementSuggestions
        {
            get { return _statementSuggestions; }
            set { SetProperty(ref _statementSuggestions, value); }
        }

        private ObservableCollection<SpendingDisplayDataModel> _spendings;
        public ObservableCollection<SpendingDisplayDataModel> Spendings
        {
            get { return _spendings; }
            set
            {
                SetProperty(ref _spendings, value);
                OnPropertyChanged("TotalRecords");
                OnPropertyChanged("Sum");
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
                    _statementSuggestions = unitOfWork.Spendings.GetStatementSuggetions();
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
                    unitOfWork.Spendings.Remove(unitOfWork.Spendings.SingleOrDefault(s => s.RegistrationDate == _selectedSpending.Spending.RegistrationDate));
                    unitOfWork.Safes.Remove(unitOfWork.Safes.SingleOrDefault(s => s.RegistrationDate == _selectedSpending.Spending.RegistrationDate));
                    unitOfWork.Complete();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Add Spending

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
                NewSpending = new SpendingAddDataModel();
                shiftSpendingAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(shiftSpendingAddDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _add;
        public RelayCommand Add
        {
            get
            {
                return _add ?? (_add = new RelayCommand(
                    ExecuteAdd,
                    CanExecuteAdd));
            }
        }
        private void ExecuteAdd()
        {
            try
            {
                if (NewSpending.Statement == null || NewSpending.Amount == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    DateTime dt = DateTime.Now;
                    unitOfWork.Spendings.Add(new Spending
                    {
                        RegistrationDate = dt,
                        UserID = UserData.ID,
                        Statement = _newSpending.Statement,
                        Amount = _newSpending.Amount
                    });
                    unitOfWork.Safes.Add(new Safe
                    {
                        Amount = _newSpending.Amount,
                        CanDelete = false,
                        Statement = _newSpending.Statement,
                        RegistrationDate = dt,
                        UserID = UserData.ID,
                        Type = false
                    });
                    unitOfWork.Complete();
                    _statementSuggestions.Add(_newSpending.Statement);
                    NewSpending = new SpendingAddDataModel();
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteAdd()
        {
            return !NewSpending.HasErrors;
        }

        // Update Spending

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
                SpendingUpdate = new SpendingUpdateDataModel();
                SpendingUpdate.Statement = _selectedSpending.Spending.Statement;
                SpendingUpdate.Amount = _selectedSpending.Spending.Amount;
                shiftSpendingUpdateDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(shiftSpendingUpdateDialog);
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
                if (SpendingUpdate.Statement == null || SpendingUpdate.Amount == null)
                    return;
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _selectedSpending.Spending.Statement = _spendingUpdate.Statement;
                    _selectedSpending.Spending.Amount = _spendingUpdate.Amount;
                    unitOfWork.Spendings.Edit(_selectedSpending.Spending);
                    var safe = unitOfWork.Safes.SingleOrDefault(s => s.RegistrationDate == _selectedSpending.Spending.RegistrationDate);
                    safe.Amount = _spendingUpdate.Amount;
                    safe.Statement = _spendingUpdate.Statement;
                    unitOfWork.Safes.Edit(safe);
                    unitOfWork.Complete();
                    await currentWindow.HideMetroDialogAsync(shiftSpendingUpdateDialog);
                    Load();
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
                return !SpendingUpdate.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(shiftSpendingAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(shiftSpendingUpdateDialog);
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
