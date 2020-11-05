using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wajeeh.AdminReuests.Dto;

namespace Wajeeh.AdminReuests
{
    public interface IAdminRequestAppService : IAsyncCrudAppService<AdminRequestDto, long, AdminPagedRequestResultRequestDto, AdminCreateRequestDto, AdminUpdateRquestDto>
    {
        long CreateRequestFromAdminPanel(AdminCreateRequestWithOfferDto input);
        bool UpdaterequestFromAdminPanel(AdminCreateRequestWithOfferDto input);
        bool CancelRequestFromAdminPanel(long input);
        Task<List<TopRequestSalesClientDto>> GetTopRequestSalesClients(GetTopRequestSalesClientInput input);
    }
}
