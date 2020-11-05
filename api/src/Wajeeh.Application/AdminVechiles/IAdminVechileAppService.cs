using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wajeeh.AdminVechiles.Dto;

namespace Wajeeh.AdminVechiles
{
    public interface IAdminVechileAppService : IAsyncCrudAppService<AdminVechileDto, long, AdminPagedVechcileResultRequestDto, CreateAdminVechileDto, UpdateAdminVechileDto>
    {
        Task<string> CreateVechileAsync(CreateAdminVechileDto input);
    }
}
