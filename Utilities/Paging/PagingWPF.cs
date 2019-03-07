using DAL.BindableBaseService;
using System;

namespace Utilities.Paging
{
    public class PagingWPF : ValidatableBindableBase
    {
        public static int PageSize = 17;

        private bool _first;
        public bool First
        {
            get { return _first; }
            set { SetProperty(ref _first, value); }
        }

        private bool _last;
        public bool Last
        {
            get { return _last; }
            set { SetProperty(ref _last, value); }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        private int _lastPage;
        public int LastPage
        {
            get { return _lastPage; }
            set { SetProperty(ref _lastPage, value); }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }

        public void GetFirst()
        {
            CurrentPage = 1;
            First = false;
            LastPage = (int)Math.Ceiling(Convert.ToDecimal((double)TotalRecords / PageSize));
            if (_lastPage == 0)
                LastPage = 1;
            if (_lastPage == 1)
                Last = false;
            else
                Last = true;
        }

        public void Next()
        {
            CurrentPage++;
            First = true;
            if (_currentPage == _lastPage)
                Last = false;
        }

        public void Previous()
        {
            CurrentPage--;
            Last = true;
            if (_currentPage == 1)
                First = false;
        }

    }
}
