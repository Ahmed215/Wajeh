using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminReuests.Dto
{
    public class AdminPagedRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? Status { get; set; }
        public int? SubcategoryId { get; set; }
        public int? CompanyId { get; set; }
        public string KeyWord { get; set; }
    }
}
