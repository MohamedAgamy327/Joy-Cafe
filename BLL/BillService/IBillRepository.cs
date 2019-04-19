using BLL.RepositoryService;
using DAL.Entities;
using DTO.BillDataModel;
using System;
using System.Collections.Generic;

namespace BLL.BillService
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Bill GetLastBill(int deviceId);
        List<BillDisplayDataModel> Search(DateTime date);
    }
}
