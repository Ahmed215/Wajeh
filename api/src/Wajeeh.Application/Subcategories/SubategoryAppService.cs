using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Subcategories.Dto;
using Abp.Linq.Extensions;
using System.Linq;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Wajeeh.Subcategories
{
    public class SubategoryAppService : AsyncCrudAppService<Subcategory, SubcategoryDto, long, PagedSubcategoryResultRequestDto, CreateSubcategoryDto, UpdateSubcategoryDto>, ISubcategoryAppService
    {
        private readonly IRepository<Subcategory, long> _repository;
        private readonly IHostingEnvironment _hostingEnv;
        private const string image = "";
        public SubategoryAppService(IRepository<Subcategory, long> repository,
            IHostingEnvironment hostingEnv) : base(repository)
        {
            _repository = repository;
            _hostingEnv = hostingEnv;
        }

        public List<SubcategoryDto> GetSubcategoriesByCategoryId(long categoryId)
        {
            var subcategories = _repository.GetAll().Where(s => s.CategoryId == categoryId).ToList();
            var subcategoriesDto = subcategories.Select(s=>MapToEntityDto(s)).ToList();
            return subcategoriesDto;
        }

        protected override IQueryable<Subcategory> CreateFilteredQuery(PagedSubcategoryResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.Keyword), t => t.Name == input.Keyword || t.NameAr == input.Keyword);
        }



        protected override SubcategoryDto MapToEntityDto(Subcategory entity)
        {
            var subcategoryDto = base.MapToEntityDto(entity);
            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {
                subcategoryDto.DisplayName = entity.NameAr;
                subcategoryDto.DisplayDescription = entity.DescriptionAr;
            }
            else
            {
                subcategoryDto.DisplayName = entity.Name;
                subcategoryDto.DisplayDescription = entity.Description;
            }
            subcategoryDto.Image = image;

            return subcategoryDto;
        }
    }
}
