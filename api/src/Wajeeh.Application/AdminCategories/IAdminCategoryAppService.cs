using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminCategories.Dto;

namespace Wajeeh.AdminCategories
{
    public interface IAdminCategoryAppService : IAsyncCrudAppService<AdminCategoryDto, long, AdminPagedCategoryResultRequestDto, AdminCreateCategoryDto, AdminUpdateCategoryDto>
    {
    }
}
