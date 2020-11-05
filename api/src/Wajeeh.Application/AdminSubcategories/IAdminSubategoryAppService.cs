using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminSubcategories.Dto;

namespace Wajeeh.AdminSubcategories
{
    public interface IAdminSubategoryAppService : IAsyncCrudAppService<AdminSubcategoryDto, long, AdminPagedSubcategoryResultRequestDto, AdminCreateSubcategoryDto, AdminUpdateSubcategoryDto>
    {
    }
}
