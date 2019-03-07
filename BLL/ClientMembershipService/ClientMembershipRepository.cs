using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.RepositoryService;
using DAL;
using DAL.Entities;
using DTO.ClientMembershipDataModel;

namespace BLL.ClientMembershipService
{
    public class ClientMembershipRepository : GenericRepository<ClientMembership>, IClientMembershipRepository
    {
        public ClientMembershipRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string key, DateTime dtFrom, DateTime dtTo)
        {
            return GeneralDBContext.ClientsMemberships.AsNoTracking().Where(w => (w.Client.Name + w.Membership.Name + w.Client.Code + w.Client.Telephone).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
        }

        public List<ClientMembershipDisplayDataModel> Search(string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            return GeneralDBContext.ClientsMemberships.AsNoTracking().OrderByDescending(o => o.RegistrationDate).Where(w => (w.Client.Name + w.Membership.Name + w.Client.Code + w.Client.Telephone).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .Select(s => new ClientMembershipDisplayDataModel
                    {
                        Client = s.Client,
                        DeviceType = s.Membership.DeviceType,
                        Membership = s.Membership,
                        ClientMembership = s,
                        User = s.User,
                        CanDelete = GeneralDBContext.ClientMembershipMinutes.FirstOrDefault(d => d.ClientID == s.ClientID && d.DeviceTypeID == s.Membership.DeviceTypeID).Minutes >= s.Membership.Minutes ? true : false
                    }).ToList();
        }
    }
}
