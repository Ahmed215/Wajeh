using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminDrivers.Dto
{
    public class AdminPagedDriverResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? CompanyId { get; set; }
        public int? SubcategoryId { get; set; }
        public bool? IsAvilible { get; set; }
    }
}
