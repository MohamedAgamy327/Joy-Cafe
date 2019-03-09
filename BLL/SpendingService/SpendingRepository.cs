using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.SpendingDataModel;

namespace BLL.SpendingService
{
    public class SpendingRepository : GenericRepository<Spending>, ISpendingRepository
    {
        public SpendingRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(s => (s.Statement + s.User.Name).Contains(key)).Count();
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(s => (s.Statement + s.User.Name).Contains(key) && s.RegistrationDate >= dtFrom && s.RegistrationDate <= dtTo).Count();
        }

        public List<string> GetStatementSuggetions()
        {
            return GeneralDBContext.Spendings.AsNoTracking().OrderBy(o => o.Statement).Select(s => s.Statement).Distinct().ToList();
        }

        public decimal? GetTotalAmount(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(w => (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo).Sum(s => s.Amount);
        }

        public List<SpendingDisplayDataModel> Search(string key, int userID)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(w => w.UserID == userID && w.Statement.Contains(key) && w.RegistrationDate >= w.User.Shifts.FirstOrDefault(f => f.EndDate == null).StartDate && w.RegistrationDate <= DateTime.Now).OrderByDescending(o => o.RegistrationDate).Select(s => new SpendingDisplayDataModel
            {
                Spending = s,
                User = s.User
            }).ToList();
        }

        public List<SpendingDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(w => (w.Statement + w.User.Name).Contains(key)).OrderByDescending(o => o.RegistrationDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new SpendingDisplayDataModel
            {
                Spending = s,
                User = s.User
            }).ToList();
        }

        public List<SpendingDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Spendings.AsNoTracking().Where(w => (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new SpendingDisplayDataModel
            {
                Spending = s,
                User = s.User
            }).ToList();
        }
    }
}
