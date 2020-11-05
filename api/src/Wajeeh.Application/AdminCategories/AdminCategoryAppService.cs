using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abp.Linq.Extensions;
using System.Text;
using Wajeeh.AdminCategories.Dto;
using Wajeeh.Categories;
using Microsoft.AspNetCore.Hosting;
using Wajeeh.Authorization;
using Abp.Authorization;
using Abp.Extensions;

namespace Wajeeh.AdminCategories
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminCategoryAppService : AsyncCrudAppService<Category, AdminCategoryDto, long, AdminPagedCategoryResultRequestDto, AdminCreateCategoryDto, AdminUpdateCategoryDto>, IAdminCategoryAppService
    {
        private readonly IHostingEnvironment _hostingEnv;

        private const string image = "";
        public AdminCategoryAppService(IRepository<Category, long> repository,
            IHostingEnvironment hostingEnv) : base(repository)
        {
            _hostingEnv = hostingEnv;
        }

        protected override IQueryable<Category> CreateFilteredQuery(AdminPagedCategoryResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!input.Keyword.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.Keyword);
                    string nameAr = filter_query["NameAr"];
                    query = query.WhereIf(!nameAr.IsNullOrEmpty(), t => t.NameAr.Contains(nameAr));
                    string name = filter_query["Name"];
                    query = query.WhereIf(!name.IsNullOrEmpty(), t => t.Name.Contains(name));
                }
                catch (Exception )
                {
                    query = query.Where(t => t.Name == input.Keyword || t.NameAr == input.Keyword);
                }
            }
            return query;
        }
        protected override AdminCategoryDto MapToEntityDto(Category entity)
        {
            var categoryDto = base.MapToEntityDto(entity);
            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {
                categoryDto.DisplayName = entity.NameAr;
                categoryDto.DisplayDescription = entity.DescriptionAr;
            }
            else
            {
                categoryDto.DisplayName = entity.Name;
                categoryDto.DisplayDescription = entity.Description;
            }
            categoryDto.Image = image;

            return categoryDto;
        }
    }
}
