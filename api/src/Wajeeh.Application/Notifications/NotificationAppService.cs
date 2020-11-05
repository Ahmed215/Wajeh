using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using Wajeeh.NotificationTokens;
using System.Linq;
using Wajeeh.Notifications.Dto;

namespace Wajeeh.Notifications
{
    public class NotificationAppService : ApplicationService,INotificationAppService
    {
        private readonly IRepository<NotificationToken, long> _notificationTokenRepository;
        public NotificationAppService(IRepository<NotificationToken, long> notificationTokenRepository)
        {
            _notificationTokenRepository = notificationTokenRepository;
        }

        public UserFireTokenDto SetUserFireToken(SetUserFireTokenDto input)
        {
            var userId = AbpSession.GetUserId();
            var notif= _notificationTokenRepository.GetAll().Where(n => n.UserId == userId).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(input.Token))
                return new UserFireTokenDto() { Token= notif.Token };

            if (notif == null)
            {
                _notificationTokenRepository.Insert(new NotificationToken() {
                    UserId=userId,
                    Token=input.Token
                });
                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                notif.Token = input.Token;
                _notificationTokenRepository.Update(notif);
                CurrentUnitOfWork.SaveChanges();
            }

            return new UserFireTokenDto
            {
                Token=input.Token
            };
        }
    }
}
