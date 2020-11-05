using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using Wajeeh.ClientAdresses;
using Wajeeh.Clinets;
using Wajeeh.DriverNotifications;
using Wajeeh.Drivers;
using Wajeeh.NotificationLogs;
using Wajeeh.NotificationTokens;
using Wajeeh.OfferPrices;
using Wajeeh.Requests;

namespace Wajeeh.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public Client Client { get; set; }
        public Driver Driver { get; set; }
        public NotificationToken NotificationToken { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<ClientAdress> ClientAdresses { get; set; }
        public virtual ICollection<NotificationLog> NotificationLogs { get; set; }
        ICollection<DriverNotification> DriverNotifications { get; set; }
        //public virtual ICollection<OfferPrice> OfferPrices { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
