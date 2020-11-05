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
using Wajeeh.Companies;
using Wajeeh.AdminCompanies.Dto;
using Wajeeh.Authorization;
using Abp.Authorization;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace Wajeeh.AdminCompanies
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class CompanyAppService : AsyncCrudAppService<Company, CompanyDto, long, PagedCompanyResultRequestDto, CreateCompanyDto, UpdateCompanyDto>, ICompanyAppService
    {
        
        public CompanyAppService(IRepository<Company, long> repository) : base(repository)
        {
            
        }

        protected override IQueryable<Company> CreateFilteredQuery(PagedCompanyResultRequestDto input)
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
                catch (Exception ex)
                {
                    query = query.Where(t => t.Name == input.Keyword || t.NameAr == input.Keyword);
                }
            }
            return query;
        }
        protected override CompanyDto MapToEntityDto(Company entity)
        {
            var CompanyDto = base.MapToEntityDto(entity);
            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {
                CompanyDto.DisplayName = entity.NameAr;
                CompanyDto.DisplayDescription = entity.DescriptionAr;
               
            }
            else
            {
                CompanyDto.DisplayName = entity.Name;
                CompanyDto.DisplayDescription = entity.Description;
            }
           

            return CompanyDto;
        }

        [AbpAllowAnonymous]
        public override Task<PagedResultDto<CompanyDto>> GetAllAsync(PagedCompanyResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }
    }
}
