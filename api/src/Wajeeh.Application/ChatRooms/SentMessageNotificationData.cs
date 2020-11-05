using Abp.Notifications;
using System;

namespace Wajeeh.ChatRooms
{
    [Serializable]
    public class SentMessageNotificationData : NotificationData
    {
        public string SenderUserName { get; set; }
        public string Message { get; set; }

        public SentMessageNotificationData(string senderUserName, string friendshipMessage)
        {
            SenderUserName = senderUserName;
            Message = friendshipMessage;
        }
    }
}
