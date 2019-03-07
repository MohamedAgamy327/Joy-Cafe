using Cafe.Views.UserViews;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DAL.BindableBaseService;
using Utilities.Paging;
using BLL.UnitOfWorkService;
using DAL;
using DTO.UserDataModel;
using DAL.Entities;

namespace Cafe.ViewModels.UserViewModels
{
    public class UserDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;
        private readonly UserAddDialog _userAddDialog;
        private readonly UserUpdateDialog _userUpdateDialog;

        private void Load(bool isNew)
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Users.GetRecordsNumber(isNew, _key);
                Paging.GetFirst();
                Users = new ObservableCollection<UserDisplayDataModel>(unitOfWork.Users.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public UserDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            _userAddDialog = new UserAddDialog();
            _userUpdateDialog = new UserUpdateDialog();
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

        private UserDisplayDataModel _selectedUser;
        public UserDisplayDataModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private UserAddDataModel _newUser;
        public UserAddDataModel NewUser
        {
            get { return _newUser; }
            set { SetProperty(ref _newUser, value); }
        }

        private UserUpdateDataModel _userUpdate;
        public UserUpdateDataModel UserUpdate
        {
            get { return _userUpdate; }
            set { SetProperty(ref _userUpdate, value); }
        }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get { return _selectedRole; }
            set { SetProperty(ref _selectedRole, value); }
        }

        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> Roles
        {
            get { return _roles; }
            set { SetProperty(ref _roles, value); }
        }

        private ObservableCollection<UserDisplayDataModel> _users;
        public ObservableCollection<UserDisplayDataModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
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
                    _roles = new ObservableCollection<Role>(unitOfWork.Roles.GetAll());
                }
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
                    Users = new ObservableCollection<UserDisplayDataModel>(unitOfWork.Users.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Users = new ObservableCollection<UserDisplayDataModel>(unitOfWork.Users.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
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
                    unitOfWork.Users.Remove(_selectedUser.User);
                    unitOfWork.Complete();
                }
                Load(true);
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
                SelectedRole = new Role();
                NewUser = new UserAddDataModel();
                _userAddDialog.DataContext = this;
                await _currentWindow.ShowMetroDialogAsync(_userAddDialog);
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
                if (NewUser.Name == null || NewUser.Password == null || SelectedRole == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.SingleOrDefault(s => s.Name == _newUser.Name);

                    if (user != null)
                    {
                        await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذا المستخدم موجودة مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        unitOfWork.Users.Add(new User
                        {
                            IsWorked = true,
                            Name = _newUser.Name,
                            Password = _newUser.Password,
                            RoleID = _newUser.RoleID
                        });
                        unitOfWork.Complete();
                        NewUser = new UserAddDataModel();
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
            return !NewUser.HasErrors;
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
                _userUpdateDialog.DataContext = this;
                UserUpdate = new UserUpdateDataModel();
                UserUpdate.ID = SelectedUser.User.ID;
                UserUpdate.IsWorked = SelectedUser.User.IsWorked;
                UserUpdate.Name = SelectedUser.User.Name;
                UserUpdate.Password = SelectedUser.User.Password;
                UserUpdate.RoleID = SelectedUser.User.RoleID;
                UserUpdate.Role = SelectedUser.Role;
                await _currentWindow.ShowMetroDialogAsync(_userUpdateDialog);
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
                if (UserUpdate.Password == null || UserUpdate.Role == null || UserUpdate.Name == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.SingleOrDefault(s => s.Name == UserUpdate.Name && s.ID != UserUpdate.ID);
                    if (user != null)
                    {
                        await _currentWindow.ShowMessageAsync("فشل الإضافة", "هذا المستخدم موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else
                    {
                        SelectedUser.User.RoleID = UserUpdate.RoleID;
                        SelectedUser.User.Role = UserUpdate.Role;
                        SelectedUser.User.Name = UserUpdate.Name;
                        SelectedUser.User.Password = UserUpdate.Password;
                        SelectedUser.User.IsWorked = UserUpdate.IsWorked;
                        unitOfWork.Users.Edit(SelectedUser.User);
                        unitOfWork.Complete();
                        await _currentWindow.HideMetroDialogAsync(_userUpdateDialog);
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
                return !UserUpdate.HasErrors;
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
                        await _currentWindow.HideMetroDialogAsync(_userAddDialog);
                        break;
                    case "Update":
                        await _currentWindow.HideMetroDialogAsync(_userUpdateDialog);
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
