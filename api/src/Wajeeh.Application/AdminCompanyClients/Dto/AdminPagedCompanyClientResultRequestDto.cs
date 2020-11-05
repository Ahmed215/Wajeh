using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminCompanyClients.Dto
{
    public class AdminPagedCompanyClientResultRequestDto: PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? CompanyId { get; set; }
    }
}
