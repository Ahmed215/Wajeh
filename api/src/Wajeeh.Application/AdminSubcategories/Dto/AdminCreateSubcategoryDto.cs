﻿using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Subcategories;

namespace Wajeeh.AdminSubcategories.Dto
{
    [AutoMap(typeof(Subcategory))]
    public class AdminCreateSubcategoryDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        public long CategoryId { get; set; }
    }
}
