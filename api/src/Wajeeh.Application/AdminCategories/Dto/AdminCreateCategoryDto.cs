using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories;

namespace Wajeeh.AdminCategories.Dto
{
    [AutoMap(typeof(Category))]
    public class AdminCreateCategoryDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
    }
}
