using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.ClientDataModel;

namespace BLL.ClientService
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {

        public ClientRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key)
        {
            return GeneralDBContext.Clients.Where(s => (s.Name + s.Telephone + s.Code).Contains(key)).Count();
        }

        public List<string> GetTelephoneSuggetions()
        {
            return GeneralDBContext.Clients.AsNoTracking().OrderBy(o => o.Telephone).Select(s => s.Telephone).Distinct().ToList();
        }

        public List<ClientDisplayDataModel> Search(string key, int pageNumber, int pageSize)
        {
            return GeneralDBContext.Clients.Where(w => (w.Name + w.Telephone + w.Code).Contains(key)).OrderBy(o => o.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).
      Select(s => new ClientDisplayDataModel
      {
          Client = s,
          CanDelete = s.Bills.Count > 0 ? false : s.ClientsMemberships.Count > 0 ? false : true
      }).ToList();
        }

        public List<ClientPointDataModel> Search(string key, int pageNumber, int pageSize, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.Clients.AsNoTracking().Where(w => (w.Name + w.Telephone + w.Code).Contains(key)).OrderBy(o => o.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                   .Select(s => new ClientPointDataModel
                   {
                       Client = s,
                       Points = s.Bills.Where(w => w.Date >= dtFrom && w.Date <= dtTo).Sum(k => k.EarnedPoints).HasValue ? s.Bills.Where(w => w.Date >= dtFrom && w.Date <= dtTo).Sum(k => k.EarnedPoints) : 0
                   }).OrderByDescending(o => o.Points)
                     .ToList();
        }

    }
}
