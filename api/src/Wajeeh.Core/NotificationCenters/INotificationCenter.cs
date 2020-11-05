using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wajeeh.NotificationCenters
{
    public interface INotificationCenter : IDomainService
    {
        Task<bool> SendPushNotification(string[] deviceTokens, string title, string body, object data);
    }
}
