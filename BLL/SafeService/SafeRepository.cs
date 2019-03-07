using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.SafeDataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.SafeService
{
    public class SafeRepository : GenericRepository<Safe>, ISafeRepository
    {
        public SafeRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public decimal? GetCurrentAccount()
        {
            return (GeneralDBContext.Safes.AsNoTracking().Any(w => w.Type == true) ? GeneralDBContext.Safes.AsNoTracking().Where(w => w.Type == true).Sum(s => s.Amount) : 0)
                - (GeneralDBContext.Safes.AsNoTracking().Any(w => w.Type == false) ? GeneralDBContext.Safes.AsNoTracking().Where(w => w.Type == false).Sum(s => s.Amount) : 0);
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Safes.AsNoTracking().Where(s => (s.Statement + s.User.Name).Contains(key)).Count();
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Safes.AsNoTracking().Where(s => (s.Statement + s.User.Name).Contains(key) && s.RegistrationDate >= dtFrom && s.RegistrationDate <= dtTo).Count();
        }

        public List<string> GetStatementSuggetions()
        {
            return GeneralDBContext.Safes.AsNoTracking().Where(w => w.CanDelete == true).OrderBy(o => o.Statement).Select(s => s.Statement).Distinct().ToList();
        }

        public decimal? GetTotalIncome(string key, DateTime dtFrom, DateTime dtTo)
        {
            return (GeneralDBContext.Safes.AsNoTracking().Any(w => w.Type == true && (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo) ? GeneralDBContext.Safes.AsNoTracking().Where(w => w.Type == true && (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo).Sum(s => s.Amount) : 0);
        }

        public decimal? GetTotalOutgoings(string key, DateTime dtFrom, DateTime dtTo)
        {
            return (GeneralDBContext.Safes.AsNoTracking().Any(w => w.Type == false && (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo) ? GeneralDBContext.Safes.AsNoTracking().Where(w => w.Type == false && (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo).Sum(s => s.Amount) : 0);
        }

        public List<SafeDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Safes.AsNoTracking().Where(w => (w.Statement+ w.User.Name).Contains(key)).OrderByDescending(o => o.RegistrationDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new SafeDisplayDataModel
            {
                Safe = s,
                User = s.User
            }).ToList();
        }

        public List<SafeDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Safes.AsNoTracking().Where(w => (w.Statement + w.User.Name).Contains(key) && w.RegistrationDate >= dtFrom && w.RegistrationDate <= dtTo).OrderByDescending(o => o.RegistrationDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new SafeDisplayDataModel
            {
                Safe = s,
                User = s.User
            }).ToList();
        }
    }
}
