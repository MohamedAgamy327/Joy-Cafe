using BLL.RepositoryService;
using DAL;
using DAL.Entities;

namespace BLL.RoleService
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }
    }
}
