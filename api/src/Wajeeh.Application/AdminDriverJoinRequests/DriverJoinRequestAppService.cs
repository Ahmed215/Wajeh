using Abp.Application.Services;
using Abp.Domain.Repositories;
using Wajeeh.AdminDriverJoinRequests.Dto;
using Wajeeh.DriverJoinRequests;

namespace Wajeeh.AdminDriverJoinRequests
{
    public class DriverJoinRequestAppService : AsyncCrudAppService<DriverJoinRequest, DriverJoinRequestDto, long, AdminPagedDriverJoinRequestResultRequestDto, AdminCreateDriverJoinRequestDto, AdminUpdateDriverJoinRequestDto>, IDriverJoinRequestAppService
    {
        public DriverJoinRequestAppService(IRepository<DriverJoinRequest, long> repository) : base(repository)
        {
        }
    }
}
