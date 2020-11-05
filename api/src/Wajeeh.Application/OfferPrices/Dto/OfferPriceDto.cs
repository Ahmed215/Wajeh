using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPrices.Dto
{
    [AutoMap(typeof(OfferPrice))]
    public class OfferPriceDto : EntityDto<long>
    {
        public double VAT { get; set; }
        public double DeliveryCost { get; set; }
        public string AwayFrom { get; set; }
        public int DeliveryThroughDays { get; set; }
        public int DeliveryThroughHours { get; set; }
        public int DeliveryThroughMinutes { get; set; }
        public int DeliveryThroughSeconds { get; set; }
        public long RequestId { get; set; }
        public long DriverId { get; set; }
        public string DriverName { get; set; }
        public string ClientName { get; set; }
        public int OfferStatus { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDriverRead { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
        public long? UserId { get; set; }


    }
}
