using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.CompanyClients;

namespace Wajeeh.AdminCompanyClients.Dto
{
    [AutoMap(typeof(CompanyClient))]
    public class AdminCreateCompanyClientDto
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public long CompanyId { get; set; }
    }
}
