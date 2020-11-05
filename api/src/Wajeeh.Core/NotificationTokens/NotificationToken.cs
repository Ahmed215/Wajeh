using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;

namespace Wajeeh.NotificationTokens
{
    public class NotificationToken : FullAuditedEntity<long>
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
