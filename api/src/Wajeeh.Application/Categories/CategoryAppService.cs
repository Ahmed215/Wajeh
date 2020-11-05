using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories.Dto;
using Abp.Linq.Extensions;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Abp.Extensions;

namespace Wajeeh.Categories
{
    public class CategoryAppService : AsyncCrudAppService<Category, CategoryDto, long, PagedCategoryResultRequestDto, CreateCategoryDto, UpdateCategoryDto>, ICategoryAppService
    {
        private readonly IHostingEnvironment _hostingEnv;

        private const string image = "";
        public CategoryAppService(IRepository<Category, long> repository,
            IHostingEnvironment hostingEnv) : base(repository)
        {
            _hostingEnv = hostingEnv;
        }

        protected override IQueryable<Category> CreateFilteredQuery(PagedCategoryResultRequestDto input)
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
                catch (Exception)
                {
                    query = query.Where(t => t.Name == input.Keyword || t.NameAr == input.Keyword);
                }
            }
            return query;
        }
        protected override CategoryDto MapToEntityDto(Category entity)
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
