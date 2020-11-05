using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.PlateTypes.Dto
{
    public class PagedPlateTypeDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
