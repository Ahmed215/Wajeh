using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPriceStatus.Dto
{
    public class PagedOfferPriceStatusDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
