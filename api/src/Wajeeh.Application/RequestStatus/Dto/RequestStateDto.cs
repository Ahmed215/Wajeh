using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.OfferPriceStatus;

namespace Wajeeh.RequestStatus.Dto
{
    [AutoMap(typeof(RequestState))]
    public class RequestStateDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
