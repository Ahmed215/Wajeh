using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Wajeeh.OfferPriceStatus;

namespace Wajeeh.OfferPriceStatus.Dto
{
    [AutoMap(typeof(OfferPriceState))]
    public class OfferPriceStatusDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
