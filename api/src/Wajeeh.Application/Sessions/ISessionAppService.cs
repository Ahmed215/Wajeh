using System.Threading.Tasks;
using Abp.Application.Services;
using Wajeeh.Sessions.Dto;

namespace Wajeeh.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
