using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPriceStatus
{
    public class OfferPriceState : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
