using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.PhoneCodeConfirms
{
    public class PhoneCodeConfirm : FullAuditedEntity<long>
    {
        public string Phone { get; set; }
        public string Code { get; set; }
    }
}
