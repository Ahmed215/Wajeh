using Abp;
using Abp.Dependency;
using Abp.Notifications;
using System.Threading.Tasks;

namespace Wajeeh.ChatRooms
{
    public class SubscribeMessageAppService : ITransientDependency
    {
        private readonly INotificationSubscriptionManager notificationSubscriptionManager;

        public SubscribeMessageAppService(INotificationSubscriptionManager notificationSubscriptionManager)
        {
            this.notificationSubscriptionManager = notificationSubscriptionManager;
        }

        //Subscribe to a general notification
        public async Task SubscribeMessage(int? tenantId, long userId)
        {
            await notificationSubscriptionManager.SubscribeAsync(new UserIdentifier(tenantId, userId), "SentMessageRequest");
        }
    }
}
