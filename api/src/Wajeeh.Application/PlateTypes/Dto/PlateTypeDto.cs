﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Wajeeh.PlateTypes.Dto
{
    [AutoMap(typeof(PlateType))]
    public class PlateTypeDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}