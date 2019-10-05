using System.Collections.Generic;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL.RoleService
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public IEnumerable<Role> GetAll()
        {
            return GeneralDBContext.Roles.AsNoTracking().ToList();
        }
    }
}
