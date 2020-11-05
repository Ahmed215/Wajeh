using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPrices.Dto
{
    public class PagedRequestOfferPriceResultRequestDto: PagedAndSortedResultRequestDto
    {
        public long RequestId { get; set; }
    }
}
