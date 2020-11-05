using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories;

namespace Wajeeh.AdminCategories.Dto
{
    [AutoMap(typeof(Category))]
    public class AdminCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string DisplayName { get; set; }
        public string DisplayDescription { get; set; }
        public string Image { get; set; }
    }
}
