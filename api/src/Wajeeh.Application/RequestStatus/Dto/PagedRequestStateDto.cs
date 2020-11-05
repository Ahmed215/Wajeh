using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.RequestStatus.Dto
{
    public class PagedRequestStateDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
