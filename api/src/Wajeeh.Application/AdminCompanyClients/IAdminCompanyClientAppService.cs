using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminCompanyClients.Dto;

namespace Wajeeh.AdminCompanyClients
{
    public interface IAdminCompanyClientAppService : IAsyncCrudAppService<AdminCompanyClientDto, long, AdminPagedCompanyClientResultRequestDto, AdminCreateCompanyClientDto, AdminUpdateCompanyClientDto>
    {
    }
}
