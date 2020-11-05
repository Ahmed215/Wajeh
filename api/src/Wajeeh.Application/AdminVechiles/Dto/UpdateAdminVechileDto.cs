using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Vechiles;

namespace Wajeeh.AdminVechiles.Dto
{
    [AutoMap(typeof(Vechile))]
    public class UpdateAdminVechileDto : EntityDto<long>
    {
        public string SequenceNumber { get; set; }
        public string Plate { get; set; }
        public int PlateType { get; set; }
    }
}
