//using Abp;
//using Abp.Application.Services;
//using Abp.Notifications;
//using Abp.RealTime;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Wajeeh.SignalR
//{
//    public class SignalR:ApplicationService
//    {
//        private IOnlineClientManager _onlineClientManager;
//        private IHubContext<Hub> _hubContext;
//        private IUserIdentifier u;
//        IRealTimeNotifier realTimeNotifier;
//        public SignalR(IOnlineClientManager onlineClientManager)
//        {
//            _onlineClientManager = onlineClientManager;
//        }

//        public async Task f()
//        {
//            var x = new Dictionary<string, object>();
//            x.Add("l;l", ",;'l,");
//            UserNotification[] usn = new UserNotification[] {
//                new UserNotification()
//                {
//                    Notification=new TenantNotification()
//                    {
//                        Data=new NotificationData{ 
//                            Properties=x
//                        }
//                    }
//                }
//            };
//            var onlineClients = _onlineClientManager.GetAllByUserId(u);
//            foreach (var onlineClient in onlineClients)
//            {
//                var signalRClient = _hubContext.Clients.Client(onlineClient.ConnectionId);
//                await signalRClient.SendAsync("getNotification", usn);
//            }
            
//            await realTimeNotifier.SendNotificationsAsync(usn);

//        }
//    }
//}
