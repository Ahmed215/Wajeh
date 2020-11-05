using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminWaselDrivers.Dto;

namespace Wajeeh.AdminWaselDrivers
{
    public interface IAdminWaselDriverAppService : IAsyncCrudAppService<AdminWaselDriverDto, long, AdminPagedWaselDriverResultRequestDto, AdminCreateWaselDriverDto, AdminUpdateWaselDriverDto>
    {
    }
}
