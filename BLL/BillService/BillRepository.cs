using BLL.RepositoryService;
using DAL;
using DAL.ConstString;
using DAL.Entities;
using DTO.BillDataModel;
using DTO.UserDataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.BillService
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(GeneralDBContext context)
            : base(context)
        {
        }

        public new GeneralDBContext GeneralDBContext
        {
            get { return Context as GeneralDBContext; }
        }

        public int GetRecordsNumber(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    default:
                        return 0;
                }
            }
            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).Count();
                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public decimal GetTotalMinimum(int userId, DateTime dtStart)
        {
          return GeneralDBContext.Bills.AsNoTracking().Where(f => f.UserID == UserData.ID && f.Minimum != null && f.EndDate >= dtStart && f.EndDate <= DateTime.Now).Sum(s => s.Minimum) ?? 0;
        }

        public decimal GetTotalDevices(int userId, DateTime dtStart)
        {
            return GeneralDBContext.Bills.AsNoTracking().Where(f => f.UserID == UserData.ID && f.EndDate >= dtStart && f.EndDate <= DateTime.Now).Sum(s => s.DevicesSum) ?? 0;
        }

        public decimal GetTotalItems(int userId, DateTime dtStart)
        {
            return GeneralDBContext.Bills.AsNoTracking().Where(f => f.UserID == UserData.ID && f.EndDate >= dtStart && f.EndDate <= DateTime.Now).Sum(s => s.ItemsSum) ?? 0;
        }

        public decimal GetTotalDiscount(int userId, DateTime dtStart)
        {
            return GeneralDBContext.Bills.AsNoTracking().Where(f => f.UserID == UserData.ID && f.EndDate >= dtStart && f.EndDate <= DateTime.Now).Sum(s => s.Discount) ?? 0;
        }

        public decimal? DevicesSum(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.DevicesSum != null).Sum(s => s.DevicesSum);
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public decimal? ItemsSum(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    default:
                        return null;
                }
            }
            else
                return null;

        }

        public decimal? DiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.Discount != null).Sum(s => s.Discount);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.ItemsSum != null).Sum(s => s.ItemsSum);
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public decimal? TotalAfterDiscountSum(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {

            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo && w.TotalAfterDiscount != null).Sum(s => s.TotalAfterDiscount);
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public Bill GetItemsBill()
        {
            return GeneralDBContext.Bills.AsNoTracking().SingleOrDefault(s => s.EndDate == null && s.Type == BillTypeText.Items);
        }

        public Bill GetById(int id)
        {
            return GeneralDBContext.Bills.Find(id);
        }

        public Bill GetLastBill(int deviceId)
        {
            return GeneralDBContext.BillsDevices.AsNoTracking().Where(w => w.Bill.EndDate != null && w.DeviceID == deviceId).OrderByDescending(o => o.EndDate).FirstOrDefault()?.Bill;
        }

        public IEnumerable<BillDayDataModel> Search(DateTime date)
        {
            if (UserData.Role == RoleText.Admin)
            {
                return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Date == date && w.Type == BillTypeText.Devices).OrderBy(o => o.ID).Select(s => new BillDayDataModel
                {
                    Bill = s,
                    User = s.User,
                    Client = s.Client
                }).ToList();
            }
            else if (UserData.Role == RoleText.Tax)
            {
                return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Date == date && w.Type == BillTypeText.Devices).OrderBy(o => o.ID).Select(s => new BillDayDataModel
                {
                    Bill = s,
                    User = s.User,
                    Client = s.Client
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<BillDisplayDataModel> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo, int pageNumber, int pageSize)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(s => new BillDisplayDataModel
                        {
                            Bill = s,
                            User = s.User,
                            Client = s.Client,
                            Case = s.Canceled == true ? BillCaseText.Canceled : s.Deleted == true ? BillCaseText.Deleted : BillCaseText.Available
                        }).ToList();
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public IEnumerable<Bill> Search(string billCase, string key, DateTime dtFrom, DateTime dtTo)
        {
            if (UserData.Role == RoleText.Admin)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    default:
                        return null;
                }
            }

            else if (UserData.Role == RoleText.Tax)
            {
                switch (billCase)
                {
                    case BillCaseText.All:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Available:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == false && w.Canceled == false && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Canceled:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Canceled == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    case BillCaseText.Deleted:
                        return GeneralDBContext.Bills.AsNoTracking().Where(w => w.TotalAfterDiscount <= 30 && w.Deleted == true && w.EndDate != null && w.Type == BillTypeText.Devices && (w.ID.ToString() + w.Client.Name + w.User.Name + w.ID.ToString()).Contains(key) && w.Date >= dtFrom && w.Date <= dtTo).OrderBy(o => o.ID).ToList();
                    default:
                        return null;
                }
            }
            else
                return null;

        }

    }
}
