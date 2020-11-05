using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.WaselDrivers
{
    public class WaselDriver : FullAuditedEntity<long>
    {
        public string identityNumber { get; set; }
        public string dateOfBirthGregorian { get; set; }
        public string mobileNumber { get; set; }
        public string email { get; set; }
    }
}
