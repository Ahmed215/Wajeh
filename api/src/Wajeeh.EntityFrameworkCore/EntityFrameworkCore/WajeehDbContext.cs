using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Wajeeh.Authorization.Roles;
using Wajeeh.Authorization.Users;
using Wajeeh.MultiTenancy;
using Wajeeh.Drivers;
using Wajeeh.Clinets;
using Wajeeh.Categories;
using Wajeeh.Subcategories;
using Wajeeh.Requests;
using Wajeeh.ClientAdresses;
using Wajeeh.OfferPrices;
using Wajeeh.ClientAppAbouts;
using Wajeeh.ClientAppTermsAndPolicies;
using Wajeeh.NotificationTokens;
using Wajeeh.NotificationLogs;
using Wajeeh.PhoneCodeConfirms;
using Wajeeh.Companies;
using Wajeeh.CompanyClients;
using Wajeeh.Messages;
using Wajeeh.DriverNotifications;
using Wajeeh.Vechiles;
using Wajeeh.WaselDrivers;
using Wajeeh.DriverJoinRequests;
using Wajeeh.OfferPriceStatus;
using Wajeeh.RequestStatus;
using Wajeeh.PlateTypes;
using Wajeeh.TrackingTrips;
using Wajeeh.Coupons;

namespace Wajeeh.EntityFrameworkCore
{
    public class WajeehDbContext : AbpZeroDbContext<Tenant, Role, User, WajeehDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Subcategory> Subcategories { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<ClientAdress> ClientAdresses { get; set; }
        public virtual DbSet<OfferPrice> OfferPrices { get; set; }
        public virtual DbSet<ClientAppAbout> ClientAppAbouts { get; set; }
        public virtual DbSet<ClientAppTermsAndPolicy> ClientAppTermsAndPolicies { get; set; }
        public virtual DbSet<NotificationToken> NotificationTokens { get; set; }
        public virtual DbSet<NotificationLog> NotificationLogs { get; set; }
        public virtual DbSet<PhoneCodeConfirm> PhoneCodeConfirms { get; set; }
        public virtual DbSet<CompanyClient> CompanyClients { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<DriverNotification> DriverNotifications { get; set; }
        public virtual DbSet<Vechile> Vechiles { get; set; }
        public virtual DbSet<WaselDriver> WaselDrivers { get; set; }
        public virtual DbSet<DriverJoinRequest> DriverJoinRequests { get; set; }
        public virtual DbSet<OfferPriceState> OfferPriceStatus { get; set; }
        public virtual DbSet<RequestState> RequestStatus { get; set; }
        public virtual DbSet<PlateType> PlateTypes { get; set; }

        public virtual DbSet<TrackingTrip> TrackingTrips { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }


        public WajeehDbContext(DbContextOptions<WajeehDbContext> options)
            : base(options)
        {
        }
    }
}
