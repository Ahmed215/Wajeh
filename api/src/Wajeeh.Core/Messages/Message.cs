using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Messages
{
    public class Message : FullAuditedEntity<long>
    {
        public string Content { get; set; }
        public long From { get; set; }
        public long To { get; set; }
    }
}
