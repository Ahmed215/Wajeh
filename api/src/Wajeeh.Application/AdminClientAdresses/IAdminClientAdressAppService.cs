using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminClientAdresses.Dto;

namespace Wajeeh.AdminClientAdresses
{
    public interface IAdminClientAdressAppService : IAsyncCrudAppService<AdminClientAdressDto, long, AdminPagedClientAdressResultRequestDto, AdminCreateClientAdressDto, AdminUpdateClientAdressDto>
    {
    }
}
