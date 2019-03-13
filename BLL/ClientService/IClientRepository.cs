using BLL.RepositoryService;
using DAL.Entities;
using DTO.ClientDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ClientService
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        int GetRecordsNumber(bool isNew, string key);
        int GetRecordsNumber(string key);
        List<string> GetTelephoneSuggetions();
        List<ClientDisplayDataModel> Search(string key, int pageNumber, int pageSize);
        List<ClientPointDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);
    }
}
