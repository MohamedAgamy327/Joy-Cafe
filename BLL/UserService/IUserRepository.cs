using BLL.RepositoryService;
using DAL.Entities;
using DTO.UserDataModel;
using System.Collections.Generic;

namespace BLL.UserService
{
    public interface IUserRepository : IGenericRepository<User>
    {
        int GetRecordsNumber(string key);
        List<UserDisplayDataModel> Search(string key, int pageNumber, int pageSize);
    }
}
