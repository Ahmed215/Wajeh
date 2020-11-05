using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.ClientAdresses;

namespace Wajeeh.AdminClientAdresses.Dto
{
    [AutoMap(typeof(ClientAdress))]
    public class AdminCreateClientAdressDto
    {
        public string Adress { get; set; }
        public string Title { get; set; }
        public string LangLat { get; set; }
    }
}
