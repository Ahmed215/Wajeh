using Abp;
using Abp.Dependency;
using Abp.Notifications;
using System.Threading.Tasks;

namespace Wajeeh.ChatRooms
{
    public class PublishMessageAppService : ITransientDependency, IPublishMessageAppService
    {
        private readonly INotificationPublisher notificationPublisher;

        public PublishMessageAppService(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public async Task PublishMessage(string senderUserName, string Message, UserIdentifier targetUserId)
        {
            await notificationPublisher.PublishAsync("SentMessageRequest", new SentMessageNotificationData(senderUserName, Message), userIds: new[] { targetUserId });
        }
    }
}
