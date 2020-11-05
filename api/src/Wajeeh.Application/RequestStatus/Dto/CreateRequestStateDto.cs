using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.RequestStatus.Dto
{
    [AutoMap(typeof(RequestState))]
    public class CreateRequestStateDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
