using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories.Dto;

namespace Wajeeh.Categories
{
    public interface ICategoryAppService : IAsyncCrudAppService<CategoryDto, long, PagedCategoryResultRequestDto, CreateCategoryDto, UpdateCategoryDto>
    {
    }
}
