using Cafe.Helpers;
using Cafe.Views.BillViews;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAL.BindableBaseService;

namespace Cafe.ViewModels.BillViewModels
{
    public class SpendingShiftViewModel : ValidatableBindableBase
    {
        //MetroWindow _currentWindow;
        //SafeServices _safeServ;
        //SpendingServices _spendingServ;
        //private readonly SpendingAddDialog _spendingAddDialog;
        //private readonly SpendingUpdateDialog _spendingUpdateDialog;

        //private void Load()
        //{
        //    Spendings = new ObservableCollection<Spending>(_spendingServ.SearchShiftSpendings(_key, MainViewModel.UserID));
        //}

        //public SpendingShiftViewModel()
        //{
        //    _safeServ = new SafeServices();
        //    _spendingServ = new SpendingServices();
        //    _spendingAddDialog = new SpendingAddDialog();
        //    _spendingUpdateDialog = new SpendingUpdateDialog();
        //    _currentWindow = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
        //    _key = "";
        //    _isFocused = true;
        //    _statementSuggestions = _spendingServ.GetStatementSuggetions();
        //    Load();
        //}

        //private bool _isFocused;
        //public bool IsFocused
        //{
        //    get { return _isFocused; }
        //    set { SetProperty(ref _isFocused, value); }
        //}

        //private int _totalRecords;
        //public int TotalRecords
        //{
        //    get { return _totalRecords = Spendings.Count; }
        //}

        //private decimal? _sum;
        //public decimal? Sum
        //{
        //    get { return _sum = Spendings.Sum(s => s.Amount); }
        //}

        //private string _key;
        //public string Key
        //{
        //    get { return _key; }
        //    set
        //    {
        //        SetProperty(ref _key, value);
        //    }
        //}

        //private Spending _selectedSpending;
        //public Spending SelectedSpending
        //{
        //    get { return _selectedSpending; }
        //    set { SetProperty(ref _selectedSpending, value); }
        //}

        //private Spending _newSpending;
        //public Spending NewSpending
        //{
        //    get { return _newSpending; }
        //    set { SetProperty(ref _newSpending, value); }
        //}

        //private List<string> _statementSuggestions;
        //public List<string> StatementSuggestions
        //{
        //    get { return _statementSuggestions; }
        //    set { SetProperty(ref _statementSuggestions, value); }
        //}

        //private ObservableCollection<Spending> _spendings;
        //public ObservableCollection<Spending> Spendings
        //{
        //    get { return _spendings; }
        //    set
        //    {
        //        SetProperty(ref _spendings, value);
        //        OnPropertyChanged("TotalRecords");
        //        OnPropertyChanged("Sum");
        //    }
        //}

        //// Display

        //private RelayCommand _search;
        //public RelayCommand Search
        //{
        //    get
        //    {
        //        return _search
        //            ?? (_search = new RelayCommand(SearchMethod));
        //    }
        //}
        //private void SearchMethod()
        //{
        //    try
        //    {
        //        Load();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _delete;
        //public RelayCommand Delete
        //{
        //    get
        //    {
        //        return _delete
        //            ?? (_delete = new RelayCommand(DeleteMethod));
        //    }
        //}
        //private void DeleteMethod()
        //{
        //    try
        //    {
        //        _spendingServ.DeleteSpending(_selectedSpending);
        //        _safeServ.DeleteSafe(_selectedSpending.RegistrationDate);
        //        Load();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //// Add Spending

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
        //        StatementSuggestions = _spendingServ.GetStatementSuggetions();
        //        NewSpending = new Spending();
        //        _spendingAddDialog.DataContext = this;
        //        await _currentWindow.ShowMetroDialogAsync(_spendingAddDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _add;
        //public RelayCommand Add
        //{
        //    get
        //    {
        //        return _add ?? (_add = new RelayCommand(
        //            ExecuteAdd,
        //            CanExecuteAdd));
        //    }
        //}
        //private void ExecuteAdd()
        //{
        //    try
        //    {
        //        if (NewSpending.Statement == null || NewSpending.Amount == null)
        //            return;
        //        DateTime dt = DateTime.Now;
        //        _newSpending.RegistrationDate = dt;
        //        _newSpending.UserID = MainViewModel.UserID;
        //        _spendingServ.AddSpending(_newSpending);
        //        Safe _safe = new Safe
        //        {
        //            Amount = -_newSpending.Amount,
        //            CanDelete = false,
        //            Statement = _newSpending.Statement,
        //            RegistrationDate = dt,
        //            UserID = MainViewModel.UserID
        //        };
        //        _safeServ.AddSafe(_safe);
        //        _statementSuggestions.Add(_newSpending.Statement);
        //        NewSpending = new Spending();
        //        //await _currentWindow.ShowMessageAsync("نجاح الإضافة", "تم الإضافة بنجاح", MessageDialogStyle.Affirmative, new MetroDialogSettings()
        //        //{
        //        //    AffirmativeButtonText = "موافق",
        //        //    DialogMessageFontSize = 25,
        //        //    DialogTitleFontSize = 30
        //        //});
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteAdd()
        //{
        //    return !NewSpending.HasErrors;
        //}

        //// Update Spending

        //private RelayCommand _showUpdate;
        //public RelayCommand ShowUpdate
        //{
        //    get
        //    {
        //        return _showUpdate
        //            ?? (_showUpdate = new RelayCommand(ShowUpdateMethod));
        //    }
        //}
        //private async void ShowUpdateMethod()
        //{
        //    try
        //    {
        //        _spendingUpdateDialog.DataContext = this;
        //        await _currentWindow.ShowMetroDialogAsync(_spendingUpdateDialog);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private RelayCommand _update;
        //public RelayCommand Update
        //{
        //    get
        //    {
        //        return _update ?? (_update = new RelayCommand(
        //            ExecuteUpdateAsync,
        //            CanExecuteUpdate));
        //    }
        //}
        //private async void ExecuteUpdateAsync()
        //{
        //    try
        //    {
        //        if (SelectedSpending.Statement == null || SelectedSpending.Amount == null)
        //            return;
        //        _spendingServ.UpdateSpending(_selectedSpending);
        //        var safe = _safeServ.GetSafe(_selectedSpending.RegistrationDate);
        //        safe.Amount = _selectedSpending.Amount;
        //        safe.Statement = _selectedSpending.Statement;
        //        _safeServ.UpdateSafe(safe);
        //        await _currentWindow.HideMetroDialogAsync(_spendingUpdateDialog);
        //        Load();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        //private bool CanExecuteUpdate()
        //{
        //    try
        //    {
        //        return !SelectedSpending.HasErrors;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
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
        //                await _currentWindow.HideMetroDialogAsync(_spendingAddDialog);
        //                Load();
        //                break;
        //            case "Update":
        //                await _currentWindow.HideMetroDialogAsync(_spendingUpdateDialog);
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
