using DAL.BindableBaseService;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using Utilities.Paging;
using System.Collections.ObjectModel;
using DTO.BillDataModel;
using BLL.UnitOfWorkService;
using DAL;
using DAL.ConstString;
using System.Windows;
using System.Linq;

namespace Cafe.ViewModels.BillViewModels
{
    public class BillDisplayViewModel : ValidatableBindableBase
    {
        private void Load()
        {
            using (var unitOfWork = new UnitOfWork(new GeneralDBContext()))
            {
                Paging.TotalRecords = unitOfWork.Bills.GetRecordsNumber(BillCaseText.All, _key, _dateFrom, _dateTo);
                Paging.GetFirst();
                Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                AvailableCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == false && w.Deleted == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                DeletedCount = unitOfWork.Bills.GetRecordsNumber(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);
                CanceledCount = unitOfWork.Bills.GetRecordsNumber(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.Client.Name + w.User.Name + w.ID.ToString()).Contains(_key) && w.Date >= _dateFrom && w.Date <= _dateTo);

            }
        }

        public BillDisplayViewModel()
        {
            _key = "";
            _dateTo = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _dateFrom = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            _paging = new PagingWPF();
            _billCases = new ObservableCollection<BillCaseDataModel>()
            { new BillCaseDataModel { Value= "الكل",Key= BillCaseText.All },
           new BillCaseDataModel { Value= "المتاح",Key= BillCaseText.Available },
           new BillCaseDataModel { Value=  "الملغى",Key= BillCaseText.Canceled },
           new BillCaseDataModel{  Value="المحذوف",Key= BillCaseText.Deleted } };
            _selectedBillCase = _billCases.SingleOrDefault(w => w.Key == BillCaseText.All);
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        private int? _availableCount;
        public int? AvailableCount
        {
            get { return _availableCount; }
            set { SetProperty(ref _availableCount, value); }
        }

        private int? _deletedCount;
        public int? DeletedCount
        {
            get { return _deletedCount; }
            set { SetProperty(ref _deletedCount, value); }
        }

        private int? _canceledCount;
        public int? CanceledCount
        {
            get { return _canceledCount; }
            set { SetProperty(ref _canceledCount, value); }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private PagingWPF _paging;
        public PagingWPF Paging
        {
            get { return _paging; }
            set { SetProperty(ref _paging, value); }
        }

        private BillCaseDataModel _selectedBillCase;
        public BillCaseDataModel SelectedBillCase
        {
            get { return _selectedBillCase; }
            set { SetProperty(ref _selectedBillCase, value); }
        }

        private ObservableCollection<BillCaseDataModel> _billCases;
        public ObservableCollection<BillCaseDataModel> BillCases
        {
            get { return _billCases; }
            set { SetProperty(ref _billCases, value); }
        }

        private ObservableCollection<BillDisplayDataModel> _bills;
        public ObservableCollection<BillDisplayDataModel> Bills
        {
            get { return _bills; }
            set { SetProperty(ref _bills, value); }
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
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
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
                    Bills = new ObservableCollection<BillDisplayDataModel>(unitOfWork.Bills.Search(_selectedBillCase.Key, _key, _dateFrom, _dateTo, Paging.CurrentPage, PagingWPF.PageSize));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
