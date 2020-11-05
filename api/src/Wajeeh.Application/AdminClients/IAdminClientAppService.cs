using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminClients.Dto;

namespace Wajeeh.AdminClients
{
    public interface IAdminClientAppService : IAsyncCrudAppService<AdminClientDto, long, AdminPagedClientResultRequestDto, AdminCreateClientDto, AdminUpdateClientDto>
    {
    }
}
