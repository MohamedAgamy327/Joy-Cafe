using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using System.Linq;

namespace BLL.BillDeviceService
{
    public class BillDeviceRepository : GenericRepository<BillDevice>, IBillDeviceRepository
    {
        public BillDeviceRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public BillDevice GetLast(int billID)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().OrderByDescending(o => o.EndDate).FirstOrDefault(f => f.BillID == billID);
        }
    }
}
