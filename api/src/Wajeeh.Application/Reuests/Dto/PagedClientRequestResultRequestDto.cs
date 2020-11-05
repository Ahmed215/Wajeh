using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Reuests.Dto
{
    public class PagedClientRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? status { get; set; }
    }

    public enum UserType
    {
        Client=1,
        Driver=2
    }
}
