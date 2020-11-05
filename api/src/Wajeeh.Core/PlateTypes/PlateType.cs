using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.PlateTypes
{
    public class PlateType : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
