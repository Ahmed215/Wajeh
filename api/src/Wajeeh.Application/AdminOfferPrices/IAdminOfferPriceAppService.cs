using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wajeeh.AdminOfferPrices.Dto;

namespace Wajeeh.AdminOfferPrices
{
    public interface IAdminOfferPriceAppService : IAsyncCrudAppService<AdminOfferPriceDto, long, AdminPagedOfferPriceResultRequestDto, AdminCreateOfferPriceDto, AdminUpdateOfferPriceDto>
    {
        Task<List<TopRequestSalesDriverDto>> GetTopRequestSalesDrivers(GetTopRequestSalesDriverInput input);
    }
}
