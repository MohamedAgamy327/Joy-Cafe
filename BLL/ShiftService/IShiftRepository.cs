using BLL.RepositoryService;
using DAL.Entities;
using DTO.ShiftDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ShiftService
{
    public interface IShiftRepository : IGenericRepository<Shift>
    {
        int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo);
        List<ShiftDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize);
    }
}
