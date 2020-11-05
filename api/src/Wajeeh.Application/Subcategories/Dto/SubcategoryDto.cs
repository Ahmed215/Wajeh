using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Subcategories.Dto
{
    [AutoMap(typeof(Subcategory))]
    public class SubcategoryDto : EntityDto<long>
    {
        public string DisplayName { get; set; }
        public string DisplayDescription { get; set; }
        public string Image { get; set; }
        public long CategoryId { get; set; }
    }
}
