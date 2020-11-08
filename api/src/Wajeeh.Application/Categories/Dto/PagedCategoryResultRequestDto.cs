using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Categories.Dto
{
    public class PagedCategoryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
