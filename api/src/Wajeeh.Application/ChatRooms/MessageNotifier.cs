using Abp.Dependency;
using Abp.Net.Mail;
using Abp.Notifications;
using System;
using System.Threading.Tasks;
using Wajeeh.Authorization.Users;

namespace Wajeeh.ChatRooms
{
    public class MessageNotifier : IRealTimeNotifier, ITransientDependency
    {
        private readonly UserManager _userManager;
        private readonly IEmailSender _emailSender;

        public MessageNotifier(UserManager userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public void SendNotifications(UserNotification[] userNotifications)
        {
            throw new NotImplementedException();
        }

        public async Task SendNotificationsAsync(UserNotification[] userNotifications)
        {
            foreach (var userNotification in userNotifications)
            {
                if (userNotification.Notification.Data is MessageNotificationData data)
                {
                    var user = await _userManager.GetUserByIdAsync(userNotification.UserId);

                    _emailSender.Send(
                        to: "goharym.dev@outlook.com",
                        subject: "You have a new notification!",
                        body: data.Message,
                        isBodyHtml: true
                    );
                }
            }
        }
    }
}
