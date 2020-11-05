using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories;

namespace Wajeeh.AdminSubcategories.Dto
{
    [AutoMap(typeof(Category))]
    public class AdminPagedSubcategoryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}
