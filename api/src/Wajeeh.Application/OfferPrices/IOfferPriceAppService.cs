using Abp.Application.Services;
using Wajeeh.OfferPrices.Dto;

namespace Wajeeh.OfferPrices
{
    public interface IOfferPriceAppService : IAsyncCrudAppService<OfferPriceDto, long, PagedOfferPriceResultRequestDto, CreateOfferPriceDto, UpdateOfferPriceDto>
    {
        OfferPriceDto OfferPriceByRequestId(GetOfferPriceDto input);
    }
}
