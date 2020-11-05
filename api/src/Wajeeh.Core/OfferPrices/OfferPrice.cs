using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.Requests;

namespace Wajeeh.OfferPrices
{
    public class OfferPrice : FullAuditedEntity<long>
    {
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDriverRead { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
        public bool? IsAccepted { get; set; }
        public int OfferStatus { get; set; }
        public string DriverName { get; set; }
        public string ClientName { get; set; }
        public double DeliveryCost { get; set; }
        public double VAT { get; set; }
        public string AwayFrom { get; set; }
        public int DeliveryThroughDays { get; set; }
        public int DeliveryThroughHours { get; set; }
        public int DeliveryThroughMinutes { get; set; }
        public int DeliveryThroughSeconds { get; set; }
        [ForeignKey("Request")]
        public long RequestId { get; set; }
        public Request Request { get; set; }
        //[ForeignKey("User")]
        public long DriverId { get; set; }
        //public User Driver { get; set; }
    }
}
