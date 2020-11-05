using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminWaselDrivers.Dto
{
    public class AdminPagedWaselDriverResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string KeyWord { get; set; }
    }
}
