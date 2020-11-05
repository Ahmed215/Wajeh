using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Companies;

namespace Wajeeh.AdminCompanies.Dto
{
    [AutoMap(typeof(Company))]
    public class CompanyDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string DisplayName { get; set; }
        public string DisplayDescription { get; set; }
       
    }
}
