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

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(w => (w.User.Name).Contains(key) && w.StartDate >= dtFrom && w.StartDate <= dtTo).Count();
        }

        public List<ShiftDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Shifts.AsNoTracking().Where(w => (w.User.Name).Contains(key) && w.StartDate >= dtFrom && w.StartDate <= dtTo).OrderByDescending(o => o.StartDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new ShiftDisplayDataModel
            {
                Shift=s,
                User=s.User
            }).ToList();
        }
    }
}
