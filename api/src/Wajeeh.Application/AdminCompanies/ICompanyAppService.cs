using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminCompanies.Dto;
using Wajeeh.Categories.Dto;

namespace Wajeeh.AdminCompanies
{
    public interface ICompanyAppService : IAsyncCrudAppService<CompanyDto, long, PagedCompanyResultRequestDto, CreateCompanyDto, UpdateCompanyDto>
    {
    }
}
