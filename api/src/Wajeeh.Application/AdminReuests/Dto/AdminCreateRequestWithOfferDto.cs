using Abp.AutoMapper;
using Wajeeh.AdminOfferPrices.Dto;
using Wajeeh.OfferPrices;
using Wajeeh.Requests;

namespace Wajeeh.AdminReuests.Dto
{
    public class AdminCreateRequestWithOfferDto
    {
        [AutoMap(typeof(Request))]
        public AdminCreateRequestDto adminCreateRequestDto { get; set; }
        [AutoMap(typeof(OfferPrice))]
        public AdminOfferPriceDto adminOfferPriceDto { get; set; }
    }
}
