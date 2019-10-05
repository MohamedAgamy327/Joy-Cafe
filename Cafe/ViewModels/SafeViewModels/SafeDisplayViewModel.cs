using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Cafe.Views.SafeViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL;
using Utilities.Paging;
using DTO.SafeDataModel;
using DAL.Entities;
using DTO.UserDataModel;

namespace Cafe.ViewModels.SafeViewModels
{
    public class SafeDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly SafeAddDialog safeAddDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Safes.GetRecordsNumber(_key);
                Paging.GetFirst();
                Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public SafeDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            safeAddDialog = new SafeAddDialog();
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

        private SafeDisplayDataModel _selectedSafe;
        public SafeDisplayDataModel SelectedSafe
        {
            get { return _selectedSafe; }
            set { SetProperty(ref _selectedSafe, value); }
        }

        private SafeAddDataModel _newSafe;
        public SafeAddDataModel NewSafe
        {
            get { return _newSafe; }
            set { SetProperty(ref _newSafe, value); }
        }

        private List<string> _statementSuggestions;
        public List<string> StatementSuggestions
        {
            get { return _statementSuggestions; }
            set { SetProperty(ref _statementSuggestions, value); }
        }

        private ObservableCollection<SafeDisplayDataModel> _safes;
        public ObservableCollection<SafeDisplayDataModel> Safes
        {
            get { return _safes; }
            set { SetProperty(ref _safes, value); }
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
                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    _statementSuggestions = unitOfWork.Safes.GetStatementSuggetions().ToList();
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
                    Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Safes = new ObservableCollection<SafeDisplayDataModel>(unitOfWork.Safes.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Safes.Remove(_selectedSafe.Safe);
                    unitOfWork.Complete();
                }
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Add

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
                NewSafe = new SafeAddDataModel();
                safeAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(safeAddDialog);
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
                if (NewSafe.Statement == null || NewSafe.Amount == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.Safes.Add(new Safe
                    {
                        Amount = _newSafe.Amount,
                        Statement = _newSafe.Statement,
                        Type = true,
                        RegistrationDate=DateTime.Now,
                        UserID = UserData.ID,
                        CanDelete = true
                    });
                    unitOfWork.Complete();
                    _statementSuggestions.Add(_newSafe.Statement);
                    NewSafe = new SafeAddDataModel();
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
            return !NewSafe.HasErrors;
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
                        await currentWindow.HideMetroDialogAsync(safeAddDialog);
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
