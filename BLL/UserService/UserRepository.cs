using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.UserDataModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.ConstString;

namespace BLL.UserService
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Users.AsNoTracking().Where(s => s.Name.Contains(key)).Count();
        }

        public int GetWorkedUsers(string role)
        {
            return GeneralDBContext.Users.AsNoTracking().Where(s => s.IsWorked == true && s.Role.Name == role).Count();
        }

        public User GetByName(string name)
        {
            return GeneralDBContext.Users.AsNoTracking().SingleOrDefault(s => s.Name == name);
        }

        public User GetByIdName(int id, string name)
        {
            return GeneralDBContext.Users.AsNoTracking().SingleOrDefault(s => s.ID != id && s.Name == name);
        }

        public User Login(string name, string password)
        {
            return GeneralDBContext.Users.AsNoTracking().SingleOrDefault(s => s.Name == name && s.Password == password && s.IsWorked == true);
        }

        public IEnumerable<UserDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Users.AsNoTracking().Where(w => (w.Name).Contains(key)).OrderByDescending(o => o.IsWorked).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new UserDisplayDataModel
            {
                User = s,
                Role = s.Role,
                Status = s.IsWorked == true ? GeneralText.Work : GeneralText.NotWork,
                CanDelete = s.Bills.Count > 0 || s.Spendings.Count > 0 || s.Safes.Count > 0 || s.Shifts.Count > 0 || s.ID == 1 || s.ID == 2 || s.ID == 3 ? false : true
            }).ToList();
        }

    }
}
