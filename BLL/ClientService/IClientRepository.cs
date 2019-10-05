using BLL.RepositoryService;
using DAL.Entities;
using DTO.ClientDataModel;
using System;
using System.Collections.Generic;

namespace BLL.ClientService
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        int GetRecordsNumber(string key);

        Client GetById(int id);

        Client GetByCodeTelephone(string code, string telephone);

        Client GetByTelephone(string telephone);

        Client GetByIdCodeTelephone(int id, string code, string telephone);

        IEnumerable<string> GetTelephoneSuggetions();

        IEnumerable<Client> Search();

        IEnumerable<Client> Search(string key);

        IEnumerable<ClientDisplayDataModel> Search(string key, int pageNumber, int pageSize);

        IEnumerable<ClientPointDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo);
    }
}
