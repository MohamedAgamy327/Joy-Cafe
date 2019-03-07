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
        static List<User> users { get; set; }

        public UserRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(bool isNew, string key)
        {
            if (isNew)
                users = GetAll().ToList();
            return users.Where(s => s.Name.Contains(key)).Count();

        }

        public List<UserDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return users.Where(w => (w.Name).Contains(key)).OrderByDescending(o => o.IsWorked).ThenBy(t => t.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new UserDisplayDataModel
            {
                User = s,
                Role = s.Role,
                Status = s.IsWorked == true ? GeneralText.Work : GeneralText.NotWork,
                CanDelete = s.Bills.Count > 0 || s.Spendings.Count > 0 || s.Safes.Count > 0 ? false : true
            }).ToList();
        }
    }
}
