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
using DAL.ConstString;

namespace Cafe.ViewModels.UserViewModels
{
    public class UserDisplayViewModel : ValidatableBindableBase
    {
        MetroWindow currentWindow;
        private readonly UserAddDialog userAddDialog;
        private readonly UserUpdateDialog userUpdateDialog;

        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Users.GetRecordsNumber(_key);
                Paging.GetFirst();
                Users = new ObservableCollection<UserDisplayDataModel>(unitOfWork.Users.Search(_key, Paging.CurrentPage, PagingWPF.PageSize));
            }
        }

        public UserDisplayViewModel()
        {
            _key = "";
            _isFocused = true;
            _paging = new PagingWPF();
            userAddDialog = new UserAddDialog();
            userUpdateDialog = new UserUpdateDialog();
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
                Load();
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
                userAddDialog.DataContext = this;
                await currentWindow.ShowMetroDialogAsync(userAddDialog);
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
                if (NewUser.Name == null || NewUser.Password == null || SelectedRole.Name == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.GetByName(_newUser.Name);

                    if (user != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل الإضافة", "هذا المستخدم موجودة مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
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
                userUpdateDialog.DataContext = this;
                UserUpdate = new UserUpdateDataModel
                {
                    ID = SelectedUser.User.ID,
                    IsWorked = SelectedUser.User.IsWorked,
                    Name = SelectedUser.User.Name,
                    Password = SelectedUser.User.Password,
                    RoleID = SelectedUser.User.RoleID,
                    Role = SelectedUser.Role
                };
                await currentWindow.ShowMetroDialogAsync(userUpdateDialog);
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
                if (UserUpdate.Password == null || UserUpdate.Role.Name == null || UserUpdate.Name == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.GetByIdName(_userUpdate.ID, _userUpdate.Name);
                    if (user != null)
                    {
                        await currentWindow.ShowMessageAsync("فشل التعديل", "هذا المستخدم موجود مسبقاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else if (_userUpdate.IsWorked == false && _userUpdate.Role.Name == RoleText.Admin && unitOfWork.Users.GetWorkedUsers(RoleText.Admin) == 1)
                    {
                        await currentWindow.ShowMessageAsync("فشل التعديل", "لا يوجد مستخدم آخر لديه نفس الصلاحيات يجب اضافة مستخدم اخر بنفس الصلاحيات ثم تعديل هذا المستخدم", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                    }
                    else if (_userUpdate.IsWorked == false && _userUpdate.Role.Name == RoleText.Cashier && unitOfWork.Users.GetWorkedUsers(RoleText.Cashier) == 2)
                    {
                        await currentWindow.ShowMessageAsync("فشل التعديل", "يجب ان يكون لديك مستخدمين على الاقل لهم نفس الصلاحيات لكى تستطيع تعديل هذا المستخدم", MessageDialogStyle.Affirmative, new MetroDialogSettings()
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
                        await currentWindow.HideMetroDialogAsync(userUpdateDialog);
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
                        await currentWindow.HideMetroDialogAsync(userAddDialog);
                        break;
                    case "Update":
                        await currentWindow.HideMetroDialogAsync(userUpdateDialog);
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
