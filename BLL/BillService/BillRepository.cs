using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
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

        public List<BillDayDataModel> Search(DateTime date)
        {
            return GeneralDBContext.Bills.Where(w => w.IsDeleted == false && w.EndDate != null && w.Date == date && w.Type== GeneralText.Devices).OrderBy(o => o.ID).Select(s => new BillDayDataModel
            {
                Bill = s,
                User=s.User,
                Client=s.Client
            }).ToList();
        }
    }
}
