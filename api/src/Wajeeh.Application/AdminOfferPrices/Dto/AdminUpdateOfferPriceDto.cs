using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.OfferPrices;

namespace Wajeeh.AdminOfferPrices.Dto
{
    [AutoMap(typeof(OfferPrice))]
    public class AdminUpdateOfferPriceDto : EntityDto<long>
    {
        public double DeliveryCost { get; set; }
        public string AwayFrom { get; set; }
        public int DeliveryThroughDays { get; set; }
        public int DeliveryThroughHours { get; set; }
        public int DeliveryThroughMinutes { get; set; }
        public int DeliveryThroughSeconds { get; set; }
        public long RequestId { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
    }
}
