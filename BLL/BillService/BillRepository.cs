using BLL.RepositoryService;
using DAL;
using DAL.Entities;

namespace BLL.BillService
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }
    }
}
