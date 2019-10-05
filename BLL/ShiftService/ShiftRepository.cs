using System;
using System.Collections.Generic;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using System.Data.Entity;
using DTO.ShiftDataModel;

namespace BLL.ShiftService
{
    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(w => (w.User.Name).Contains(key) && w.StartDate >= dtFrom && w.StartDate <= dtTo).Count();
        }

        public decimal? GetTotalDevices(DateTime dtFrom, DateTime dtTo)
        {
          return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.TotalDevices != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.TotalDevices) ?? 0;
        }

        public decimal? GetTotalDiscount(DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.TotalDiscount != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.TotalDiscount) ?? 0;
        }

        public decimal? GetTotalIncome(DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.Income != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.Income) ?? 0;
        }

        public decimal? GetTotalItems(DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.TotalItems != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.TotalItems) ?? 0;
        }

        public decimal? GetTotalMinimum(DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.TotalMinimum != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.TotalMinimum) ?? 0;
        }

        public decimal? GetTotalSpending(DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(f => f.Spending != null && f.EndDate >= dtFrom && f.EndDate <= dtTo).Sum(s => s.Spending) ?? 0;
        }

        public Shift GetActive()
        {
            return GeneralDBContext.Shifts.AsNoTracking().SingleOrDefault(s => s.EndDate == null);
        }

        public IEnumerable<Shift> Search(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(w => (w.User.Name).Contains(key) && w.StartDate >= dtFrom && w.StartDate <= dtTo).OrderByDescending(o => o.StartDate).ToList();
        }

        public IEnumerable<ShiftDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(w => (w.User.Name).Contains(key) && w.StartDate >= dtFrom && w.StartDate <= dtTo).OrderByDescending(o => o.StartDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new ShiftDisplayDataModel
            {
                Shift = s,
                User = s.User
            }).ToList();
        }
    }
}
