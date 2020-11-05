using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminDrivers.Dto;

namespace Wajeeh.AdminDrivers
{
    public interface IAdminDriverAppService : IAsyncCrudAppService<AdminDriverDto, long, AdminPagedDriverResultRequestDto, AdminCreateDriverDto, AdminUpdateDriverDto>
    {
    }
}
