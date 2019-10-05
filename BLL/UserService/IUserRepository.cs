using BLL.RepositoryService;
using DAL.Entities;
using DTO.UserDataModel;
using System.Collections.Generic;

namespace BLL.UserService
{
    public interface IUserRepository : IGenericRepository<User>
    {
        int GetRecordsNumber(string key);

        int GetWorkedUsers(string role);

        User GetByName(string name);

        User GetByIdName(int id, string name);

        User Login(string name, string password);

        IEnumerable<UserDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
