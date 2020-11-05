using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Categories.Dto
{
    [AutoMap(typeof(Category))]
    public class CategoryDto : EntityDto<long>
    {
        public string DisplayName { get; set; }
        public string DisplayDescription { get; set; }
        public string Image { get; set; }
    }
}
