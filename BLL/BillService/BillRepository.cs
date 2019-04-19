using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Bill GetLastBill(int deviceId)
        {
            
            return GeneralDBContext.BillsDevices.Where(w => w.Bill.EndDate != null && w.DeviceID == deviceId).OrderByDescending(o => o.EndDate).FirstOrDefault()!=null?
                GeneralDBContext.BillsDevices.Where(w => w.Bill.EndDate != null && w.DeviceID == deviceId).OrderByDescending(o => o.EndDate).FirstOrDefault().Bill:null ;

        }

        public List<BillDisplayDataModel> Search(DateTime date)
        {
            return GeneralDBContext.Bills.Where(w => w.IsDeleted == false && w.EndDate != null && w.Date == date).OrderBy(o => o.ID).Select(s => new BillDisplayDataModel
            {
                Bill = s
            }).ToList();
        }
    }
}
