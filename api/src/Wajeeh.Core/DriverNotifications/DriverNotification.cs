using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.Drivers;

namespace Wajeeh.DriverNotifications
{
    public class DriverNotification : FullAuditedEntity<long>
    {
        public int Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool? IsRead { get; set; }
        [ForeignKey("Driver")]
        public long DriverId { get; set; }
        public User Driver { get; set; }
    }
}
