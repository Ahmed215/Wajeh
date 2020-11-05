using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.ClientAdresses.Dto
{
    [AutoMap(typeof(ClientAdress))]
    public class ClientAdressDto : EntityDto<long>
    {
        public string Adress { get; set; }
        public string Title { get; set; }
        public string LangLat { get; set; }
    }
}
