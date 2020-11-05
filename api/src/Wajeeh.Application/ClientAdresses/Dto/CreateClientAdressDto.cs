using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.ClientAdresses.Dto
{
    [AutoMap(typeof(ClientAdress))]
    public class CreateClientAdressDto
    {
        public string Adress { get; set; }
        public string Title { get; set; }
        public string LangLat { get; set; }
    }
}
