using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Subcategories.Dto
{
    [AutoMap(typeof(Subcategory))]
    public class UpdateSubcategoryDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        public long CategoryId { get; set; }
    }
}
