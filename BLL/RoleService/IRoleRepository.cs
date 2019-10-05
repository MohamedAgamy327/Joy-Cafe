using BLL.RepositoryService;
using DAL.Entities;
using System.Collections.Generic;

namespace BLL.RoleService
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        IEnumerable<Role> GetAll();
    }
}
