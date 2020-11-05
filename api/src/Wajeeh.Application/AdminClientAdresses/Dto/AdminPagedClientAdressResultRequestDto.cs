using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminClientAdresses.Dto
{
    public class AdminPagedClientAdressResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long ClientId { get; set; }
    }
}
