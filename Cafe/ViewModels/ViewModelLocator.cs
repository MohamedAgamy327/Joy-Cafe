using Cafe.ViewModels.BillViewModels;
using Cafe.ViewModels.CashierViewModels;
using Cafe.ViewModels.ClientViewModels;
using Cafe.ViewModels.DeviceViewModels;
using Cafe.ViewModels.ItemViewModels;
using Cafe.ViewModels.MembershipViewModels;
using Cafe.ViewModels.ReportViewModels;
using Cafe.ViewModels.SafeViewModels;
using Cafe.ViewModels.ShiftViewModels;
using Cafe.ViewModels.SpendingViewModels;
using Cafe.ViewModels.UserViewModels;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Cafe.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<UserDisplayViewModel>();
            SimpleIoc.Default.Register<ShiftViewModel>();
            SimpleIoc.Default.Register<ShiftDisplayViewModel>();
            SimpleIoc.Default.Register<DeviceViewModel>();
            SimpleIoc.Default.Register<DeviceTypeDisplayViewModel>();
            SimpleIoc.Default.Register<DeviceDisplayViewModel>();
            SimpleIoc.Default.Register<ItemViewModel>();
            SimpleIoc.Default.Register<ItemDisplayViewModel>();
            SimpleIoc.Default.Register<ItemReportViewModel>();
            SimpleIoc.Default.Register<ClientViewModel>();
            SimpleIoc.Default.Register<ClientDisplayViewModel>();
            SimpleIoc.Default.Register<ClientPointViewModel>();
            SimpleIoc.Default.Register<MembershipViewModel>();
            SimpleIoc.Default.Register<MembershipDisplayViewModel>();
            SimpleIoc.Default.Register<ClientMembershipViewModel>();
            SimpleIoc.Default.Register<ClientMembershipMinuteViewModel>();
            SimpleIoc.Default.Register<SpendingViewModel>();
            SimpleIoc.Default.Register<SpendingDisplayViewModel>();
            SimpleIoc.Default.Register<SpendingReportViewModel>();
            SimpleIoc.Default.Register<SafeViewModel>();
            SimpleIoc.Default.Register<SafeDisplayViewModel>();
            SimpleIoc.Default.Register<SafeReportViewModel>();
            SimpleIoc.Default.Register<CashierViewModel>();
            SimpleIoc.Default.Register<DevicesViewModel>();
            SimpleIoc.Default.Register<ShiftItemsViewModel>();
            SimpleIoc.Default.Register<ShiftSpendingViewModel>();
            SimpleIoc.Default.Register<BillItemsViewModel>();
            SimpleIoc.Default.Register<AccountPaidViewModel>();
            SimpleIoc.Default.Register<BillViewModel>();
            SimpleIoc.Default.Register<BillDisplayViewModel>();
            SimpleIoc.Default.Register<BillDayViewModel>();
            SimpleIoc.Default.Register<BillShowViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UserViewModel User
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserViewModel>();
            }
        }

        public UserDisplayViewModel UserDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserDisplayViewModel>();
            }
        }

        public ShiftViewModel Shift
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftViewModel>();
            }
        }

        public ShiftDisplayViewModel ShiftDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftDisplayViewModel>();
            }
        }

        public DeviceViewModel Device
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeviceViewModel>();
            }
        }

        public DeviceTypeDisplayViewModel DeviceTypeDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeviceTypeDisplayViewModel>();
            }
        }

        public DeviceDisplayViewModel DeviceDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DeviceDisplayViewModel>();
            }
        }

        public ItemViewModel Item
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemViewModel>();
            }
        }

        public ItemDisplayViewModel ItemDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemDisplayViewModel>();
            }
        }

        public ItemReportViewModel ItemReport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemReportViewModel>();
            }
        }

        public ClientViewModel Client
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientViewModel>();
            }
        }

        public ClientDisplayViewModel ClientDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientDisplayViewModel>();
            }
        }

        public ClientPointViewModel ClientPoint
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientPointViewModel>();
            }
        }

        public MembershipViewModel Membership
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MembershipViewModel>();
            }
        }

        public MembershipDisplayViewModel MembershipDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MembershipDisplayViewModel>();
            }
        }

        public ClientMembershipViewModel ClientMembership
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientMembershipViewModel>();
            }
        }

        public ClientMembershipMinuteViewModel ClientMembershipMinute
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClientMembershipMinuteViewModel>();
            }
        }

        public SpendingViewModel Spending
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingViewModel>();
            }
        }

        public SpendingReportViewModel SpendingReport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingReportViewModel>();
            }
        }

        public SpendingDisplayViewModel SpendingDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpendingDisplayViewModel>();
            }
        }

        public SafeViewModel Safe
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeViewModel>();
            }
        }

        public SafeDisplayViewModel SafeDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeDisplayViewModel>();
            }
        }

        public SafeReportViewModel SafeReport
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SafeReportViewModel>();
            }
        }

        public CashierViewModel Cashier
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CashierViewModel>();
            }
        }

        public DevicesViewModel Devices
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DevicesViewModel>();
            }
        }

        public BillItemsViewModel BillItems
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillItemsViewModel>();
            }
        }

        public ShiftSpendingViewModel ShiftSpending
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftSpendingViewModel>();
            }
        }

        public ShiftItemsViewModel ShiftItems
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShiftItemsViewModel>();
            }
        }

        public AccountPaidViewModel AccountPaid
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AccountPaidViewModel>();
            }
        }

        public BillViewModel Bill
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillViewModel>();
            }
        }

        public BillDisplayViewModel BillDisplay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillDisplayViewModel>();
            }
        }

        public BillDayViewModel BillDay
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillDayViewModel>();
            }
        }

        public BillShowViewModel BillShow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BillShowViewModel>();
            }
        }

        public ReportViewModel Report
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReportViewModel>();
            }
        }

        public static void Cleanup(string viewModel)
        {
            switch (viewModel)
            {
                case "Main":
                    SimpleIoc.Default.Unregister<MainViewModel>();
                    break;

                case "User":
                    SimpleIoc.Default.Unregister<UserViewModel>();
                    SimpleIoc.Default.Register<UserViewModel>();
                    break;

                case "UserDisplay":
                    SimpleIoc.Default.Unregister<UserDisplayViewModel>();
                    SimpleIoc.Default.Register<UserDisplayViewModel>();
                    break;

                case "Shift":
                    SimpleIoc.Default.Unregister<ShiftViewModel>();
                    SimpleIoc.Default.Register<ShiftViewModel>();
                    break;

                case "ShiftDisplay":
                    SimpleIoc.Default.Unregister<ShiftDisplayViewModel>();
                    SimpleIoc.Default.Register<ShiftDisplayViewModel>();
                    break;

                case "Device":
                    SimpleIoc.Default.Unregister<DeviceViewModel>();
                    SimpleIoc.Default.Register<DeviceViewModel>();
                    break;

                case "DeviceTypeDisplay":
                    SimpleIoc.Default.Unregister<DeviceTypeDisplayViewModel>();
                    SimpleIoc.Default.Register<DeviceTypeDisplayViewModel>();
                    break;

                case "DeviceDisplay":
                    SimpleIoc.Default.Unregister<DeviceDisplayViewModel>();
                    SimpleIoc.Default.Register<DeviceDisplayViewModel>();
                    break;

                case "Item":
                    SimpleIoc.Default.Unregister<ItemViewModel>();
                    SimpleIoc.Default.Register<ItemViewModel>();
                    break;

                case "ItemDisplay":
                    SimpleIoc.Default.Unregister<ItemDisplayViewModel>();
                    SimpleIoc.Default.Register<ItemDisplayViewModel>();
                    break;

                case "ItemReport":
                    SimpleIoc.Default.Unregister<ItemReportViewModel>();
                    SimpleIoc.Default.Register<ItemReportViewModel>();
                    break;

                case "Client":
                    SimpleIoc.Default.Unregister<ClientViewModel>();
                    SimpleIoc.Default.Register<ClientViewModel>();
                    break;

                case "ClientDisplay":
                    SimpleIoc.Default.Unregister<ClientDisplayViewModel>();
                    SimpleIoc.Default.Register<ClientDisplayViewModel>();
                    break;

                case "ClientPoint":
                    SimpleIoc.Default.Unregister<ClientPointViewModel>();
                    SimpleIoc.Default.Register<ClientPointViewModel>();
                    break;

                case "Membership":
                    SimpleIoc.Default.Unregister<MembershipViewModel>();
                    SimpleIoc.Default.Register<MembershipViewModel>();
                    break;

                case "MembershipDisplay":
                    SimpleIoc.Default.Unregister<MembershipDisplayViewModel>();
                    SimpleIoc.Default.Register<MembershipDisplayViewModel>();
                    break;

                case "ClientMembership":
                    SimpleIoc.Default.Unregister<ClientMembershipViewModel>();
                    SimpleIoc.Default.Register<ClientMembershipViewModel>();
                    break;

                case "ClientMembershipMinute":
                    SimpleIoc.Default.Unregister<ClientMembershipMinuteViewModel>();
                    SimpleIoc.Default.Register<ClientMembershipMinuteViewModel>();
                    break;

                case "Spending":
                    SimpleIoc.Default.Unregister<SpendingViewModel>();
                    SimpleIoc.Default.Register<SpendingViewModel>();
                    break;

                case "SpendingDisplay":
                    SimpleIoc.Default.Unregister<SpendingDisplayViewModel>();
                    SimpleIoc.Default.Register<SpendingDisplayViewModel>();
                    break;

                case "SpendingReport":
                    SimpleIoc.Default.Unregister<SpendingReportViewModel>();
                    SimpleIoc.Default.Register<SpendingReportViewModel>();
                    break;

                case "Safe":
                    SimpleIoc.Default.Unregister<SafeViewModel>();
                    SimpleIoc.Default.Register<SafeViewModel>();
                    break;

                case "SafeDisplay":
                    SimpleIoc.Default.Unregister<SafeDisplayViewModel>();
                    SimpleIoc.Default.Register<SafeDisplayViewModel>();
                    break;

                case "SafeReport":
                    SimpleIoc.Default.Unregister<SafeReportViewModel>();
                    SimpleIoc.Default.Register<SafeReportViewModel>();
                    break;

                case "Cashier":
                    SimpleIoc.Default.Unregister<CashierViewModel>();
                    SimpleIoc.Default.Register<CashierViewModel>();
                    break;

                case "Devices":
                    SimpleIoc.Default.Unregister<DevicesViewModel>();
                    SimpleIoc.Default.Register<DevicesViewModel>();
                    break;

                case "BillItems":
                    SimpleIoc.Default.Unregister<BillItemsViewModel>();
                    SimpleIoc.Default.Register<BillItemsViewModel>();
                    break;

                case "ShiftSpending":
                    SimpleIoc.Default.Unregister<ShiftSpendingViewModel>();
                    SimpleIoc.Default.Register<ShiftSpendingViewModel>();
                    break;

                case "ShiftItems":
                    SimpleIoc.Default.Unregister<ShiftItemsViewModel>();
                    SimpleIoc.Default.Register<ShiftItemsViewModel>();
                    break;

                case "AccountPaid":
                    SimpleIoc.Default.Unregister<AccountPaidViewModel>();
                    SimpleIoc.Default.Register<AccountPaidViewModel>();
                    break;

                case "Bill":
                    SimpleIoc.Default.Unregister<BillViewModel>();
                    SimpleIoc.Default.Register<BillViewModel>();
                    break;

                case "BillDisplay":
                    SimpleIoc.Default.Unregister<BillDisplayViewModel>();
                    SimpleIoc.Default.Register<BillDisplayViewModel>();
                    break;

                case "BillDay":
                    SimpleIoc.Default.Unregister<BillDayViewModel>();
                    SimpleIoc.Default.Register<BillDayViewModel>();
                    break;

                case "BillShow":
                    SimpleIoc.Default.Unregister<BillShowViewModel>();
                    SimpleIoc.Default.Register<BillShowViewModel>();
                    break;

                case "Report":
                    SimpleIoc.Default.Unregister<ReportViewModel>();
                    SimpleIoc.Default.Register<ReportViewModel>();
                    break;

                default:
                    break;
            }


        }
    }
}