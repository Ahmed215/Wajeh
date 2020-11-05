using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.ClientAdresses.Dto
{
    public class PagedClientAdressResultRequestDto: PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
