using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Vechiles
{
    public class Vechile : FullAuditedEntity<long>
    {
        public string SequenceNumber { get; set; }
        public string Plate { get; set; }
        public int PlateType { get; set; }
    }
}
