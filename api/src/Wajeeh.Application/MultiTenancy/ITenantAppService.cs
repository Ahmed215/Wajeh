using Abp.Application.Services;
using Wajeeh.MultiTenancy.Dto;

namespace Wajeeh.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

