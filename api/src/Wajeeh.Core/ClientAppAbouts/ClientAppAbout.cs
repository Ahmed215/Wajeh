using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.ClientAppAbouts
{
    public class ClientAppAbout : FullAuditedEntity<long>
    {
        public string Content { get; set; }
        public string ContentAr { get; set; }
    }
}