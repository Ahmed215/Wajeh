using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPriceStatus.Dto
{
    [AutoMap(typeof(OfferPriceState))]
    public class CreateOfferPriceStatusDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
