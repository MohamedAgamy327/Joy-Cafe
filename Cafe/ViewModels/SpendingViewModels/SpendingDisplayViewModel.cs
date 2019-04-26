using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Cafe.Views.SpendingViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DAL.BindableBaseService;
using Utilities.Paging;
using DAL.Entities;
using DTO.SpendingDataModel;
using BLL.UnitOfWorkService;
using DAL;
using DTO.UserDataModel;

namespace Cafe.ViewModels.SpendingViewModels
{
    public class SpendingDisplayViewModel : ValidatableBindableBase
    {
        private MetroWindow currentWindow;
        private readonly SpendingAddDialog spendingAddDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Spendings.GetRecordsNumber(_key);
                Paging.GetFirst();
                Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public SpendingDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            spendingAddDialog = new SpendingAddDialog();
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

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Spendings = new ObservableCollection<SpendingDisplayDataModel>(unitOfWork.Spendings.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                NewSpending = new SpendingAddDataModel();
                spendingAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(spendingAddDialog);
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
        private bool CanExecuteSave()
        {
            return !NewSpending.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(spendingAddDialog);
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
