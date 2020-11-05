using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Subcategories.Dto;

namespace Wajeeh.Subcategories
{
    public interface ISubcategoryAppService : IAsyncCrudAppService<SubcategoryDto, long, PagedSubcategoryResultRequestDto, CreateSubcategoryDto, UpdateSubcategoryDto>
    {
        List<SubcategoryDto> GetSubcategoriesByCategoryId(long categoryId);
    }
}
