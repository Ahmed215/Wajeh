using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.AdminVechiles.Dto
{
    public class AdminPagedVechcileResultRequestDto: PagedAndSortedResultRequestDto
    {
        public string KeyWord { get; set; }
    }
}
