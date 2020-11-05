using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Wajeeh.OfferPriceStatus.Dto
{
    [AutoMap(typeof(OfferPriceState))]
    public class UpdateOfferPriceStatusDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
