using BLL.RepositoryService;
using DAL.Entities;

namespace BLL.BillDeviceService
{
    public interface IBillDeviceRepository : IGenericRepository<BillDevice>
    {
        BillDevice GetLast(int billID);
    }
}
