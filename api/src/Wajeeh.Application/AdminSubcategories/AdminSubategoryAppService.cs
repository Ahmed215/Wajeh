using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminSubcategories.Dto;
using Wajeeh.Subcategories;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.IO;
using Wajeeh.AdminCategories.Dto;
using Wajeeh.Authorization;
using Abp.Authorization;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Wajeeh.AdminSubcategories
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminSubategoryAppService : AsyncCrudAppService<Subcategory, AdminSubcategoryDto, long, AdminPagedSubcategoryResultRequestDto, AdminCreateSubcategoryDto, AdminUpdateSubcategoryDto>, IAdminSubategoryAppService
    {

        private IWebHostEnvironment _webHostingEnvironment;

        public AdminSubategoryAppService(IRepository<Subcategory, long> repository,
            IWebHostEnvironment webHostingEnvironment) : base(repository)
        {
            _webHostingEnvironment = webHostingEnvironment;
        }

        protected override IQueryable<Subcategory> CreateFilteredQuery(AdminPagedSubcategoryResultRequestDto input)
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
                catch
                {
                    query = query.Where(t => t.Name == input.Keyword || t.NameAr == input.Keyword);
                }
            }
            query = query.WhereIf(input.CategoryId.HasValue && input.CategoryId.Value > 0, x => x.CategoryId == input.CategoryId.Value);
            return query;
        }
        protected override AdminSubcategoryDto MapToEntityDto(Subcategory entity)
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
            if (!string.IsNullOrWhiteSpace(categoryDto.Image))
            {
                categoryDto.Image = GetFile(entity.Image, "SubcategoryPictures");
            }

            return categoryDto;
        }



        protected override IQueryable<Subcategory> ApplySorting(IQueryable<Subcategory> query, AdminPagedSubcategoryResultRequestDto input)
        {
            //if (input.Sorting != null)
            //{
            //    if (input.Sorting.StartsWith("Subcategory"))
            //    {
            //        if (input.Sorting.EndsWith("Asc"))
            //        {
            //            return query.OrderBy(p => p.Subcategory.Id);

            //        }
            //        else
            //        {
            //            return query.OrderByDescending(p => p.Subcategory.Id);

            //        }
            //    }


            //    else
            //    {
            //        return base.ApplySorting(query, input);
            //    }
            //}
            //else
            //{
            //    return base.ApplySorting(query, input);
            //}
            return query;// base.ApplySorting(query, input);
        }




        protected override Subcategory MapToEntity(AdminCreateSubcategoryDto createInput)
        {
            var client = base.MapToEntity(createInput);

            if (!string.IsNullOrWhiteSpace(createInput.Image))
            {
                var temp = SaveFile(createInput.Image, "SubcategoryPictures");
                if (temp != null)
                    client.Image = temp;
            }

            return client;
        }

        protected override void MapToEntity(AdminUpdateSubcategoryDto updateInput, Subcategory entity)
        {
            if (!string.IsNullOrWhiteSpace(updateInput.Image))
            {
                var temp = SaveFile(updateInput.Image, "SubcategoryPictures");
                if (temp != null)
                    updateInput.Image = temp;
                else
                    updateInput.Image = entity.Image;
            }
            else
            {
                updateInput.Image = entity.Image;
            }

            base.MapToEntity(updateInput, entity);
        }



        private String SaveFile(string file, string folder)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {

                var targetDirectory = Path.Combine(_webHostingEnvironment.WebRootPath, folder);
                Random rand = new Random();
                var fileName = rand.Next() * 10000 + (DateTime.Now).Ticks + Guid.NewGuid().ToString().Substring(1, 6) + ".Jpg";

                var savePath = Path.Combine(targetDirectory, fileName);

                byte[] bytes = Convert.FromBase64String(file);

                File.WriteAllBytes(savePath, bytes);

                return fileName;
            }
            return null;
        }

        private string GetFile(string fileName, string folder)
        {
            return folder + "/" + fileName;
        }

        [AbpAllowAnonymous]
        public async override Task<PagedResultDto<AdminSubcategoryDto>> GetAllAsync(AdminPagedSubcategoryResultRequestDto input)
        {
            try
            {
                var result = await base.GetAllAsync(input);
                await CurrentUnitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
