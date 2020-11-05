using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.OfferPrices;

namespace Wajeeh.AdminOfferPrices.Dto
{
    [AutoMap(typeof(OfferPrice))]
    public class AdminOfferPriceDto : EntityDto<long>
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
        public int OfferStatus { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
    }


    public class TopRequestSalesDriverDto : EntityDto<long>
    {
        public string DriverName { get; set; }
        public int RequestsCount { get; set; }
        public string Email { get; set; }
        public string Phone{ get; set; }
    }
    public class GetTopRequestSalesDriverInput
    {
        public string DriverName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
