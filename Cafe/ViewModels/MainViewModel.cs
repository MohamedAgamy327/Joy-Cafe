using GalaSoft.MvvmLight.CommandWpf;
using Cafe.Views.MainViews;
using Cafe.Views.SafeViews;
using Cafe.Views.SpendingViews;
using MahApps.Metro.Controls;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System;
using System.Data.Entity;
using System.Windows.Input;
using System.Data.SqlClient;
using Cafe.Views.UserViews;
using Cafe.Views.ClientViews;
using Cafe.Views.DeviceViews;
using Cafe.Views.BillViews;
using Cafe.Views.ShiftViews;
using Cafe.Views.MembershipViews;
using DAL.BindableBaseService;
using BLL.UnitOfWorkService;
using DAL;
using DTO.MainDataModel;
using DAL.Entities;
using DAL.ConstString;
using Cafe.Views.ItemViews;
using DTO.UserDataModel;
using System.IO;
using System.Data;
using DTO.ShiftDataModel;

namespace Cafe.ViewModels
{
    public class MainViewModel : ValidatableBindableBase
    {
        MetroWindow _currentWindow;

        private readonly BackupDialog _backupDialog;
        private readonly RestoreBackupDialog _restoreBackupDialog;
        private readonly Views.MainViews.LoginDialog _loginDialog;
        private readonly StartShiftDialog _startShiftDialog;

        public MainViewModel()
        {
            _isFocused = true;
            _loginModel = new LoginDataModel();
            _currentWindow = System.Windows.Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
            _backupDialog = new BackupDialog();
            _restoreBackupDialog = new RestoreBackupDialog();
            _loginDialog = new Views.MainViews.LoginDialog();
            _startShiftDialog = new StartShiftDialog();
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get { return _isFocused; }
            set { SetProperty(ref _isFocused, value); }
        }

        private LoginDataModel _loginModel;
        public LoginDataModel LoginModel
        {
            get { return _loginModel; }
            set { SetProperty(ref _loginModel, value); }
        }

        private StartShiftDataModel _newShiftModel;
        public StartShiftDataModel NewShiftModel
        {
            get { return _newShiftModel; }
            set { SetProperty(ref _newShiftModel, value); }
        }

        private BackupDataModel _backupModel;
        public BackupDataModel BackupModel
        {
            get { return _backupModel; }
            set { SetProperty(ref _backupModel, value); }
        }

        private RestoreBackupDataModel _restoreBackupModel;
        public RestoreBackupDataModel RestoreBackupModel
        {
            get { return _restoreBackupModel; }
            set { SetProperty(ref _restoreBackupModel, value); }
        }

        // Main

        private RelayCommand<string> _navigateToView;
        public RelayCommand<string> NavigateToView
        {
            get
            {
                return _navigateToView
                    ?? (_navigateToView = new RelayCommand<string>(NavigateToViewMethodAsync));
            }
        }
        public async void NavigateToViewMethodAsync(string destination)
        {
            try
            {
                switch (destination)
                {
                    case "Backup":
                        BackupModel = new BackupDataModel();
                        _backupDialog.DataContext = this;
                        await _currentWindow.ShowMetroDialogAsync(_backupDialog);
                        break;

                    case "BackupRestore":
                        RestoreBackupModel = new RestoreBackupDataModel();
                        _restoreBackupDialog.DataContext = this;
                        await _currentWindow.ShowMetroDialogAsync(_restoreBackupDialog);
                        break;

                    case "User":
                        _currentWindow.Hide();
                        new UserWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Shifts":
                        _currentWindow.Hide();
                        new ShiftWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "SignOut":
                        LoginModel = new LoginDataModel();
                        _loginDialog.DataContext = this;
                        await _currentWindow.ShowMetroDialogAsync(_loginDialog);
                        break;

                    case "Device":
                        _currentWindow.Hide();
                        new DeviceWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Item":
                        _currentWindow.Hide();
                        new ItemWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Client":
                        _currentWindow.Hide();
                        new ClientWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Membership":
                        _currentWindow.Hide();
                        new MembershipWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Bill":
                        using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                        {
                            if (unitOfWork.DevicesTypes.FirstOrDefault(f => f.SingleHourPrice == 0) != null)
                            {
                                await _currentWindow.ShowMessageAsync("تنبيه", "يجب وضع اسعار الانواع اولاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                                {
                                    AffirmativeButtonText = "موافق",
                                    DialogMessageFontSize = 25,
                                    DialogTitleFontSize = 30
                                });
                                if (UserData.Role == RoleText.Cashier)
                                    ExecuteShutdown();
                                return;
                            }
                        }
                        _currentWindow.Hide();
                        new BillWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Spending":
                        _currentWindow.Hide();
                        new SpendingWindow().ShowDialog();
                        _currentWindow.Show();
                        break;

                    case "Safe":
                        _currentWindow.Hide();
                        new SafeWindow().ShowDialog();
                        _currentWindow.Show();
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

        // Login

        private RelayCommand _showLogin;
        public RelayCommand ShowLogin
        {
            get
            {
                return _showLogin ?? (_showLogin = new RelayCommand(
                    ExecuteShowLoginAsync));
            }
        }
        private async void ExecuteShowLoginAsync()
        {
            try
            {
                _loginDialog.DataContext = this;
                await _currentWindow.ShowMetroDialogAsync(_loginDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private RelayCommand _signIn;
        public RelayCommand SignIn
        {
            get
            {
                return _signIn ?? (_signIn = new RelayCommand(
                    ExecuteSignInAsync,
                    CanExecuteSignIn));
            }
        }
        private async void ExecuteSignInAsync()
        {
            try
            {
                if (LoginModel.Name == null || LoginModel.Password == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    var user = unitOfWork.Users.SingleOrDefault(s => s.Name == _loginModel.Name && s.Password == _loginModel.Password && s.IsWorked == true);

                    if (user != null)
                    {
                        UserData.ID = user.ID;
                        UserData.Role = user.Role.Name;
                        UserData.Name = user.Name;

                        if (UserData.Role == RoleText.Admin)
                        {
                            await _currentWindow.HideMetroDialogAsync(_loginDialog);
                        }

                        else
                        {
                            if (unitOfWork.Shifts.SingleOrDefault(s => s.UserID == user.ID && s.EndDate == null) != null)
                            {
                                await _currentWindow.HideMetroDialogAsync(_loginDialog);
                                NavigateToViewMethodAsync("Bill");
                            }
                            else if (unitOfWork.Shifts.SingleOrDefault(s => s.UserID != user.ID && s.EndDate == null) != null)
                            {
                                await _currentWindow.HideMetroDialogAsync(_loginDialog);
                                await _currentWindow.ShowMessageAsync("فشل الدخول", "يجب انهاء الشفت اولاً", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                                {
                                    AffirmativeButtonText = "موافق",
                                    DialogMessageFontSize = 25,
                                    DialogTitleFontSize = 30
                                });
                                await _currentWindow.ShowMetroDialogAsync(_loginDialog);
                            }
                            else
                            {
                                await _currentWindow.HideMetroDialogAsync(_loginDialog);
                                NewShiftModel = new StartShiftDataModel();
                                _startShiftDialog.DataContext = this;
                                await _currentWindow.ShowMetroDialogAsync(_startShiftDialog);
                            }
                        }
                    }

                    else
                    {
                        await _currentWindow.HideMetroDialogAsync(_loginDialog);
                        await _currentWindow.ShowMessageAsync("فشل الدخول", "يوجد خطأ فى الاسم أو الرقم السرى يرجى التأكد من البيانات", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        await _currentWindow.ShowMetroDialogAsync(_loginDialog);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteSignIn()
        {
            return !LoginModel.HasErrors;
        }

        // Start Shift

        private RelayCommand _startShift;
        public RelayCommand StartShift
        {
            get
            {
                return _startShift ?? (_startShift = new RelayCommand(
                    ExecuteStartShiftAsync,
                    CanExecuteStartShift));
            }
        }
        private async void ExecuteStartShiftAsync()
        {
            try
            {
                if (NewShiftModel.SafeStart == null)
                    return;

                using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
                {
                    unitOfWork.Shifts.Add(new Shift
                    {
                        SafeStart = _newShiftModel.SafeStart,
                        StartDate = DateTime.Now,
                        UserID = UserData.ID
                    });
                    unitOfWork.Complete();
                }
                await _currentWindow.HideMetroDialogAsync(_startShiftDialog);
                NavigateToViewMethodAsync("Bill");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteStartShift()
        {
            return !NewShiftModel.HasErrors;
        }

        // Backup

        private RelayCommand _browseFloder;
        public RelayCommand BrowseFloder
        {
            get
            {
                return _browseFloder ?? (_browseFloder = new RelayCommand(
                    ExecuteBrowseFloder));
            }
        }
        private void ExecuteBrowseFloder()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                BackupModel.Path = fbd.SelectedPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _backup;
        public RelayCommand Backup
        {
            get
            {
                return _backup ?? (_backup = new RelayCommand(
                    ExecuteBackupAsync,
                    CanExecuteBackup));
            }
        }
        private async void ExecuteBackupAsync()
        {
            try
            {
                if (BackupModel.Path == null)
                    return;
                using (GeneralDBContext db = new GeneralDBContext())
                {
                    try
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        string fileName = BackupModel.Path + "\\JoyDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                                + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                        string dbname = db.Database.Connection.Database;
                        string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                        BackupModel = new BackupDataModel();
                        Mouse.OverrideCursor = null;
                    }
                    catch
                    {
                        Mouse.OverrideCursor = null;
                        await _currentWindow.HideMetroDialogAsync(_backupDialog);
                        await _currentWindow.ShowMessageAsync("فشل الحفظ", "يجب إختيار مكان آخر لحفظ النسخة الإحتياطية", MessageDialogStyle.Affirmative, new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "موافق",
                            DialogMessageFontSize = 25,
                            DialogTitleFontSize = 30
                        });
                        await _currentWindow.ShowMetroDialogAsync(_backupDialog);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteBackup()
        {
            return !BackupModel.HasErrors;
        }

        // Restore Backup

        private RelayCommand _browseFile;
        public RelayCommand BrowseFile
        {
            get
            {
                return _browseFile ?? (_browseFile = new RelayCommand(
                    ExecuteBrowseFile));
            }
        }
        private void ExecuteBrowseFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "استرجاع نسخة احتياطية",
                    Filter = "Backup files(*.Bak)|*.Bak"
                };
                ofd.ShowDialog();
                RestoreBackupModel.Path = ofd.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private RelayCommand _restore;
        public RelayCommand Restore
        {
            get
            {
                return _restore ?? (_restore = new RelayCommand(
                    ExecuteRestore,
                    CanExecuteRestore));
            }
        }
        private void ExecuteRestore()
        {
            try
            {
                if (RestoreBackupModel.Path == null)
                    return;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                SqlConnection sqlconnection = new SqlConnection(@"Server=.\sqlexpress; Database=master; Integrated Security=true");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlconnection;
                cmd.Parameters.Add("@fileName", SqlDbType.NChar).Value = RestoreBackupModel.Path;
                cmd.CommandText = "ALTER Database JoyDB SET OFFLINE WITH ROLLBACK IMMEDIATE; Restore Database JoyDB From Disk= @fileName WITH REPLACE";
                sqlconnection.Open();
                cmd.ExecuteNonQuery();
                string strQuery = "ALTER Database JoyDB SET ONLINE WITH ROLLBACK IMMEDIATE";
                cmd = new SqlCommand(strQuery, sqlconnection);
                cmd.ExecuteNonQuery();
                sqlconnection.Close();
                RestoreBackupModel = new RestoreBackupDataModel();
                Mouse.OverrideCursor = null;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanExecuteRestore()
        {
            return !RestoreBackupModel.HasErrors;
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
                    case "Backup":
                        await _currentWindow.HideMetroDialogAsync(_backupDialog);
                        break;
                    case "Restore":
                        await _currentWindow.HideMetroDialogAsync(_restoreBackupDialog);
                        break;
                    case "startShift":
                        await _currentWindow.HideMetroDialogAsync(_startShiftDialog);
                        _currentWindow.Close();
                        break;
                    case "back":
                        await _currentWindow.HideMetroDialogAsync(_startShiftDialog);
                        LoginModel = new LoginDataModel();
                        _loginDialog.DataContext = this;
                        await _currentWindow.ShowMetroDialogAsync(_loginDialog);
                        break;
                    case "Login":
                        await _currentWindow.HideMetroDialogAsync(_loginDialog);
                        _currentWindow.Close();
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

        private RelayCommand _shutdown;
        public RelayCommand Shutdown
        {
            get
            {
                return _shutdown ?? (_shutdown = new RelayCommand(
                    ExecuteShutdown));
            }
        }
        public void ExecuteShutdown()
        {
            try
            {
                string _path = "";
                bool _exists;
                try
                {
                    _path = @"D:\JoyDB";
                    _exists = Directory.Exists(_path);
                    if (!_exists)
                        Directory.CreateDirectory(_path);
                }
                catch
                {

                }
                try
                {
                    _path = @"E:\JoyDB";
                    _exists = Directory.Exists(_path);
                    if (!_exists)
                        Directory.CreateDirectory(_path);
                }
                catch
                {
                    _path = @"D:\JoyDB";
                }
                using (GeneralDBContext db = new GeneralDBContext())
                {
                    try
                    {
                        string fileName = _path + "\\JoyDB " + DateTime.Now.ToShortDateString().Replace('/', '-')
                                                + " - " + DateTime.Now.ToLongTimeString().Replace(':', '-');
                        string dbname = db.Database.Connection.Database;
                        string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'" + fileName + ".bak' WITH NOFORMAT, NOINIT,NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                        db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname));
                    }
                    catch
                    {
                    }
                }
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
